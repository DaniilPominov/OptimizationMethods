using MathNet.Symbolics;
namespace OptimizationMethods.FirstOrderMethods
{
    internal class Chord
    {
        internal static double Search(SymbolicExpression function,double a0, double b0, double epsilon, SymbolicExpression X, bool? alt = false)
        {
            int k = 0;
            Dictionary<int,double> x = new(), a = new(), b = new();
            a[k] = a0;
            b[k] = b0;
            var derivate = function.Differentiate(X);
            goto second;
            second:{
                var f_b = derivate.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", b[k] } }).RealValue;
                var f_a = derivate.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", a[k] } }).RealValue;
                x[k+1] = a[k] - f_a*(b[k] - a[k])/(f_b-f_a);
                goto thirth;
            }
            thirth:{
                if (k>2&&Math.Abs(derivate.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", x[k+1] } }).RealValue)<= epsilon){
                    Console.WriteLine(k);
                    return x[k+1];
                }
                else{
                    goto fourth;
                }
            }
            fourth:{
                if(derivate.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", x[k + 1] } }).RealValue > 0){
                    a[k+1] = a[k];
                    b[k+1] = x[k+1];
                }
                else{
                    a[k+1] = x[k+1];
                    b[k+1] = b[k];
                }
                k+=1;
                goto second;
            }


        }
    }
}