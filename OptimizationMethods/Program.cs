using MathNet.Symbolics;
using OptimizationMethods.FirstOrderMethods;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a = -2;
            double b = -0.5;
            double epsilon = Math.Pow(10, -7);

            var x = SymbolicExpression.Variable("x");

            SymbolicExpression func = -x.Pow(3) -x.Pow(2) - x -1;
            Console.WriteLine(func.ToString());

            var result = Chord.Search(func, a,b,epsilon,x);
            var val = func.Evaluate(new Dictionary<string, FloatingPoint>() { { "x",result} }).RealValue;


            Console.WriteLine($"Минимум на отрезке [{a};{b}]: \nx* = {result}, f(x*) = {val}");
        }
    }
}
