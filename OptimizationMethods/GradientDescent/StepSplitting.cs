using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;
using MathNet.Numerics.LinearAlgebra;

namespace OptimizationMethods.GradientDescent
{
    internal class StepSplitting
    {
        internal static Vector<double> Search(Expr f, Expr x, Expr y, Expr z, (double,double,double) start, double epsiolon,double initStep, double? d)
        {
            int maxIterations = 100000;
            var stepSize = initStep;
            var delta = d ?? 0.5;
#if DEBUG
            int iterCount = 0;
#endif
            //вспомогательная структара
            var gradient = new[] { f.Differentiate(x), f.Differentiate(y), f.Differentiate(z) };
            var initialPoint = Vector<double>.Build.Dense(new[] { start.Item1, start.Item2, start.Item3 });
            var currentPoint = initialPoint;
            for (int i = 0; i < maxIterations; i++)
            {
                // Вычисляем градиент в текущей точке
                var grad = Vector<double>.Build.Dense(new[]
                {
                gradient[0].Evaluate(new Dictionary<string, FloatingPoint> { { "x", currentPoint[0] }, { "y", currentPoint[1] }, { "z", currentPoint[2] } }).RealValue,
                gradient[1].Evaluate(new Dictionary<string, FloatingPoint> { { "x", currentPoint[0] }, { "y", currentPoint[1] }, { "z", currentPoint[2] } }).RealValue,
                gradient[2].Evaluate(new Dictionary<string, FloatingPoint> { { "x", currentPoint[0] }, { "y", currentPoint[1] }, { "z", currentPoint[2] } }).RealValue
            });

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
                while (f.Evaluate(new Dictionary<string, FloatingPoint> { { "x", nextPoint[0] }, { "y", nextPoint[1] }, { "z", nextPoint[2] } }).RealValue >
                       f.Evaluate(new Dictionary<string, FloatingPoint> { { "x", currentPoint[0] }, { "y", currentPoint[1] }, { "z", currentPoint[2] } }).RealValue)
                {

                    stepSize *= delta;
                    if(stepSize <= epsiolon)
                    {
                        throw new Exception();
                    }
                    nextPoint = currentPoint - stepSize * grad;
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
