using System.Security.Cryptography.X509Certificates;
using OptimizationMethods.ByBit;
using OptimizationMethods.ExclusionMethods;
using OptimizationMethods.FirstOrderMethods;

namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a = -1;
            double b = 2;
            double epsilon = Math.Pow(10, -5);

            //Func<double, double> function = static (x) => -Math.Cbrt((4 - x) * 2 * Math.Pow(x - 1, 2));
            Func<double, double> function = static (x) => Math.Pow(x,4)+Math.Pow(x,2)+x+1;

            var result = Chord.Search(function,a,b,epsilon);
            var val = function(result);


            Console.WriteLine($"Минимум на отрезке [{a};{b}]: \nx* = {result}, f(x*) = {val}");
        }
    }
}
