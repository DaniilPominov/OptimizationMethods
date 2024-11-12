using MathNet.Symbolics;
using OptimizationMethods.FirstOrderMethods;
using OptimizationMethods.SecondOrderMethods;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -7);

            var x = SymbolicExpression.Variable("x");
            var start = -4;

            SymbolicExpression func = x.Pow(3) -x + (-x).Exp();
            Console.WriteLine(func.ToString());

            var result = Markvardt.Search(func, start, epsilon, x);

            var val = func.Evaluate(new Dictionary<string, FloatingPoint>() { { "x",result} }).RealValue;


            Console.WriteLine($"Минимум при начальном х={start} \nx* = {result}, f(x*) = {val}");
        }
    }
}
