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
            //if (functionToUse.Evaluate(Common.BuildPointDict(currentPoint, vars)).RealValue > epsilon)
            //{
            //    penaltyCoefficient *= penaltyIncrease;
            //    if ((equalityConstraints?.Count ?? 0) > 0 || (inequalityConstraints?.Count ?? 0) > 0)
            //    {
            //        functionToUse = BuildPenalizedFunction(f, equalityConstraints, inequalityConstraints, penaltyCoefficient);
            //    }
            //    goto start;
            //}
            var epsilon = 1e-6;
            var vars = new List<Expr> { Expr.Variable("x"), Expr.Variable("y") };
            Expr func = Expr.Parse("x+y");
            
            var inequality = new List<Expr> { 
                Expr.Parse("x^2+2*x+y^2-2*y-14"),
                Expr.Parse("2*x+y"),
                Expr.Parse("1-x-2*y")};
            var penaltyCoefficient= 4.5;
            var penaltyIncrease = 1.5;
            var penalty = StepSplitting.BuildPenalizedFunction(func, null, inequality, penaltyCoefficient, epsilon);
            
            func = penalty.Item1;
            var P = penalty.Item2;
            
            var result = StepSplitting.Search(
                f: func,
                epsiolon: epsilon,
                vars: vars,
                initialGuess: Vector<double>.Build.Dense(new[] { 0.0, 3.0 }),
                initStep: 0.5,
                d: 0.5
            );

            var PValue = P.Evaluate(Common.BuildPointDict(result, vars)).RealValue;
            var k = 0;
            while(PValue > epsilon && k++ < 500)
            {
                penaltyCoefficient *= penaltyIncrease;
                penalty = StepSplitting.BuildPenalizedFunction(func, null, inequality, penaltyCoefficient, epsilon);
                func = penalty.Item1;
                P = penalty.Item2;
                PValue = P.Evaluate(Common.BuildPointDict(result, vars)).RealValue;

                result = StepSplitting.Search(
                f: func,
                epsiolon: epsilon,
                vars: vars,
                initialGuess: result,
                initStep: 0.5,
                d: 0.5
            );
            }

            Console.WriteLine(result);

        }
    }
}
