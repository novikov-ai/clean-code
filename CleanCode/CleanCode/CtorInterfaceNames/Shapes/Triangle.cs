using System;

namespace CleanCode.CtorInterfaceNames.Shapes
{
    public class Triangle : ShapeAbstract
    {
        public override double Area { get; }
        public override double Perimeter { get; }

        // 3.1 (3)
        // prev: public Triangle(double sideA, double sideB, double sideC) { ... }
        private Triangle(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                throw new ArgumentException("Arguments must be > 0");

            Perimeter = sideA + sideB + sideC;
            double halfPerimeter = Perimeter / 2;

            Area = Math.Sqrt(
                halfPerimeter * (halfPerimeter - sideA) * (halfPerimeter - sideB) * (halfPerimeter - sideC));
        }

        public static Triangle CreateByThreeSides(double sideA, double sideB, double sideC)
        {
            return new Triangle(sideA, sideB, sideC);
        }
    }
}