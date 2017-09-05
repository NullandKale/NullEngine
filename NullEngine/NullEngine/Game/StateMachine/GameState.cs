using System;
using System.Collections.Generic;
using NullEngine;
using NullEngine.Entity;
using NullEngine.Component;
using NullEngine.Managers;
using NullEngine.StateMachine;
using System.Drawing;
using NullGame.Component;

namespace NullGame.StateMachine
{
    class GameState : iState
    {
        // keep a reference to the state it can change to
        PauseState pState;
        MenuState mState;

        // keep a list of the contained entities update functions
        List<Action> updaters;

        //game manager singletons
        Managers.EnemyManager eMan;
        Managers.WorldManager wMan;

        //general game entities and components
        public quad background;
        public Button gameover;

        //player related game entities and components
        public quad playerCharacter;
        public cHealth playerHealth;

        //enemy related game entities and components

        //UI elements
        public Button uiHealth;
        public Button uiLevel;
        public Button uiPos;

        private Point LastWorldPos;

        public GameState()
        {
            //get a reference to pause state
            pState = GameStateManager.man.pState;
            mState = GameStateManager.man.mState;

            //initialize list of entity updaters and the collision manager singleton
            updaters = new List<Action>();
            int seed = 5; //Game.rng.Next();
            wMan = new Managers.WorldManager(seed, 10, 100, 10d, 64, Game.colMan, new Point(0,0));
            Game.worldMaxX = wMan.worldMaxX;
            Game.worldMaxY = wMan.worldMaxY;
            LastWorldPos = new Point(Game.worldCenterX, Game.worldCenterY);

            //initialize background entity
            background = new quad(Managers.WorldManager.worldTex, this);
            background.AddComponent(new cBackgroundManger());
            updaters.Add(background.update);

            //initialize player character entity
            playerCharacter = new quad("Game/Content/roguelikeCharBeard_transparent.png", this);
            playerCharacter.AddComponent(new cFollowCamera(playerCharacter));
            cCollider playerCollider = new cCollider(playerCharacter);
            cMouseFire playerBulletMan = new cMouseFire(playerCharacter);
            playerHealth = new cHealth(10, playerCharacter, this, 30);
            playerCharacter.AddComponent(playerHealth);
            playerCharacter.AddComponent(playerCollider);
            playerCharacter.AddComponent(new cKeyboardMoveandCollide(5, 1.5f, playerCollider));
            playerCharacter.AddComponent(playerBulletMan);
            playerCharacter.pos.xPos = Game.window.Width / 2 + 10;
            playerCharacter.pos.yPos = Game.window.Height / 2 + 10;
            playerCharacter.AddComponent(new cDEBUG_POS());
            playerCharacter.AddComponent(new cRangedWeapon(playerCharacter, playerBulletMan, 10, this));
            playerCharacter.tag = "Player";
            updaters.Add(playerCharacter.update);

            //initialize enemy manager
            eMan = new Managers.EnemyManager(playerCharacter, playerHealth, 1000, this);
            updaters.Add(eMan.update);

            //initialize UI entities
            gameover = new Button("Game Over. Click to go to Main Menu", Game.buttonBackground, toMenuState, OpenTK.Input.MouseButton.Left, this);
            gameover.SetActive(false);
            updaters.Add(gameover.update);

            uiHealth = new Button("Health: 00", Game.buttonBackground, "", OpenTK.Input.MouseButton.Left, this);
            uiHealth.t.AddComponent(new cUIHealth(uiHealth.t, playerHealth));
            updaters.Add(uiHealth.update);

            uiLevel = new Button("Level 00", Game.buttonBackground, "", OpenTK.Input.MouseButton.Left, this);
            uiLevel.t.AddComponent(new cUILevel(uiLevel.t));
            updaters.Add(uiLevel.update);

            uiPos = new Button("[0,0] {00,00}", Game.buttonBackground, "", OpenTK.Input.MouseButton.Left, this);
            uiPos.t.AddComponent(new cUIPosition(uiPos.t));
            updaters.Add(uiPos.update);
        }

        //called whenever a state is entered
        public void enter()
        {
            Console.WriteLine("Entered GameState");
            Game.SetWindowCenter(LastWorldPos.X, LastWorldPos.Y);
        }

        public void update()
        {
            if (!gameover.t.active)
            {
                //check that all the states that this state can transititon to 
                checkStates();

                //if escape pressed transition to pause state
                if (Game.input.KeyFallingEdge(OpenTK.Input.Key.Escape))
                {
                    toPauseState();
                }

                if (Game.input.KeyFallingEdge(OpenTK.Input.Key.G))
                {
                    wMan.currentChunk.inDungeon = true;
                }

                if (Game.input.KeyFallingEdge(OpenTK.Input.Key.B))
                {
                    wMan.currentChunk.inDungeon = false;
                }


                //run all entities update functions
                for (int i = 0; i < updaters.Count; i++)
                {
                    updaters[i].Invoke();
                }
                updateUI();
            }
            else
            {
                gameover.update();
            }
        }

        private void updateUI()
        {
            uiHealth.SetPos(new Point(Game.windowRect.X, Game.windowRect.Bottom - uiHealth.background.height * transform.masterScale));
            uiLevel.SetPos(new Point(Game.windowRect.X, Game.windowRect.Y));
            uiPos.SetPos(new Point(Game.windowRect.X, Game.windowRect.Y + 48));
        }

        private void toPauseState()
        {
            Console.WriteLine("Changing to PauseState");
            GameStateManager.man.CurrentState = GameStateManager.man.pState;
            LastWorldPos = new Point(Game.worldCenterX, Game.worldCenterY);
            pState.enter();
        }

        private void checkStates()
        {
            if (pState == null || mState == null)
            {
                mState = GameStateManager.man.mState;
                pState = GameStateManager.man.pState;
            }
        }

        public void Gameover()
        {
            gameover.SetCenterPos(Game.windowRect.Width / 2, Game.windowRect.Height / 2);
            gameover.SetActive(true);
            Game.SetWindowCenter(-Game.window.Width / 2, -Game.window.Height / 2);
            Managers.EnemyManager.man.Reset();
        }

        private void toMenuState()
        {
            gameover.SetActive(false);
            playerCharacter.active = true;
            playerHealth.resurrect();
            Console.WriteLine("Changing to MenuState");
            GameStateManager.man.CurrentState = GameStateManager.man.mState;
            LastWorldPos = new Point(0,0);
            mState.enter();
        }
    }
}
