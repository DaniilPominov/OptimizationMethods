using MathNet.Symbolics;
using OptimizationMethods.FirstOrderMethods;
using OptimizationMethods.SecondOrderMethods;
using OptimizationMethods.Approximations;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -6);

            var x = SymbolicExpression.Variable("x");
            var x1 = -4;
            var x2 = -2;

            SymbolicExpression func = x.Pow(3) -x + (-x).Exp();
            //SymbolicExpression func = -x.Pow(2);
            Console.WriteLine(func.ToString());

            var result = Cubic.Search(func, x1, x2,epsilon,x);

            var val = func.Evaluate(new Dictionary<string, FloatingPoint>() { { "x",result} }).RealValue;


            Console.WriteLine($"Минимум при начальных х1={x1}, x2={x2} \nx* = {result}, f(x*) = {val}");
        }
    }
}
