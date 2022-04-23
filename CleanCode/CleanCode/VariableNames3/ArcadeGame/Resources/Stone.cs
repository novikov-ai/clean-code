namespace CleanCode.VariableNames3.ArcadeGame.Resources
{
    public class Stone : Resource
    {
        public Stone(Field field) : base(field) { }

        public override string GetResource()
        {
            return "1 rock";
        }
    }
}