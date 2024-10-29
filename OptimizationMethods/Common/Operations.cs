using System;
namespace OptimizationMethods.Common
{
    public static class Operations{
        public static double Derivate1D(Func<double,double> function, double point, double dx = 0.001){
            
            return (function(point+dx)-function(point))/dx;
        }
    }
}