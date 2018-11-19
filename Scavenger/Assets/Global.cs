using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Dictionary<string, Global> globals = new Dictionary<string, Global>();

    // Use this for initialization
    void Awake()
    {
        if (globals.ContainsKey(name))
            DestroyImmediate(gameObject);
        else
        {
            globals.Add(name, this);
            DontDestroyOnLoad(this);
        }
    }
}
