using System;
using System.Collections.Generic;
using System.Drawing;

namespace CleanCode.VariableNames3.ArcadeGame
{
    public class Obstacle
    {
        private protected readonly Field _field;
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Obstacle(Field field)
        {
            if (field is null)
                throw new ArgumentNullException("Argument input is null");

            _field = field;

            Random random = new Random();
            PositionX = random.Next(0, _field.Width);
            PositionY = random.Next(0, _field.Height);

            field.AddToField(this);
        }

        public bool DoesIntersect(Obstacle obstacle)
        {
           return obstacle.PositionX == PositionX && obstacle.PositionY == PositionY;
        }
    }
}