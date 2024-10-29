using OptimizationMethods.Common;
namespace OptimizationMethods.FirstOrderMethods
{
    internal class Secant
    {
        internal static double Search(Func<double,double> function,double a, double b, double epsilon)
        {
            int k;
            Dictionary<int,double> x = new(), f = new();
            goto first;
            first:{
                k = 1;
                var f_3 = 0;
                x[0] = 0;
                x[1] = 0;
                goto second;

            }
            second:{
                var f_k_1 = Operations.Derivate1D(function,x[k-1]);
                var f_k = Operations.Derivate1D(function,x[k]);
                x[k+1] = x[k] - f_k_1*(x[k]-x[k-1])/(f_k-f_k_1);
            }
            return 0.0;
        }
    }
}