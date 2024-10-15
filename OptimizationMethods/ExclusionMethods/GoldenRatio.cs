using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethods.ExclusionMethods
{
    internal class GoldenRatio
    {
        internal static double Search(Func<double, double> function, double a_0, double b_0, double epsilon)
        {
            int k;
            Dictionary<int,double> y = new(), z = new(), a = new(), b = new(), x = new();
            a[0] = a_0;
            b[0] = b_0;
            goto first;
            first:
            {
                goto second;
            }
            second:
            {
                k = 0;
                goto thirth;
            }
            thirth:
            {
                y[0] = (a[0] + (3-Math.Sqrt(5))/2 * (b[0] - a[0]));
                z[0] = (a[0] + b[0] - y[0]);
                goto fourth;
            }
            fourth:
            {
                goto fifth;
            }
            fifth:
            {
                if (function(y[k]) <= function(z[k]))
                {
                    a[k+1] = a[k];
                    b[k + 1] = z[k];
                    y[k+1] = (a[k + 1] + b[k + 1] - y[k]);
                    z[k+1]= y[k];
                }
                else
                {
                    a[k + 1] = y[k];
                    b[k + 1] = b[k];
                    y[k+1] = z[k];
                    z[k + 1] = a[k + 1] + b[k + 1] - z[k];
                }
                goto sixth;
            }
            sixth:
            {
                if (Math.Abs(a[k + 1] - b[k+1]) < epsilon)
                {
                    Console.WriteLine($"Теоретическая оценка n>={Math.Log((b[0] - a[0])/epsilon)/Math.Log(1.618034)}");
                    Console.WriteLine($"Потребовалось {k+1} шагов");
                    Console.WriteLine($"характеристика относительного уменьшения: R(N) = {Math.Pow(0.618,k)/Math.Pow(2,k)}");
                    return (a[k + 1] + b[k + 1]) / 2;
                }
                
                if (!((a[k] <= y[k]) && (y[k] <= b[k])) || !((a[k] <= z[k]) && (z[k] <= b[k])))
                {
                    Console.WriteLine($"y_k и z_k не попали в отрезок локализации!");
                    Console.WriteLine($"a_k= {a[k]}, b_k= {b[k]}, y_k={y[k]}, z_k={z[k]}");
                }
                k += 1;
                
                goto fourth;
            }
        }
    }
}
