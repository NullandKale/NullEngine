using System;
using NullEngine.Entity;
using NullEngine.Component;

namespace NullGame.Component
{
    class cHealth : iComponent
    {
        public int currentHealth;
        int maxHealth;

        renderable PC;
        StateMachine.GameState gState;

        int delayAmount;
        int delayTimer;

        public cHealth(int totalHealth, renderable player, StateMachine.GameState gState, int delay)
        {
            maxHealth = totalHealth;
            currentHealth = totalHealth;

            PC = player;
            this.gState = gState;
            delayAmount = delay;
            delayTimer = delay;
        }

        public void Run(renderable r)
        {
            if(currentHealth <= 0)
            {
                die();
            }
            delayTimer--;
        }

        public void damage(int amount)
        {
            if(delayTimer <= 0)
            {
                delayTimer = delayAmount;
                currentHealth -= amount;
                Debug.Text("Player Health = " + currentHealth);
            }
        }

        public void heal(int amount)
        {
            if(currentHealth + amount > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += amount;
            }
        }

        public void resurrect()
        {
            currentHealth = maxHealth;
        }

        void die()
        {
            PC.active = false;
            gState.Gameover();
        }
    }
}
