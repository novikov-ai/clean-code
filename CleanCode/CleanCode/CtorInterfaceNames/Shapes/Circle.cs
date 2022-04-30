using System;

namespace CleanCode.CtorInterfaceNames.Shapes
{
    public class Circle : ShapeAbstract
    {
        public override double Area { get; }
        public override double Perimeter { get; }
        
        public static Circle CreateByRadius(double radius)
        {
            return new Circle(radius);
        }
        
        // 3.1 (1)
        // prev: public Circle(double radius) { ... }
        private Circle(double radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Argument must be > 0");
            
            Area = Math.PI * Math.Pow(radius, 2);
            Perimeter = 2 * Math.PI * radius;
        }
    }
}