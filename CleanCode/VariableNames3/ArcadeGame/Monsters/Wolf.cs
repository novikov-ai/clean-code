namespace CleanCode.VariableNames3.ArcadeGame.Monsters
{
    public class Wolf : Monster
    {
        public override int AttackRadius => 3;
        public override int Speed => 5;

        public Wolf(Field field) : base(field) { }
    }
}