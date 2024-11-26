using System;
using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;
using MathNet.Symbolics;
namespace OptimizationMethods;

public interface IMethod
{
    public abstract static double Search(SymbolicExpression function,double x0, double epsilon, SymbolicExpression X);

}
