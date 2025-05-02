using MathNet.Numerics.LinearAlgebra;
using OptimizationMethods.Conjugate;
using OptimizationMethods.GradientDescent;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var vars = new List<Expr> { Expr.Variable("x"), Expr.Variable("y") };
            Expr objective = Expr.Parse("x+y");

            // Ограничения: x + y = 1, y >= 0
            //var equality = new List<Expr> { Expr.Parse("x + y - 1") };
            //var inequality = new List<Expr> { Expr.Parse("-y") };
            var inequality = new List<Expr> { 
                Expr.Parse("x^2+2*x+y^2-2*y-14"),
                Expr.Parse("2*x+y"),
                Expr.Parse("1-x-2*y")};
            var result = StepSplitting.Search(
                f: objective,
                vars: vars,
                initialGuess: Vector<double>.Build.Dense(new[] { 2.0, 3.0 }),
                epsilon: 1e-6,
                initStep: 0.1,
                d: 0.5,
                //equalityConstraints: equality,
                inequalityConstraints: inequality,
                penaltyCoefficient: 10.0
            );
            Console.WriteLine(result);

        }
    }
}
