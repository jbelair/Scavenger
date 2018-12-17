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
            sprites.Add(sprite, Resources.Load<Sprite>("Sprites/" + sprite));

        if (sprites[sprite] == null)
        {
            if (!sprites.ContainsKey("fill_128x128_white_sprite"))
                sprites.Add("fill_128x128_white_sprite", Resources.Load<Sprite>("Sprites/fill_128x128_white_sprite"));

            return sprites["fill_128x128_white_sprite"];
        }


        return sprites[sprite];
    }
}
