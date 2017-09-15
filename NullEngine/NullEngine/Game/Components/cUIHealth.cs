using NullEngine.Component;
using NullEngine.Entity;

namespace NullGame.Component
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

        public void OnDestroy(renderable r)
        {
            //DO NOTHING
        }

    }
}
