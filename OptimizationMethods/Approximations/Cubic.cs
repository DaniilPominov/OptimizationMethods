using System;
using MathNet.Symbolics;
using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;

namespace OptimizationMethods.Approximations;

public class Cubic
{
    public static double Search(SymbolicExpression function, double x1, double x2, double epsilon, SymbolicExpression X)
    {
        double xWave = 0, nu = 0,w,z;
        int k = 0;
        var GetPointX = Common.GetPointX;
        var derivate = function.Differentiate(X);
        var f1der = derivate.Evaluate(GetPointX(x1)).RealValue;
        var f2der = derivate.Evaluate(GetPointX(x2)).RealValue;
        if(f1der*f2der>=0){
            if(f1der>0){
            return Math.Min(x1,x2);
            }
            else{
                return Math.Max(x1,x2);
            }
        }

        goto first;
        first:{
        f1der = derivate.Evaluate(GetPointX(x1)).RealValue;
        f2der = derivate.Evaluate(GetPointX(x2)).RealValue;
        var f1 = function.Evaluate(GetPointX(x1)).RealValue;
        var f2 = function.Evaluate(GetPointX(x2)).RealValue;
        z = f1der + f2der - 3*(f2-f1)/(x2-x1);
        w = Math.Pow((z*z-f1der*f2der),0.5);
        nu = (w+z-f1der)/(2*w-f1der+f2der);
        xWave = x1 + nu*(x2-x1);
        goto second;
        }
        second:{
            k++;
            var currDeriv = derivate.Evaluate(GetPointX(xWave)).RealValue;
            if(currDeriv==0 || Math.Abs(x1-x2)<=epsilon){
                Console.WriteLine($"Steps left: {k}");
                return xWave;
            }
            if(currDeriv>0){
                x2 = xWave;
                goto first;
            }
            x1 = xWave;
            goto first;
        }

    }
}
