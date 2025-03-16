using MathNet.Numerics.LinearAlgebra;
using OptimizationMethods.DirectedMethods;
using OptimizationMethods.SecondOrderMethods;
using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -5);
            double tolerance = 20;

            var x = Expr.Variable("x");
            var y = Expr.Variable("y");
            var z = Expr.Variable("z");
            var x1 = -0.5;
            var x2 = -0.5;
            var x3 = 1;
            //var x3 = 0.0;
            var testPoints = new[] { 
                Vector<double>.Build.Dense([x1, x2,x3]),
            };
            var vars = new List<Expr> { x, y,z};
            Expr func = x.Pow(5) -5*x + +10*(-x.Pow(2)+y).Pow(2)+z.Pow(3)-1-6*z;
            
            //Expr func = 2*x.Pow(4)*y.Pow(4)+x.Pow(2)*y.Pow(2)+z.Pow(4)+x.Pow(2)*z.Pow(2)+x+y;
            //for initial (-1/2,-1/2,0) Critical point found at (x, y, z) = (-0.658417, -0.658417, 0.000000)
            //Expr func = x.Pow(2) + 5*(30*y-x).Pow(4);
            //Expr func = 2*x.Pow(2)+x*y+y.Pow(2);
            //Example from Panteleev's book, works with (0.5,1), E = 0.1, tol = 20;
            
            //Expr func = 4 * (x - 5).Pow(2) + (y - 6).Pow(2);
            Console.WriteLine(func);
            foreach (var point in testPoints)
            {
                Console.WriteLine(point);
                //var result = Markvardt.Search(func,vars,point,epsilon,20000,10000);
                var result = Configuration.Search(func, vars, point,epsilon,1,2, new List<double> { 1,1,1});
                Console.WriteLine($"point={result}, f(M)={func.Evaluate(Common.BuildPointDict(result,vars)).RealValue}");
            }
            
        }
    }
}
