using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayerRecursively(this GameObject gameObject, LayerMask layer)
    {
        gameObject.layer = layer;

        foreach(GameObject child in gameObject.transform)
        {
            child.layer = layer;
        }
    }
}
