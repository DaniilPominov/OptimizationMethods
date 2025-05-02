using Expr = MathNet.Symbolics.SymbolicExpression;
using MathNet.Numerics.LinearAlgebra;
using OptimizationMethods.SecondOrderMethods;
namespace OptimizationMethods.GradientDescent
{
    internal class StepSplitting
    {
        internal static Vector<double> Search(Expr f, List<Expr> vars, Vector<double> initialGuess,
         double epsiolon,double initStep, double? d,Vector<double> otherMethodDirection=null)
        {
            int maxIterations = 100000;
            var stepSize = initStep;
            var delta = d ?? 0.5;
#if DEBUG
            int iterCount = 0;
#endif
            //вспомогательная структара
            Expr[] gradient = new Expr[vars.Count];
                for (int i = 0; i < vars.Count; i++)
                {
                    gradient[i]= f.Differentiate(vars[i]);
                }
            var initialPoint = initialGuess.Clone();
            var currentPoint = initialPoint.Clone();
            for (int i = 0; i < maxIterations; i++)
            {
                // Вычисляем градиент в текущей точке
                
                var grad = Markvardt.EvaluateGradient(gradient,currentPoint,vars);
                if(otherMethodDirection!=null)
                grad = otherMethodDirection;

                // Обновляем точку
                var nextPoint = currentPoint - stepSize * grad;

                // Проверяем условие сходимости
                //L2Norm есть длина вектора
                if ((grad).L2Norm() < epsiolon)
                {
#if DEBUG
                    iterCount = i;
#endif
                    break;
                }

                // Дробление шага, если функция не уменьшается
                while (f.Evaluate(Common.BuildPointDict(nextPoint,vars)).RealValue >
                       f.Evaluate(Common.BuildPointDict(currentPoint,vars)).RealValue)
                {

                    stepSize *= delta;
                    
                    nextPoint = currentPoint - stepSize * grad;

                    if(stepSize <= epsiolon)
                    {
                        return nextPoint;
                    }
                }
                stepSize = initStep;
                

                currentPoint = nextPoint;
            }
#if DEBUG
            Console.WriteLine($"Конечный размер шага= {stepSize} при начальном= {initStep}, прошло итерация= {iterCount}");
#endif
            return currentPoint;

        }

        private static Expr BuildPenalizedFunction(Expr f, List<Expr> equalityConstraints, List<Expr> inequalityConstraints, double penaltyCoefficient, double epsilon = 1e-8)
        {
            Expr penalized = f;

            if (equalityConstraints != null)
            {
                foreach (var h in equalityConstraints)
                {
                    //penalized += penaltyCoefficient * h.Pow(2);
                    penalized += penaltyCoefficient * h.Pow(2);
                }
            }

            if (inequalityConstraints != null)
            {
                foreach (var g in inequalityConstraints)
                {
                    var smoothMax = (g + (g.Pow(2) + epsilon).Sqrt()) / 2;
                    penalized += penaltyCoefficient * smoothMax.Pow(2);
                }
            }

            return penalized;
        }

        internal static Vector<double> Search(
            Expr f,
            List<Expr> vars,
            Vector<double> initialGuess,
            double epsilon,
            double initStep,
            double? d,
            Vector<double> otherMethodDirection = null,
            List<Expr> equalityConstraints = null,
            List<Expr> inequalityConstraints = null,
            double penaltyCoefficient = 1.0)
        {
            int maxIterations = 100000;
            var stepSize = initStep;
            var delta = d ?? 0.5;
#if DEBUG
            int iterCount = 0;
#endif
            Expr functionToUse = f;
            if ((equalityConstraints?.Count ?? 0) > 0 || (inequalityConstraints?.Count ?? 0) > 0)
            {
                functionToUse = BuildPenalizedFunction(f, equalityConstraints, inequalityConstraints, penaltyCoefficient);
            }

            Expr[] gradient = new Expr[vars.Count];
            for (int i = 0; i < vars.Count; i++)
            {
                gradient[i] = functionToUse.Differentiate(vars[i]);
            }

            var currentPoint = initialGuess.Clone();

            for (int i = 0; i < maxIterations; i++)
            {
                var grad = Markvardt.EvaluateGradient(gradient, currentPoint, vars);
                if (otherMethodDirection != null)
                    grad = otherMethodDirection;

                var nextPoint = currentPoint - stepSize * grad;

                if (grad.L2Norm() < epsilon)
                {
#if DEBUG
                    iterCount = i;
#endif
                    break;
                }

                double currentValue = functionToUse.Evaluate(Common.BuildPointDict(currentPoint, vars)).RealValue;
                double nextValue = functionToUse.Evaluate(Common.BuildPointDict(nextPoint, vars)).RealValue;

                while (nextValue > currentValue)
                {
                    stepSize *= delta;
                    nextPoint = currentPoint - stepSize * grad;

                    if (stepSize <= epsilon)
                    {
                        return nextPoint;
                    }

                    nextValue = functionToUse.Evaluate(Common.BuildPointDict(nextPoint, vars)).RealValue;
                }

                stepSize = initStep;
                currentPoint = nextPoint;
            }

#if DEBUG
            Console.WriteLine($"Конечный размер шага= {stepSize} при начальном= {initStep}, прошло итераций= {iterCount}");
#endif
            return currentPoint;
        }
    }
}
