using System;
using System.Collections.Generic;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using NullEngine.Managers;

namespace NullEngine
{
    public class Game
    {
        public static GameWindow window;
        public static int tick = 0;
        public static InputManager input;
        public static ButtonManager buttonMan;
        public static CollisionManager colMan;
        public static Queue<Action> renderQueue;
        public static TextureAtlas font;
        public static Texture2D buttonBackground;
        public static long frameTime;
        public static Random rng;
        public static bool DEBUG_doNotLoad_SETTOFALSE = true;

        public static StateMachine.iState currentState;

        public static List<Action> toUpdate;
        public static List<Action> lateUpdate;

        private Matrix4 projMatrix;

        public static int worldx = 0;
        public static int worldy = 0;
        public static int worldMaxX;
        public static int worldMaxY;

        //a global rect that contains the entire play space
        public static Rectangle worldRect;
        
        //a gobal rect that contains the window space
        public static Rectangle windowRect;

        public static int worldCenterX
        {
            get
            {
                return worldx + window.Width / 2;
            }
        }

        public static int worldCenterY
        {
            get
            {
                return worldy + window.Height / 2;
            }
        }

        private Stopwatch sw;

        public Game()
        {
            //init global RNG
            rng = new Random();

            toUpdate = new List<Action>();
            lateUpdate = new List<Action>();

            Settings.Load("settings.ini");

            //create a window and set graphics mode
            window = new GameWindow(Settings.xRes, Settings.yRes, new OpenTK.Graphics.GraphicsMode(Settings.colorDepth, Settings.bitDepth, 0, 0));
            window.Title = "NullEngine Test";

            //initialize window data
            worldMaxX = int.MaxValue;
            worldMaxY = int.MaxValue;
            worldRect = new Rectangle(0, 0, worldMaxX, worldMaxY);
            windowRect = new Rectangle(worldx, worldy, window.Width, window.Height);

            //initialize managers
            input = new InputManager();
            buttonMan = new ButtonManager();
            colMan = new CollisionManager(100);

            //initialize global textures
            font = new TextureAtlas("Game/Content/font.png", 16, 6, 8, 12, 0);
            buttonBackground = TextureManager.LoadTexture("Game/Content/buttonBackground.png", false);

            //inititialize frame timer;
            sw = new Stopwatch();
            frameTime = 0;

            //initialize render Queue
            renderQueue = new Queue<Action>();

            //add load update and ender functions to global call lists
            window.Load += window_Load;
            window.UpdateFrame += window_UpdateFrame;
            window.RenderFrame += window_RenderFrame;
        }

        void window_Load(object sender, EventArgs e)
        {
            //OpenGl initialization stuff
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);
        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            //update window data everyframe
            projMatrix = Matrix4.CreateOrthographicOffCenter(worldx, window.Width + worldx, window.Height + worldy, worldy, 0, 1);
            windowRect.X = worldx;
            windowRect.Y = worldy;
            worldRect = new Rectangle(0, 0, worldMaxX, worldMaxY);

            //invoke update functions
            foreach (Action up in toUpdate)
            {
                up.Invoke();
            }

            foreach (Action up in lateUpdate)
            {
                up.Invoke();
            }

            currentState.update();
        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {
            //OpenGL render stuff
            GL.ClearColor(Color.DimGray);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Load projection Matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);

            //while there is something left in the render queue run that renderer
            while (renderQueue.Count != 0)
            {
                renderQueue.Dequeue().Invoke();
            }

            window.SwapBuffers();

            //Do tick at END of frame.
            Tick();
            if(sw.IsRunning)
            {
                sw.Stop();
                frameTime = sw.ElapsedMilliseconds;
                sw.Reset();
                sw.Start();
            }
            else
            {
                sw.Start();
            }
        }

        public static void Run()
        {
            //set window run speed
            window.Run(1.0 / (float)Settings.updateRate);
        }

        //transform a point from screenspace to world space
        public static Point ScreenToWorldSpace(Point p)
        {
            return new Point(p.X + worldx, p.Y + worldy);
        }

        //set window center to said world point
        public static void SetWindowCenter(int x, int y)
        {
            worldx = (window.Width / 2) + x;
            worldy = (window.Height / 2) + y;
            windowRect = new Rectangle(worldx, worldy, window.Width, window.Height);
        }

        public static void SetWindowCenterOffset(int x, int y)
        {
            worldx = x - (window.Width / 2);
            worldy = y - (window.Height / 2);
            windowRect = new Rectangle(worldx, worldy, window.Width, window.Height);
        }

        //reset tick every 20 frames
        void Tick()
        {
            tick++;
            if (tick > 19)
            {
                tick = 0;
            }
        }

        //public exit function
        public static void exit()
        {
            Environment.Exit(0);
        }
    }
}