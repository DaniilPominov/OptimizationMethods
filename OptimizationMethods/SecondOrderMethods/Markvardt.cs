using System;
using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;
using MathNet.Symbolics;
using OptimizationMethods;

namespace OptimizationMethods.SecondOrderMethods;

public class Markvardt : IMethod
{
    public static double Search(SymbolicExpression function,double x0, double epsilon, SymbolicExpression X)
    {
        var GetPointX = Common.GetPointX;
        Dictionary<int, double> x = new();
        int k = 0;
        double mu = 10;
        var derivate = function.Differentiate(X);
        var derivate2 = derivate.Differentiate(X);
        Console.WriteLine("second derivate at x0");
        Console.WriteLine(derivate2.Evaluate(GetPointX(x0)).RealValue);

        if(derivate2.Evaluate(GetPointX(x0)).RealValue>=mu){
            mu = Math.Abs(derivate2.Evaluate(GetPointX(x0)).RealValue)*10;
        }
        goto first;
        first:{
            k = 1;
            x[k] = x0 - derivate.Evaluate(GetPointX(x0)).RealValue/derivate2.Evaluate(GetPointX(x0)).RealValue;
            goto second;
        }
        second:
        {
            x[k+1] = x[k] - derivate.Evaluate(GetPointX(x[k])).RealValue/(derivate2.Evaluate(GetPointX(x[k])).RealValue+mu);
            if(function.Evaluate(GetPointX(x[k+1])).RealValue < function.Evaluate(GetPointX(x[k])).RealValue){
                mu = mu/2;
            }
            else{
                mu = 2*mu;
            }
            goto thirth;

        }
        thirth:{
            //if(Math.Abs(derivate.Evaluate(GetPointX(x[k+1])).RealValue) <= epsilon){
            if(Math.Abs(x[k+1]-x[k])<=epsilon){
                Console.WriteLine($"steps left: {k}");
                return x[k+1];
            }
            else{
                if(k>10000){
                    Console.WriteLine("Max steps reached");
                    return x[k+1];
                }
                k++;
                goto second;
            }
        }
    }

}
