using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Sprites
{
    public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public static Sprite Get(string sprite)
    {
        if (!sprites.ContainsKey(sprite))
            sprites[sprite] = Resources.Load<Sprite>("Sprites/" + sprite);

        return sprites[sprite];
    }
}
