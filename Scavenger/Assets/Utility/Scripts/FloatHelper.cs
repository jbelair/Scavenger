using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class FloatHelper
{
    public static float RiskStringToFloat(string risk)
    {
        switch(risk)
        {
            case "risk_none":
                return 0;
            case "risk_low":
                return 1;
            case "risk_medium":
                return 2;
            case "risk_high":
                return 3;
            case "risk_extreme":
                return 4;
            case "risk_fatal":
                return 5;
            default:
                return -1;
        }
    }
}
