using System;
using OpenTK.Input;
using System.Drawing;
using NullEngine.Entity;

namespace NullEngine
{
    public class Button
    {
        public text t;
        public quad background;
        public MouseButton button;
        public Action onClick;
        public StateMachine.iState containingState;

        private String echo;

        public Button(string text, Texture2D background, Action onClick, MouseButton buttonToCheck, StateMachine.iState state)
        {
            this.background = new quad(background, state);
            this.background.doDistCulling = false;
            t = new text(text, state);
            this.background.width = t.tex.width;
            this.background.height = t.tex.height;
            this.onClick = onClick;
            button = buttonToCheck;
            Game.buttonMan.Add(this);
            containingState = state;
        }

        public Button(string text, Texture2D background, String toEcho, MouseButton buttonToCheck, StateMachine.iState state)
        {
            this.background = new quad(background, state);
            this.background.doDistCulling = false;
            t = new text(text, state);
            this.background.width = t.tex.width;
            this.background.height = t.tex.height;

            if (text == " ")
            {
                this.background.width = 16;
                this.background.height = 16;
            }

            this.echo = toEcho;
            onClick = Echo;
            button = buttonToCheck;
            Game.buttonMan.Add(this);
            containingState = state;
        }

        public Button(int size, string text, Texture2D background, String toEcho, MouseButton buttonToCheck, StateMachine.iState state)
        {
            this.background = new quad(background, state);
            this.background.doDistCulling = false;
            t = new text(text, state);
            this.background.width = size;
            this.background.height = size;

            this.echo = toEcho;
            onClick = Echo;
            button = buttonToCheck;
            Game.buttonMan.Add(this);
            containingState = state;
        }

        public void update()
        {
            background.update();
            t.update();
        }

        private void Echo()
        {
            Debug.Text(echo);
        }

        public void SetScale(float scale)
        {
            t.pos.xScale = scale;
            t.pos.yScale = scale;
        }

        public void SetPos(Point p)
        {
            t.pos.xPos = p.X;
            t.pos.yPos = p.Y;
            background.pos.xPos = p.X;
            background.pos.yPos = p.Y;
        }

        public void SetPos(int X, int Y)
        {
            t.pos.xPos = X;
            t.pos.yPos = Y;
            background.pos.xPos = X;
            background.pos.yPos = Y;
        }

        public void SetCenterPos(Point p)
        {
            t.pos.xPos = p.X - (t.tex.width * transform.masterScale / 2);
            t.pos.yPos = p.Y - (t.tex.height * transform.masterScale / 2);
            background.pos.xPos = p.X - (t.tex.width * transform.masterScale / 2) - 5;
            background.pos.yPos = p.Y - (t.tex.height * transform.masterScale / 2) - 5;
        }

        public void SetCenterPos(int x, int y)
        {
            t.pos.xPos = x - (t.tex.width * transform.masterScale / 2);
            t.pos.yPos = y - (t.tex.height * transform.masterScale / 2);
            background.pos.xPos = x - (t.tex.width * transform.masterScale / 2) - 5;
            background.pos.yPos = y - (t.tex.height * transform.masterScale / 2) - 5;
        }

        public void SetActive(bool b)
        {
            t.active = b;
            background.active = b;
        }
    }
}
