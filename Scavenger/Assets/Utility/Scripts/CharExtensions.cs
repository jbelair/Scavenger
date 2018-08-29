using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class CharExtensions
{
    public static bool IsNumber(this char chr)
    {
        return chr == '0' || chr == '1' || chr == '2' || chr == '3' || chr == '4' || chr == '5' || chr == '6' || chr == '7' || chr == '8' || chr == '9';
    }
}
