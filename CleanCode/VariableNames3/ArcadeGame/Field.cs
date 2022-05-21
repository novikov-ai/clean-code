using System;
using System.Collections.Generic;

namespace CleanCode.VariableNames3.ArcadeGame
{
    public class Field
    {
        public readonly int Width;
        public readonly int Height;

        public List<Obstacle> Obstacles;

        public Field(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Argument input must be > 0");
            
            Width = width;
            Height = height;

            Obstacles = new List<Obstacle>();
        }

        public void AddToField(Obstacle obstacle)
        {
            if (obstacle is null)
                throw new ArgumentNullException("Argument input is null");

            Obstacles.Add(obstacle);
        }
    }
}