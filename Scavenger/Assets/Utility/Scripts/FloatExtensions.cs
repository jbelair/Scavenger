using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class FloatExtensions
{
    public static float RoundTo(this float f, int decimals)
    {
        int pow = (int)Mathf.Pow(10, decimals);
        return Mathf.Round(f * pow) / pow;
    }
}
