namespace CleanCode.VariableNames3.ArcadeGame.Bonuses
{
    public class Cherry : Bonus
    {
        public override int ScorePoints => 8;
        public override int CollectionArea => 1;
        public Cherry(Field field) : base(field){}
    }
}