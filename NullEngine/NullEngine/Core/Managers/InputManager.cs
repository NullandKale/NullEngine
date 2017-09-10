using OpenTK;
using OpenTK.Input;
using System.Drawing;

namespace NullEngine.Managers
{
    public class InputManager
    {
        //keyboard state storage, one for the current frame and one for the last frame
        private KeyboardState lastKeyState;
        private KeyboardState currentKeyState;

        //mouse state storage, one for the current frame and one for the last frame
        private MouseState lastMouseState;
        private MouseState currentMouseState;

        //current frame mouse pos
        public Point mousePos;

        public InputManager()
        {
            //inititalize mousePos and add update function to global update call list
            mousePos = new Point();
            Game.window.UpdateFrame += update;
        }

        public void update(object sender, FrameEventArgs e)
        {
            //on update if the currentKeyState is not invalid set lastKeyState to the old currentKeyState
            if(currentKeyState != null)
            {
                lastKeyState = currentKeyState;
            }

            //update currentKeyState
            currentKeyState = Keyboard.GetState();

            //if the currentMouseState is not invalid set the lastMouseState to the old currentMouseState
            if (currentMouseState != null)
            {
                lastMouseState = currentMouseState;
            }

            //update currentMouseState
            currentMouseState = Mouse.GetCursorState();

            //update mousePos
            mousePos = Game.window.PointToClient(new Point(currentMouseState.X, currentMouseState.Y));

        }
        //mouse state check functions
        public bool isClickedFalling(MouseButton b)
        {
            if(Game.window.Focused)
            {
                return currentMouseState.IsButtonUp(b) && lastMouseState.IsButtonDown(b);
            }
            else
            {
                return false;
            }
        }

        public bool isClickedRising(MouseButton b)
        {
            if (Game.window.Focused)
            {
                return currentMouseState.IsButtonDown(b) && lastMouseState.IsButtonUp(b);
            }
            else
            {
                return false;
            }
        }

        //keyboard state functions
        public bool KeyRisingEdge(Key k)
        {
            if(!isKeystateValid() && !Game.window.Focused)
            {
                return false;
            }
            else
            {
                return lastKeyState.IsKeyDown(k) && currentKeyState.IsKeyUp(k);
            }
        }

        public bool KeyFallingEdge(Key k)
        {
            if (!isKeystateValid() && !Game.window.Focused)
            {
                return false;
            }
            else
            {
                return lastKeyState.IsKeyUp(k) && currentKeyState.IsKeyDown(k);
            }
        }

        public bool KeyHeld(Key k)
        {
            if (!isKeystateValid() && !Game.window.Focused)
            {
                return false;
            }
            else
            {
                return lastKeyState.IsKeyDown(k) && currentKeyState.IsKeyDown(k);
            }
        }

        //check that the keyboard state is valid | this might not be needed
        private bool isKeystateValid()
        {
            return currentKeyState != null && lastKeyState != null;
        }
    }
}
