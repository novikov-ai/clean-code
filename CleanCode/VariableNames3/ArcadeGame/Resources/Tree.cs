using System;

namespace CleanCode.VariableNames3.ArcadeGame.Resources
{
    public class Tree : Resource
    {
        public Tree(Field field) : base(field) { }

        public override string GetResource()
        {
            return "1 pack of wood";
        }
    }
}