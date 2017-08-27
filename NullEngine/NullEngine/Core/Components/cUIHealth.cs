using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nullEngine.Entity;

namespace nullEngine.Component
{
    class cUIHealth : iComponent
    {
        text uiEntity;
        cHealth pcHealth;
        int lastFrameHealth;

        public cUIHealth(text ui, cHealth playerHealth)
        {
            uiEntity = ui;
            pcHealth = playerHealth;
        }

        public void Run(renderable r)
        {
            if(lastFrameHealth != pcHealth.currentHealth)
            {
                uiEntity.ChangeText("Health: " + pcHealth.currentHealth);
            }
            lastFrameHealth = pcHealth.currentHealth;
        }
    }
}
