using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using NullEngine.StateMachine;

namespace NullEngine.Managers
{
    class ButtonManager
    {
        //keep a list of references to the buttons
        public List<Button> Buttons = new List<Button>();

        public ButtonManager()
        {
            //add update function to global update call list
            Game.window.UpdateFrame += update;
        }

        public void update(object sender, FrameEventArgs e)
        {
            //get mouse state
            bool doLeft = Game.input.isClickedRising(MouseButton.Left);
            bool doRight = Game.input.isClickedRising(MouseButton.Right);
            bool doMiddle = Game.input.isClickedRising(MouseButton.Middle);

            //if any mouse buttons are pressed continue
            if(doLeft || doRight || doMiddle)
            {
                //for every button in the button list
                for(int i = 0; i < Buttons.Count; i++)
                {
                    //check if the button is active and the current gamestate contains the button
                    if(Buttons[i].background.active && GameStateManager.man.CurrentState == Buttons[i].containingState)
                    {
                        //check if the button wants a right click
                        if (Buttons[i].button == MouseButton.Right && doRight)
                        {
                            //if the mouse pos is on top of the button
                            if (isWithin(Buttons[i], Game.input.mousePos))
                            {
                                //invoke the onclick behavior
                                Buttons[i].onClick.Invoke();
                            }
                        }

                        //check if the button wants a left click
                        if (Buttons[i].button == MouseButton.Left && doLeft)
                        {
                            //if the mouse pos is on top of the button
                            if (isWithin(Buttons[i], Game.input.mousePos))
                            {
                                //invoke the onclick behavior
                                Buttons[i].onClick.Invoke();
                            }
                        }

                        //check if the button wants a middle click
                        if (Buttons[i].button == MouseButton.Middle && doMiddle)
                        {
                            //if the mouse pos is on top of the button
                            if (isWithin(Buttons[i], Game.input.mousePos))
                            {
                                //invoke the onclick behavior
                                Buttons[i].onClick.Invoke();
                            }
                        }
                    }
                }
            }
        }

        //return true if the point interects with the button
        public bool isWithin(Button b, Point p)
        {
            p = Game.ScreenToWorldSpace(p);
            return (p.X > (int)b.background.pos.xPos && p.Y > (int)b.background.pos.yPos && p.X < ((int)b.background.pos.xPos + b.background.width * 4) && p.Y < ((int)b.background.pos.yPos + b.background.height * 4));
        }

        //add the button to the buttons list
        public void Add(Button b)
        {
            Buttons.Add(b);
        }
    }
}
