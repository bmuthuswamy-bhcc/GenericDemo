using System;
using System.Collections.Generic;

namespace GenericDemo {

    /* Illustration of type parameter constraints
     * We are going to restrict T to a "numeric" type (int or double)
     * Unfortunately, there is no explicit constraint to define a numeric type
     * but the constraints below represent the combination.
     * For details, refer to p. 194 in your textbook
     */
    class GenericDemo<T>
        where T : struct,
                  IComparable, IComparable<T>,
                  IConvertible,
                  IEquatable<T>,
                  IFormattable
        { 
        public T value { get; private set; }

        public GenericDemo(T value) {
            this.value = value;
        }

        public override string ToString() => $"{typeof(T)} : {this.value}";
    }
    
    
    public interface IShape {
        public double Area { get; }
    }

    public class Square : IShape {
        public double length { get; set; }

        public Square(double length) {
            this.length = length;
        }

        public double Area => this.length * this.length;
    }

    public class Circle : IShape {
        public double radius { get; set; }

        public Circle(double radius) {
            this.radius = radius;
        }
        public double Area => Math.PI * this.radius * this.radius;
    }

    /* We are going to implement Shape comparison:
     * 1.  Compare two shapes, irrespctive of the type of shape
     * 2.  Compare a shape to itself
     * 
     * For 1., we will design a ShapeComparer class that will compare the 
     * areas of two shapes s1 and s2.  It will return:
     * -1 if area(s1) < area(s2)
     * 0 if area(s1) == area(s2)
     * 1 if area(s1) > area(s2)
     * 
     * These classes are going to implement the Compare method from interface IComparer, which is
     * defined as follows:
     * public interface IComparer<in T>  {
     *   int Compare(T x, T y);
     * }
     * 
     * The keyword in (input) with the type parameter makes this interface contravariant.   Because of this,
     * it is possible to pass IShape objects to IComparer
     */
    public class ShapeComparer : IComparer<IShape> {
        public int Compare(IShape s1, IShape s2) {
            if (s1 is null) { // corner case
                return s2 is null ? 0 : -1;
            }
            if (s2 is null) {
                return 1; // If shape 2 does not exist, obviously shape 1 has a larger (non-zero) area
            }
            return s1.Area.CompareTo(s2.Area);
        }
    }

    public class CircleComparer : IComparer<Circle> {
        public int Compare(Circle c1, Circle c2) {
            if (c1 is null) {
                return c2 is null ? 0 : -1;
            }
            if (c2 is null) {
                return 1;
            }
            return c1.radius.CompareTo(c2.radius);

        }
    }
    class Program {
        static void Main(string[] args) {
            var obj1 = new GenericDemo<int>(33);
            //We can't use string type anymore with GenericDemo because of type parameter restrictions to "numeric"
            //var obj2 = new GenericDemo<string>("Hello, world!");
            Console.WriteLine(obj1);
            //Console.WriteLine(obj2);

            Square sq1 = new Square(2);
            Circle c1 = new Circle(1);
            Circle c2 = new Circle(1);
            Console.WriteLine($"The area of square is {sq1.Area} unit^2");
            Console.WriteLine($"The area of circle is {c1.Area} unit^2");

            ShapeComparer objShapeComparer = new ShapeComparer();
            if (objShapeComparer.Compare(sq1,c1) > 0) {
                Console.WriteLine($"{sq1} has larger area than {c1}");
            }
            // do the other cases:  when Compare == 0 and when Compare < 0

            CircleComparer objCircleComparer = new CircleComparer();
            if (objCircleComparer.Compare(c1,c2) == 0) {
                Console.WriteLine("Both circles have the same radius");
            }
        }
    }
}
