using MathNet.Numerics.LinearAlgebra;
using OptimizationMethods.Conjugate;
using OptimizationMethods.GradientDescent;
using OptimizationMethods.RandomSearch;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double epsilon = Math.Pow(10, -8);

            var x = Expr.Variable("x");
            var y = Expr.Variable("y");
            var z = Expr.Variable("z");

            var x1 = 1.0;
            var x2 = 2.0;
            //var x3 = 3.0;
            var initial = Vector<double>.Build.Dense([x1,
                x2, 
                //x3
                ]);

            var vars = new List<Expr> { x, 
                y,
                //z
            };
            Expr func = x.Pow(2) + y.Pow(2);
            Console.WriteLine(func);
            var result = RandomWithReturn.Search(func, vars, initial, 0.1, 1000, 0.5, 1e-5, 300);
            Console.WriteLine(result);

        }
    }
}
