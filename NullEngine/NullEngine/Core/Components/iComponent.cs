namespace NullEngine.Component
{
    public interface iComponent
    {
        void Run(NullEngine.Entity.renderable r);
        void OnDestroy(NullEngine.Entity.renderable r);
    }
}
