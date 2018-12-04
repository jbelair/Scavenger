using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Maps
{
    public static Dictionary<string, Texture2D> maps = new Dictionary<string, Texture2D>();

    public static Texture2D Map(string name)
    {
        Texture2D ret = maps["stars"];
        maps.TryGetValue(name, out ret);
        return ret;
    }

    static Maps()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Map");

        foreach (Texture2D texture in textures)
        {
            maps.Add(texture.name, texture);
        }
    }
}
