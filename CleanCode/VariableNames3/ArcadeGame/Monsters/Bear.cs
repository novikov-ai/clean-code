namespace CleanCode.VariableNames3.ArcadeGame.Monsters
{
    public class Bear : Monster
    {
        public override int AttackRadius => 5;
        public override int Speed => 3;

        public Bear(Field field) : base(field) { }
    }
}