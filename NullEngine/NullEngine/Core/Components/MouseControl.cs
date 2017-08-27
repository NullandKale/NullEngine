using nullEngine.Entity;

namespace nullEngine.Component
{
    class MouseControl : iComponent
    {
        public void Run(renderable r)
        {
            r.pos.xPos = Game.input.mousePos.X;
            r.pos.yPos = Game.input.mousePos.Y;
        }
    }
}
