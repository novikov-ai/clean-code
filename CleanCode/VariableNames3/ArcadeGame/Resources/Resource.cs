namespace CleanCode.VariableNames3.ArcadeGame.Resources
{
    public abstract class Resource : Obstacle
    {
        public Resource(Field field) : base(field) { }

        public virtual string GetResource()
        {
            return "resource";
        }
    }
}