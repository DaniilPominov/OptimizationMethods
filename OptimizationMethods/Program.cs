using MathNet.Numerics.LinearAlgebra;
using MathNet.Symbolics;
using OptimizationMethods.Approximations;
using OptimizationMethods.GradientDescent;
using OptimizationMethods.SecondOrderMethods;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -6);
            double tolerance = 10000;

            var x = Expr.Variable("x");
            var y = Expr.Variable("y");
            var z = Expr.Variable("z");
            var x1 = -1.0;
            var x2 = 1.0;
            var x3 = -1.0;
            //var x1 = -0.5;
            //var x2 = -0.5;
            //var x3 = 0.0;
            var testPoints = new[] { 
                Vector<double>.Build.Dense([x1, x2, x3]),
            };
            //Expr func = x.Pow(5) -5*x + +10*(-x.Pow(2)+y).Pow(2)+z.Pow(3)-1-6*z;
            Expr func = 2*x.Pow(4)*y.Pow(4)+x.Pow(2)*y.Pow(2)+z.Pow(4)+x.Pow(2)*z.Pow(2)+x+y;
            // for initial (-1/2,-1/2,0) Critical point found at (x, y, z) = (-0.658417, -0.658417, 0.000000)
            Console.WriteLine(func);
            foreach (var point in testPoints)
            {
                Console.WriteLine(point);
                var result = Markvardt.Search(func, x, y, z, point, epsilon, tolerance, 10000);
                Console.WriteLine(result);
            }
            
            
        }
    }
}
