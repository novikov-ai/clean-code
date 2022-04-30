# About

Redesigned constructors overload and renamed bad interface names at 3 classes, 2 abstract classes and 1 interface according best practices:

- use static factory methods with names, describing arguments, when overloading constructors (eg. Complex fulcrumPoint = Complex.FromRealNumber(23.0)) => 3 examples

- don't use 'I' in front of interface names or prefixes for abstract classes => 3 examples


~~~
// example format

// 3.X (Y)
// <old name> - <new name> [3.2 only] 
code usage
~~~

### [Circle class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/Shapes/Circle.cs)

~~~
// 3.1 (1)
private Circle(double radius) { ... }
public static Circle CreateByRadius(double radius)
{
    return new Circle(radius);
}
~~~

### [Rectangle class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/Shapes/Rectangle.cs)

~~~
// 3.1 (2)
private Rectangle(double length, double width) { ... }

public static Rectangle CreateByLengthWidth(double length, double width)
{
    return new Rectangle(length, width);
}
~~~

### [Triangle class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/Shapes/Triangle.cs)

~~~
// 3.1 (3)
private Triangle(double sideA, double sideB, double sideC) { ... }

public static Triangle CreateByThreeSides(double sideA, double sideB, double sideC)
{
    return new Triangle(sideA, sideB, sideC);
}
~~~

### [ShapeAbstract abstract class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/Shapes/ShapeAbstract.cs)

~~~
// 3.2 (1)
// Shape - ShapeAbstract
public abstract class ShapeAbstract
{
    public abstract double Area { get;}
    public abstract double Perimeter { get; }
}
~~~

### [DataCrud interface](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/DAL/DataCrud.cs)

~~~
// 3.2 (2)
// ICrud - DataCrud
public interface DataCrud<T> { ... }
~~~

### [MonsterAbstract abstract class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CtorInterfaceNames/Monsters/MonsterAbstract.cs)

~~~
// 3.2 (3)
// Monster - MonsterAbstract
 public abstract class MonsterAbstract : Obstacle { ... }
~~~