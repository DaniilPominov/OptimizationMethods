using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods.RandomSearch
{
    public class RandomWithReturn
    {

        public static Vector<double> Search(Expr f, List<Expr> vars, Vector<double> initialGuess,
            double initialStep, int maxIterations, double beta, double minStepSize,
            int maxFailedAttempts) 
        {
            int currentIteration = 0;
            double step = initialStep;
            var currentPoint = initialGuess.Clone();
            int currentAttempt = 1;
            while (currentIteration++ < maxIterations)
            {
                Vector<double> direction = GenerateRandomDirection(vars.Count);
                Vector<double> candidatePoint = currentPoint + step * direction;


                double currentValue = f.Evaluate(Common.BuildPointDict(currentPoint,vars)).RealValue;
                double candidateValue = f.Evaluate(Common.BuildPointDict(candidatePoint, vars)).RealValue;

                if (candidateValue < currentValue)
                {
                    // Успешный шаг
                    currentPoint = candidatePoint;
                    currentIteration++;
                    currentAttempt = 1;
                }
                else
                {
                    // Неудачный шаг
                    currentAttempt++;
                    if (currentAttempt > maxFailedAttempts)
                    {
                        step *= beta;
                        currentAttempt = 1;

                        if (step <= minStepSize)
                            break;
                    }
                }
            }

            return currentPoint;
        }

        // Генерация случайного направления
        private static Vector<double> GenerateRandomDirection(int dimensions)
        {
            double[] components = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                var random = new Random(DateTime.Now.Microsecond);
                components[i] = random.NextDouble() * 2 - 1;
            }
            Vector<double> vector = Vector<double>.Build.DenseOfArray(components);
            return vector.Normalize(2);
        }

    }
}
