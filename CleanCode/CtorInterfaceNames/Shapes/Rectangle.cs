using System;

namespace CleanCode.CtorInterfaceNames.Shapes
{
    public class Rectangle : ShapeAbstract
    {
        public override double Area { get; }
        public override double Perimeter { get; }
        
        // 3.1 (2)
        // prev: public Rectangle(double length, double width) { ... }
        private Rectangle(double length, double width)
        {
            if (length <= 0 || width <= 0)
                throw new ArgumentException("Arguments must be > 0");

            Area = length * width;
            Perimeter = (length + width) * 2;
        }

        public static Rectangle CreateByLengthWidth(double length, double width)
        {
            return new Rectangle(length, width);
        }
    }
}