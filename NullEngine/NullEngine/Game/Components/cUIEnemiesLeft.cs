using NullEngine.Component;
using NullEngine.Entity;

namespace NullGame.Component
{
    class cUIEnemiesLeft : iComponent
    {
        text uiEntity;
        int lastFrameEnemiesLeft;

        public cUIEnemiesLeft(text ui)
        {
            uiEntity = ui;
        }

        public void Run(renderable r)
        {
            if (lastFrameEnemiesLeft != Managers.EnemyManager.man.enemiesLeft)
            {
                if(Managers.EnemyManager.man.enemiesLeft == 2)
                {
                    uiEntity.ChangeText(1 + " enemies left");
                }
                else
                {
                    uiEntity.ChangeText((Managers.EnemyManager.man.enemiesLeft / 2) + 1 + " enemies left");
                }
            }
            lastFrameEnemiesLeft = Managers.EnemyManager.man.enemiesLeft;
        }

        public void OnDestroy(renderable r)
        {
            //DO NOTHING
        }
    }
}
