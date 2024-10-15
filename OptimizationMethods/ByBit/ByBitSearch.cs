using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethods.ByBit
{
    internal class ByBitSearch
    {
        internal static double SearchByBit(Func<double, double> function, double a, double b, double epsilon)
        {

            double x_0, x_1, f_0, f_1, h;
            goto first;
        first:
            {
                h = (b - a) / 4;
                x_0 = a;
                f_0 = function(x_0);
                goto second;
            }
        second:
            {
                x_1 = x_0 + h;
                f_1 = function(x_1);
                goto thirth;
            }
        thirth:
            {
                if (f_0 > f_1)
                {
                    goto fourth;
                }
                else
                {
                    goto fifth;
                }

            }
        fourth:
            {
                x_0 = x_1;
                f_0 = f_1;
                if (a < x_0 && x_0 < b)
                {
                    goto second;
                }
                else
                {
                    goto fifth;
                }
            }
        fifth:
            {
                if (Math.Abs(h) <= epsilon)
                {
                    return x_0;
                }
                else
                {
                    goto sixth;
                }
            sixth:
                {
                    x_0 = x_1;
                    f_0 = f_1;
                    h = -h / 4.0;
                    goto second;
                }
            }
        }
    }
}
