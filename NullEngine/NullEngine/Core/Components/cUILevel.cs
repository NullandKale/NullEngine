using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.Entity___Component
{
    class cUILevel : iComponent
    {
        text uiEntity;
        int lastFrameLevel;

        public cUILevel(text ui)
        {
            uiEntity = ui;
        }

        public void Run(renderable r)
        {
            if (lastFrameLevel != Managers.EnemyManager.man.level)
            {
                    uiEntity.ChangeText("Level " + Managers.EnemyManager.man.level);
            }
            lastFrameLevel = Managers.EnemyManager.man.level;
        }
    }
}
