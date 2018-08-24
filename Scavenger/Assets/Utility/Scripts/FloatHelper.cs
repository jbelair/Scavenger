using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class FloatHelper
{
    public static float RiskStringToFloat(string risk)
    {
        switch(risk.ToLower())
        {
            case "none":
                return 0;
            case "low":
                return 1;
            case "medium":
                return 2;
            case "high":
                return 3;
            case "extreme":
                return 4;
            case "fatal":
                return 5;
            default:
                return 0;
        }
    }
}
