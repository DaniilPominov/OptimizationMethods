using OptimizationMethods.Common;
namespace OptimizationMethods.FirstOrderMethods
{
    internal class Chord
    {
        internal static double Search(Func<double,double> function,double a0, double b0, double epsilon)
        {
            int k = 0;
            Dictionary<int,double> x = new(), a = new(), b = new();
            a[k] = a0;
            b[k] = b0;
            goto second;
            second:{
                var f_b = Operations.Derivate1D(function,b[k]);
                var f_a = Operations.Derivate1D(function,a[k]);
                x[k+1] = a[k] - f_a*(b[k] - a[k])/(f_b-f_a);
                goto thirth;
            }
            thirth:{
                if (Math.Abs(Operations.Derivate1D(function,x[k+1]))<= epsilon){
                    Console.WriteLine(k);
                    return x[k+1];
                }
                else{
                    goto fourth;
                }
            }
            fourth:{
                if(Operations.Derivate1D(function,x[k+1])>0){
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