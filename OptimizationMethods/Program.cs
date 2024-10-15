namespace OptimizationMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a = 2.3;
            double b = 3.01;
            //double a = 1;
            //double b = 2;
            double epsilon = Math.Pow(10, -6);

            Func<double, double> function = (x) => -Math.Cbrt((4 - x) * 2 * (Math.Pow(x - 1, 2)));
            //Func<double, double> function = (x) => 2*x+1/x;

            var result = ByBitSearch.SearchByBit(function, a, b, epsilon);
            var val = function(result);


            Console.WriteLine($"Минимум на отрезке [{a};{b}]: \nx* = {result}, f(x*) = {val}");
        }
    }
}
