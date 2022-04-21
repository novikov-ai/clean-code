namespace CleanCode.VariableNames3.ArcadeGame.Bonuses
{
    public abstract class Bonus : Obstacle
    {
        public Bonus(Field field) : base(field) { }
        
        public abstract int ScorePoints { get; }
        public abstract int CollectionArea { get; }

        public void RemoveBonus(Player player)
        {
            player.ScorePoints += ScorePoints;
            _field.Obstacles.Remove(this);
        }
    }
}