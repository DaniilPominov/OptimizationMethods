using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System.Reflection.Metadata.Ecma335;

namespace OptimizationMethods.DirectedMethods
{
    internal class Configuration
    {
        public static Vector<double> Search(Expr f, List<Expr> vars, Vector<double> initialPoint, double epsilon, double labmda, double alpha, List<double>? deltas)
        {
            int i = 0;
            int k = 0;
            var currentPoint = initialPoint;
            var newPoint = Vector<double>.Build.Dense(currentPoint.ToArray());
            var testPoint = Vector<double>.Build.Dense(currentPoint.ToArray());
            List<double> delta = new List<double>();
            if (deltas != null)
                delta = deltas;
            else {
                for (int j = 0; j < vars.Count; j++)
                {
                    delta.Add(10*epsilon);
                }
            }
            second:
            {
                testPoint = Vector<double>.Build.Dense(newPoint.ToArray());
                
                testPoint[i] = newPoint[i] + delta[i];
                if (f.Evaluate(Common.BuildPointDict(testPoint, vars)).RealValue <
                    f.Evaluate(Common.BuildPointDict(newPoint, vars)).RealValue)
                    goto third;
                testPoint = Vector<double>.Build.Dense(newPoint.ToArray());
                testPoint[i] = newPoint[i] - delta[i];
                if (f.Evaluate(Common.BuildPointDict(testPoint, vars)).RealValue <
                    f.Evaluate(Common.BuildPointDict(newPoint, vars)).RealValue)
                    goto third;
                testPoint = Vector<double>.Build.Dense(newPoint.ToArray());
            }
            third:
            {
                if (i < vars.Count-1)
                {
                    i++;
                    newPoint = Vector<double>.Build.Dense(testPoint.ToArray()); ;
                    goto second;
                }
                else
                {
                    i = 0;
                    if (f.Evaluate(Common.BuildPointDict(testPoint, vars)).RealValue <
                        f.Evaluate(Common.BuildPointDict(currentPoint, vars)).RealValue)
                        goto fourth;
                    goto fifth;
                }
            }
            fourth:
            {
                newPoint = Vector<double>.Build.Dense(testPoint.ToArray());
                var buf = Vector<double>.Build.Dense(newPoint.ToArray());
                newPoint = newPoint + labmda * (newPoint - currentPoint);
                currentPoint = buf;
                k++;
                goto second;
            }
            fifth:
            {
                bool flag = false;
                for(int j = 0; j < delta.Count; j++)
                {
                    if (delta[j] > epsilon)
                    {
                        flag = true;
                        delta[j] /= alpha;
                    }
                }
                if (flag)
                {
                    newPoint = Vector<double>.Build.Dense(currentPoint.ToArray());
                    k++;
                    goto second;
                }
                Console.WriteLine(k);
                return currentPoint;
            }
        }
    }
}
