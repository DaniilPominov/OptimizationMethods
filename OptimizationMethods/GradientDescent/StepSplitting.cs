using MathNet.Symbolics;
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
    }
}
