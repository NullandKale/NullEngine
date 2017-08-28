using System;
using System.Collections.Generic;
using System.Drawing;

namespace nullEngine.StateMachines
{
    class MenuState : iState
    {
        //keep a reference to all states that this state can move to
        GameState gState;

        //keep a list of the contained entities update functions
        List<Action> updaters;

        //all this states entities
        Button newGameButton;
        Button loadGameButton;
        Button settingsButton;
        Button exitButton;

        //all of the text for the buttons
        const string newGameText = "Start New Game";
        const string loadGameText = "Load Game - Not Implemented";
        const string settingsText = "Settings - Not Implemented";
        const string exitText = "Exit To Desktop";

        public MenuState()
        {
            //get a reference to the nessesary states
            gState = GameStateManager.man.gState;

            //inititalize list of entity update functions
            updaters = new List<Action>();

            //create all buttons
            //this button calls toGameState
            newGameButton = new Button(newGameText, Game.buttonBackground, toGameState, OpenTK.Input.MouseButton.Left, this);
            newGameButton.SetCenterPos(new Point(Game.window.Width / 2, Game.window.Height / 2));
            updaters.Add(newGameButton.update);

            //NOT IMPLEMENTED
            loadGameButton = new Button(loadGameText, Game.buttonBackground, "Not Implemented", OpenTK.Input.MouseButton.Left, this);
            loadGameButton.SetCenterPos(new Point(Game.window.Width / 2, (Game.window.Height / 2) + 60));
            updaters.Add(loadGameButton.update);

            //NOT IMPLEMENTED
            settingsButton = new Button(settingsText, Game.buttonBackground, "Not Implemented Either", OpenTK.Input.MouseButton.Left, this);
            settingsButton.SetCenterPos(new Point(Game.window.Width / 2, (Game.window.Height / 2) + 120));
            updaters.Add(settingsButton.update);

            //this button calls exit
            exitButton = new Button(exitText, Game.buttonBackground, exit, OpenTK.Input.MouseButton.Left, this);
            exitButton.SetCenterPos(new Point(Game.window.Width / 2, (Game.window.Height / 2) + 180));
            updaters.Add(exitButton.update);
        }

        public void enter()
        {
            Console.WriteLine("Entered MenuState");
            Game.SetWindowCenter(-Game.window.Width / 2, -Game.window.Height / 2);
        }

        public void update()
        {
            //check that the nessesary states are valid
            checkStates();

            //for each entity call the update function
            for(int i = 0; i < updaters.Count; i++)
            {
                updaters[i].Invoke();
            }
        }

        private void checkStates()
        {
            if (gState == null)
            {
                gState = GameStateManager.man.gState;
            }
        }

        //exit game
        private void exit()
        {
            Console.WriteLine("Good Bye!");
            Program.exit();
        }

        //transition to game state
        private void toGameState()
        {
            Console.WriteLine("Changing to GameState");
            GameStateManager.man.CurrentState = GameStateManager.man.gState;
            gState.enter();
        }

    }
}
