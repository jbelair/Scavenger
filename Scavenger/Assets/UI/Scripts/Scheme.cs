using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scheme
{
    public string name;
    public string file;
    public Color colour;
    public Sprite symbol;

    public Scheme(string name, string file, Color colour, Sprite symbol)
    {
        this.name = name;
        this.file = file;
        this.colour = colour;
        this.symbol = symbol;
    }

    public Scheme(Scheme scheme)
    {
        name = scheme.name;
        file = scheme.file;
        colour = scheme.colour;
        symbol = scheme.symbol;
    }
}
