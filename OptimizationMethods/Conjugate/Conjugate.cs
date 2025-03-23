using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;
using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;
using OptimizationMethods.SecondOrderMethods;
using OptimizationMethods.GradientDescent;
namespace OptimizationMethods.Conjugate{

//Метод Флетчера-Ривса
public class Conjugate
{
        public static Vector<double> Search(Expr f, List<Expr> vars,
            Vector<double> initialPoint, double epsilon, int M,
            double secondEpsilon = 1.0 / 100.0, double stepSplitInit = 0.1, 
            double stepSplitCoeff = 0.4
        )
    {
        int k = 0;
        Expr[] gradient = new Expr[vars.Count];
        Vector<double> grad = null;
        Vector<double> p = null;
        double beta = 1;

        var currentPoint = initialPoint.Clone();
        var oldPoint = initialPoint.Clone();

        for (int i = 0; i < vars.Count; i++)
        {
            gradient[i]= f.Differentiate(vars[i]);
        }
        second:{
            grad = Markvardt.EvaluateGradient(gradient,currentPoint,vars);
                bool fDifference = false;
                if (k > 1)
                {
                    fDifference = f.Evaluate(Common.BuildPointDict(currentPoint, vars)).RealValue -
                    f.Evaluate(Common.BuildPointDict(oldPoint, vars)).RealValue < epsilon;
                }
            if(grad.L2Norm()<=epsilon || k>M || fDifference)
                {
                        return currentPoint;
            }

        }
        thirth:{

                if (k == 0 || p == null)
                    p = Vector<double>.Build.Dense((-grad).ToArray());
                else
                {
                    var oldGrad = Markvardt.EvaluateGradient(gradient, oldPoint, vars);
                    var gradVal = grad.L2Norm();
                    var oldGradVal = oldGrad.L2Norm();
                    beta = (grad.L2Norm()* grad.L2Norm()) / (oldGrad.L2Norm()* oldGrad.L2Norm());
                    p = -grad + p * beta;
                }
            var Lsub = StepSplitting.Search(f,vars,currentPoint,
                secondEpsilon, stepSplitInit, stepSplitCoeff,
                -p);
                var L = (Lsub - currentPoint) *(p) /(p.L2Norm()* p.L2Norm());
            var buff = currentPoint.Clone();
            currentPoint = currentPoint + L*p;
            oldPoint = buff.Clone();
            k++;
            goto second;
        }
        
    }
}
}