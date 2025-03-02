using MathNet.Symbolics;
using OptimizationMethods.Approximations;
using OptimizationMethods.GradientDescent;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -6);

            var x = Expr.Variable("x");
            var y = Expr.Variable("y");
            var z = Expr.Variable("z");
            //var x1 = -1.0;
            //var x2 = 10.0;
            //var x3 = 1.0;
            var x1 = 1.0;
            var x2 = 2.0;
            var x3 = 0.0;
            Expr func = x.Pow(5) -5*x + +10*(-x.Pow(2)+y).Pow(2)+z.Pow(3)-1-6*z;
            var result = StepSplitting.Search(func, x, y, z, (x1, x2, x3), epsilon, 0.1,null);
            Console.WriteLine(result);
        }
    }
}
