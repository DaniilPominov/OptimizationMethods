using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace OptimizationMethods.SecondOrderMethods;

public class Markvardt : IMethod
{
    static bool IsPositiveDefinite(Matrix<double> matrix)
    {
        // ���������, ��� ������� �����������
        if (!matrix.IsSymmetric())
        {
            return false;
        }

        // ��������� ����������� ��������
        var eigenValues = matrix.Evd().EigenValues;

        // ���������, ��� ��� ����������� �������� ������������
        foreach (var eigenValue in eigenValues)
        {
            if (eigenValue.Real <= 0)
            {
                return false;
            }
        }

        return true;
    }
    public static Vector<double> Search(Expr f, List<Expr> vars, Vector<double> initialGuess, double epsilon, double InitTolerance, int maxIterations)
    {
        //�������� �������
        Expr[] gradient = new Expr[vars.Count];
        for (int i = 0; i < vars.Count; i++)
        {
            gradient[i]= f.Differentiate(vars[i]);
        }

        var tolerance = InitTolerance;
        //������� �������
        Expr[,] hessian = new Expr[vars.Count, vars.Count];
        for(int i =0;i<vars.Count;i++)
        {
            for (int j = 0; j < vars.Count; j++)
            {
                hessian[i,j] = f.Differentiate(vars[i]).Differentiate(vars[j]);
            }
        }
        var currentPoint = initialGuess;
        var grad = EvaluateGradient(gradient, currentPoint,vars);
        var hess = EvaluateHessian(hessian, currentPoint,vars);

        int k = 0;
        checkEnd:
        {
            grad = EvaluateGradient(gradient, currentPoint,vars);
            if (grad.L2Norm() <= epsilon || k>maxIterations) {
#if DEBUG
                Console.WriteLine($"�������� ����������� ����� {k} ��������");
#endif          
                Console.WriteLine("hessian");
                Console.WriteLine(hess);
                Console.WriteLine("Positive?");
                Console.WriteLine(IsPositiveDefinite(hess));
                return currentPoint;
            }
            //goto evalute;
            hess = EvaluateHessian(hessian, currentPoint,vars);
        }
        evalute:
        {
            //var hess  = EvaluateHessian(hessian,currentPoint);
            
            //��������� �������
            var identityMatrix = Matrix<double>.Build.DenseIdentity(vars.Count);
            var subMatrix = (hess + tolerance * identityMatrix).Inverse();

            var nextPoint = currentPoint - subMatrix*grad;
            if (f.Evaluate(Common.BuildPointDict(nextPoint, vars)).RealValue <
                f.Evaluate(Common.BuildPointDict(currentPoint, vars)).RealValue)
            {
                var fValue = f.Evaluate(Common.BuildPointDict(nextPoint,vars));
                currentPoint = nextPoint;
                k += 1;
                tolerance /= 2;
                    goto checkEnd;
            }
            else
            {
                tolerance *= 2;
                if(tolerance>= 100000000)
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
    public static Vector<double> EvaluateGradient(Expr[] gradient, Vector<double> point, List<Expr> vars)
    {
        var result = Vector<double>.Build.Dense(vars.Count);
        //var symbols = new Dictionary<string, FloatingPoint>
        //{
        //    { "x", point[0] },
        //    { "y", point[1] },
        //    { "z", point[2] }
        //};
        var symbols = Common.BuildPointDict(point, vars);

        for (int i = 0; i < vars.Count; i++)
            result[i] = gradient[i].Evaluate(symbols).RealValue;

        return result;
    }

    public static Matrix<double> EvaluateHessian(Expr[,] hessian, Vector<double> point, List<Expr> vars)
    {
        var result = Matrix<double>.Build.Dense(vars.Count, vars.Count);
        var symbols = Common.BuildPointDict(point, vars);

        for (int i = 0; i < vars.Count; i++)
            for (int j = 0; j < vars.Count; j++)
                result[i, j] = hessian[i, j].Evaluate(symbols).RealValue;

        return result;
    }
    public static double MarquardtLineSearch(
            Func<Vector<double>, double> function,
            Func<Vector<double>, Vector<double>> gradient,
            Func<Vector<double>, Matrix<double>> hessian,
            Vector<double> point,
            Vector<double> direction,
            double initialStep = 1.0,
            double lambda = 1.0,
            double tolerance = 1e-6,
            int maxIterations = 100)
        {
            double step = initialStep;
            double previousStepValue = function(point + step * direction);

            for (int i = 0; i < maxIterations; i++)
             {
                Vector<double> newPoint = point + step * direction;
                
                Vector<double> grad = gradient(newPoint);
                Matrix<double> hess = hessian(newPoint);
                
                double firstDerivative = grad.DotProduct(direction);
                double secondDerivative = direction.DotProduct(hess * direction);

                double adjustedSecondDerivative = secondDerivative + lambda;
                
                if (Math.Abs(adjustedSecondDerivative) < double.Epsilon)
                    adjustedSecondDerivative = Math.Sign(adjustedSecondDerivative) * tolerance;

                double delta = -firstDerivative / adjustedSecondDerivative;
                step += delta;

                double currentStepValue = function(point + step * direction);
                
                if (Math.Abs(delta) < tolerance || Math.Abs(currentStepValue - previousStepValue) < tolerance)
                    break;
                previousStepValue = currentStepValue;
                
                lambda = adjustedSecondDerivative <= 0 ? lambda * 10 : lambda / 10;
            }

            return step;
        }

}
