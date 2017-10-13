using System;
using System.Collections.Generic;
using NullEngine;
using NullEngine.Entity;
using NullEngine.Component;
using System.Drawing;

namespace NullGame
{
    class TestState : NullEngine.StateMachine.iState
    {
        public Button increaseCount;
        public Button decreaseCount;
        public Button numberOfColliders;

        public int colliderCount = 500;

        public List<quad> colliders;

        public cCollider TopWall;
        public cCollider BottomWall;
        public cCollider LeftWall;
        public cCollider RightWall;

        private List<Action> updaters;

        public TestState()
        {
            updaters = new List<Action>();
            colliders = new List<quad>();

            increaseCount = new Button("+", Game.buttonBackground, increaseColCount, OpenTK.Input.MouseButton.Left, this);
            increaseCount.SetPos(10, 10);

            numberOfColliders = new Button(colliderCount.ToString() + "    ", Game.buttonBackground, "", OpenTK.Input.MouseButton.Left, this);
            numberOfColliders.SetPos(60, 10);

            decreaseCount = new Button("-", Game.buttonBackground, decreaseColCount, OpenTK.Input.MouseButton.Left, this);
            decreaseCount.SetPos(240, 10);

            TopWall = new cCollider(new Rectangle(Game.windowRect.Left, Game.windowRect.Top - 10, 1920, 10), this);
            BottomWall = new cCollider(new Rectangle(Game.windowRect.Left, Game.windowRect.Bottom, 1920, 10), this);

            LeftWall = new cCollider(new Rectangle(Game.windowRect.Left - 10, Game.windowRect.Top , 10, 1920), this);
            RightWall = new cCollider(new Rectangle(Game.windowRect.Right, Game.windowRect.Top, 10, 1920), this);
        }

        public void update()
        {
            foreach (Action update in updaters)
            {
                update.Invoke();
            }

            while(colliderCount > colliders.Count)
            {
                colliders.Add(createEntity());
            }

            while(colliderCount < colliders.Count)
            {
                removeEntity();
            }
        }

        void increaseColCount()
        {
            if(colliderCount < 100)
            {
                colliderCount++;
            }
            else if(colliderCount < 1000)
            {
                colliderCount += 5;
            }
            else if(colliderCount < 5000)
            {
                colliderCount += 100;
            }

            numberOfColliders.t.ChangeText(colliderCount.ToString());
        }

        void decreaseColCount()
        {
            if(colliderCount == 0)
            {
                //DO NOTHING
            }
            else if (colliderCount < 100)
            {
                colliderCount--;
            }
            else if (colliderCount < 1000)
            {
                colliderCount -= 5;
            }
            else if (colliderCount < 5000)
            {
                colliderCount -= 100;
            }

            numberOfColliders.t.ChangeText(colliderCount.ToString());
        }

        public quad createEntity()
        {
            quad temp = new quad("Game/Content/bullet.png", this);
            temp.AddComponent(new Components.cBounce(temp));
            temp.pos.xScale = 0.75f;
            temp.pos.yScale = 0.75f;
            temp.pos.xPos = Game.rng.Next(Game.windowRect.Left, Game.windowRect.Right);
            temp.pos.yPos = Game.rng.Next(Game.windowRect.Top, Game.windowRect.Bottom);
            return temp;
        }

        public void removeEntity()
        {
            int last = colliders.Count - 1;
            updaters.Remove(colliders[last].update);
            colliders[last].OnDestroy();
            colliders.RemoveAt(last);
        }

        public void addUpdater(Action toAdd)
        {
            updaters.Add(toAdd);
        }

        public void enter()
        {
            throw new NotImplementedException();
        }
    }
}
