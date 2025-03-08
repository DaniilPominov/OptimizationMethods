using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;
using MathNet.Numerics.Optimization;

namespace OptimizationMethods.SecondOrderMethods;

public class Markvardt : IMethod
{
    public static Vector<double> Search(Expr f, Expr x, Expr y, Expr z, Vector<double> initialGuess, double epsilon, double InitTolerance, int maxIterations)
    {
        //градиент функции
        var gradient = new[] { f.Differentiate(x), f.Differentiate(y), f.Differentiate(z) };
        var tolerance = InitTolerance;
        //гессиан функции
        var hessian = new[,]
        {
            { f.Differentiate(x).Differentiate(x), f.Differentiate(x).Differentiate(y), f.Differentiate(x).Differentiate(z) },
            { f.Differentiate(y).Differentiate(x), f.Differentiate(y).Differentiate(y), f.Differentiate(y).Differentiate(z) },
            { f.Differentiate(z).Differentiate(x), f.Differentiate(z).Differentiate(y), f.Differentiate(z).Differentiate(z) }
        };
        var currentPoint = initialGuess;
        var grad = EvaluateGradient(gradient, currentPoint);
        var hess = EvaluateHessian(hessian, currentPoint);
        int k = 0;
        checkEnd:
        {
            grad = EvaluateGradient(gradient, currentPoint);
            if (grad.L2Norm() <= epsilon || k>maxIterations) {
#if DEBUG
                Console.WriteLine($"Алгоритм остановился после {k} итераций");
#endif
                return currentPoint;
            }
            //goto evalute;
            hess = EvaluateHessian(hessian, currentPoint);
        }
        evalute:
        {
            //var hess  = EvaluateHessian(hessian,currentPoint);
            
            //единичная матрица
            var identityMatrix = Matrix<double>.Build.DenseIdentity(3);

            var nextPoint = currentPoint - (hess + tolerance * identityMatrix).Inverse()*grad;

            if (f.Evaluate(new Dictionary<string, FloatingPoint> { { "x", nextPoint[0] }, { "y", nextPoint[1] }, { "z", nextPoint[2] } }).RealValue <
                f.Evaluate(new Dictionary<string, FloatingPoint> { { "x", currentPoint[0] }, { "y", currentPoint[1] }, { "z", currentPoint[2] } }).RealValue)
            {
                currentPoint = nextPoint;
                k += 1;
                tolerance /= 2;
                if (tolerance <= 1.0 / 10000000.0)
                {
                    Console.WriteLine($"tolerance become too small,{k}");
                    return currentPoint;
                }
                    goto checkEnd;
            }
            else
            {
                tolerance *= 2;
                if(tolerance>= 10000000)
                {
                    Console.WriteLine($"tolerance become too big,{k}");
                    return currentPoint;
                }
                goto evalute;
            }
        }
    }
    public static double Search(SymbolicExpression function,double x0, double epsilon, SymbolicExpression X)
    {
        var GetPointX = Common.GetPointX;
        Dictionary<int, double> x = new();
        int k = 0;
        double mu = 10;
        var derivate = function.Differentiate(X);
        var derivate2 = derivate.Differentiate(X);
        Console.WriteLine("second derivate at x0");
        Console.WriteLine(derivate2.Evaluate(GetPointX(x0)).RealValue);

        if(derivate2.Evaluate(GetPointX(x0)).RealValue>=mu){
            mu = Math.Abs(derivate2.Evaluate(GetPointX(x0)).RealValue)*10;
        }
        goto first;
        first:{
            k = 1;
            x[k] = x0 - derivate.Evaluate(GetPointX(x0)).RealValue/derivate2.Evaluate(GetPointX(x0)).RealValue;
            goto second;
        }
        second:
        {
            x[k+1] = x[k] - derivate.Evaluate(GetPointX(x[k])).RealValue/(derivate2.Evaluate(GetPointX(x[k])).RealValue+mu);
            if(function.Evaluate(GetPointX(x[k+1])).RealValue < function.Evaluate(GetPointX(x[k])).RealValue){
                mu = mu/2;
            }
            else{
                mu = 2*mu;
            }
            goto thirth;

        }
        thirth:{
            //if(Math.Abs(derivate.Evaluate(GetPointX(x[k+1])).RealValue) <= epsilon){
            if(Math.Abs(x[k+1]-x[k])<=epsilon){
                Console.WriteLine($"steps left: {k}");
                return x[k+1];
            }
            else{
                if(k>10000){
                    Console.WriteLine("Max steps reached");
                    return x[k+1];
                }
                k++;
                goto second;
            }
        }
    }
    static Vector<double> EvaluateGradient(Expr[] gradient, Vector<double> point)
    {
        var result = Vector<double>.Build.Dense(3);
        var symbols = new Dictionary<string, FloatingPoint>
        {
            { "x", point[0] },
            { "y", point[1] },
            { "z", point[2] }
        };

        for (int i = 0; i < 3; i++)
            result[i] = gradient[i].Evaluate(symbols).RealValue;

        return result;
    }

    static Matrix<double> EvaluateHessian(Expr[,] hessian, Vector<double> point)
    {
        var result = Matrix<double>.Build.Dense(3, 3);
        var symbols = new Dictionary<string, FloatingPoint>
        {
            { "x", point[0] },
            { "y", point[1] },
            { "z", point[2] }
        };

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                result[i, j] = hessian[i, j].Evaluate(symbols).RealValue;

        return result;
    }

}
