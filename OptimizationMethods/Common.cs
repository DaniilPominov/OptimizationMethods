using MathNet.Symbolics;
using MathNet.Numerics.LinearAlgebra;
using Expr = MathNet.Symbolics.SymbolicExpression;
namespace OptimizationMethods;

public class Common
{
    public static Dictionary<string,FloatingPoint> GetPointX(double x){
        return new Dictionary<string,FloatingPoint>(){{"x",x}};
    }
    public static Dictionary<string, FloatingPoint> BuildPointDict(Vector<double> point, List<Expr> vars)
    {
        var symbols = new Dictionary<string, FloatingPoint>();
        for (int i = 0; i < vars.Count; i++)
        {
            symbols[vars[i].VariableName] = point[i];
        }
        return symbols;
    }

}
