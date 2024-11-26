using System;
using MathNet.Symbolics;
namespace OptimizationMethods;

public class Common
{
    public static Dictionary<string,FloatingPoint> GetPointX(double x){
        return new Dictionary<string,FloatingPoint>(){{"x",x}};
    }

}
