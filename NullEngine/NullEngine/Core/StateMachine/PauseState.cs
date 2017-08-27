using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace nullEngine.StateMachines
{
    class PauseState : iState
    {
        //keep a reference to all states that this state can move too
        GameState gState;
        MenuState mState;

        //keep a list of the update functions from all of the entities in this state
        List<Action> updaters;

        //keep the entities in this state
        Button returnToGameButton;
        Button optionsButton;
        Button exitToMenu;

        Button areYouSure;
        Button yes;
        Button no;

        //this is the strings for the buttons
        const string returnText = "Return to Game";
        const string optionsText = "Options";
        const string exitText = "Exit to Main Menu";

        const string areYouSureText = "This will delete all progress. Are you sure?";
        const string yesText = "Yes";
        const string noText = "No";

        bool isConfirmationOpen;

        public PauseState()
        {
            //get other states
            gState = GameStateManager.man.gState;
            mState = GameStateManager.man.mState;

            //initialize updaters list
            updaters = new List<Action>();

            //create buttons
            //for this button if it is pressed call toGameState
            returnToGameButton = new Button(returnText, Game.buttonBackground, toGameState, OpenTK.Input.MouseButton.Left, this);
            returnToGameButton.SetCenterPos(new Point(Game.window.Width / 2, Game.window.Height / 2));
            updaters.Add(returnToGameButton.update);

            //NOT IMPLEMENTED
            optionsButton = new Button(optionsText, Game.buttonBackground, "Not Implemented", OpenTK.Input.MouseButton.Left, this);
            optionsButton.SetCenterPos(new Point(Game.window.Width / 2, Game.window.Height / 2 + 60));
            updaters.Add(optionsButton.update);

            //for this button if it is pressed call confirmation
            exitToMenu = new Button(exitText, Game.buttonBackground, confirmation, OpenTK.Input.MouseButton.Left, this);
            exitToMenu.SetCenterPos(new Point(Game.window.Width / 2, Game.window.Height / 2 + 120));
            updaters.Add(exitToMenu.update);

            //this is used just for text
            areYouSure = new Button(areYouSureText, Game.buttonBackground, "", OpenTK.Input.MouseButton.Left, this);
            areYouSure.SetCenterPos(new Point(Game.window.Width / 2, Game.window.Height / 2));
            areYouSure.SetActive(false);
            updaters.Add(areYouSure.update);

            //for this button if it is pressed call toMenuState
            yes = new Button(yesText, Game.buttonBackground, toMenuState, OpenTK.Input.MouseButton.Left, this);
            yes.SetCenterPos(new Point(Game.window.Width / 2 - 60, Game.window.Height / 2 + 60));
            yes.SetActive(false);
            updaters.Add(yes.update);

            //for this button if it is pressed call confirmation
            no = new Button(noText, Game.buttonBackground, confirmation, OpenTK.Input.MouseButton.Left, this);
            no.SetCenterPos(new Point(Game.window.Width / 2 + 60, Game.window.Height / 2 + 60));
            no.SetActive(false);
            updaters.Add(no.update);

            //set confirmation open to false
            isConfirmationOpen = false;
        }

        public void enter()
        {
            Console.WriteLine("Entered PauseState");
        }

        public void update()
        {
            if(Game.worldx != 0)
            {
                Game.worldx = 0;
            }
            if (Game.worldy != 0)
            {
                Game.worldy = 0;
            }

            //check if the states are valid
            checkStates();

            //if escape pressed call toGameState
            if(Game.input.KeyFallingEdge(OpenTK.Input.Key.Escape))
            {
                toGameState();
            }

            //for each entity call the update function
            for (int i = 0; i < updaters.Count; i++)
            {
                updaters[i].Invoke();
            }
        }

        //change to gameState
        private void toGameState()
        {
            Console.WriteLine("Changing to GameState");
            GameStateManager.man.CurrentState = GameStateManager.man.gState;
            GameStateManager.man.CurrentState.enter();
        }

        //change to menueState
        private void toMenuState()
        {
            Console.WriteLine("Changing to MenuState");
            GameStateManager.man.CurrentState = GameStateManager.man.mState;
            mState.enter();
        }

        //toggle if the confirmation text is showm
        private void confirmation()
        {
            Console.WriteLine("Confirmation Called");
            isConfirmationOpen = !isConfirmationOpen;
            returnToGameButton.SetActive(!isConfirmationOpen);
            optionsButton.SetActive(!isConfirmationOpen);
            exitToMenu.SetActive(!isConfirmationOpen);

            areYouSure.SetActive(isConfirmationOpen);
            yes.SetActive(isConfirmationOpen);
            no.SetActive(isConfirmationOpen);
        }

        //ensure states are valid
        private void checkStates()
        {
            if(gState == null)
            {
                gState = GameStateManager.man.gState;
            }
            if(mState == null)
            {
                mState = GameStateManager.man.mState;
            }
        }
    }
}
