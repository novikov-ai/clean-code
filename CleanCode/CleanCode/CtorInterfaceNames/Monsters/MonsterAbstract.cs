using System;
using CleanCode.VariableNames3.ArcadeGame;

namespace CleanCode.CtorInterfaceNames.Monsters
{
    // 3.2 (3)
    // Monster - MonsterAbstract
    public abstract class MonsterAbstract : Obstacle
    {
        public MonsterAbstract(Field field) : base(field)
        {
            if (field is null)
                throw new ArgumentNullException("Argument input is null");
        }

        public abstract int AttackRadius { get; }
        public abstract int Speed { get; }

        public void Move()
        {
            int oldPosition = PositionX;
            bool isDirectionX = true;

            Random random = new Random();

            if (random.Next(0, 2) == 0) // direction: X or Y
            {
                if (random.Next(0, 2) == 0) // X: left or right
                {
                    PositionX = PositionX + Speed <= _field.Width ? PositionX + Speed : PositionX;
                }
                else
                {
                    PositionX = PositionX - Speed > 0 ? PositionX - Speed : PositionX;
                }
            }
            else
            {
                isDirectionX = false;
                
                oldPosition = PositionY;

                if (random.Next(0, 2) == 0) // Y: up or down
                {
                    PositionY = PositionY + Speed <= _field.Height ? PositionY + Speed : PositionY;
                }
                else
                {
                    PositionY = PositionY - Speed > 0 ? PositionY - Speed : PositionY;
                }
            }

            foreach (var obstacle in _field.Obstacles)
            {
                if (obstacle.DoesIntersect(this))
                {
                    if (obstacle is Player player)
                    {
                        player.Alive = false;
                        return;
                    }

                    if (isDirectionX)
                        PositionX = oldPosition;
                    else
                        PositionY = oldPosition;

                    return;
                }
            }        
        }

        public bool CanAttack(Player player)
        {
            return Math.Abs(PositionX - player.PositionX) <= AttackRadius &&
                 Math.Abs(PositionY - player.PositionY) <= AttackRadius;
        }
    }
}