using System.Security.Cryptography.X509Certificates;
using OptimizationMethods.ByBit;
using OptimizationMethods.ExclusionMethods;
using MathNet.Symbolics;
using OptimizationMethods.FirstOrderMethods;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a = -1;
            double b = 2;
            double epsilon = Math.Pow(10, -7);

            var x = SymbolicExpression.Variable("x");

            SymbolicExpression func = x.Pow(4) + x.Pow(2) + x + 1;

            var result = Chord.Search(func, a,b,epsilon,x);
            var val = func.Evaluate(new Dictionary<string, FloatingPoint>() { { "x",result} }).RealValue;


            Console.WriteLine($"Минимум на отрезке [{a};{b}]: \nx* = {result}, f(x*) = {val}");
        }
    }
}
