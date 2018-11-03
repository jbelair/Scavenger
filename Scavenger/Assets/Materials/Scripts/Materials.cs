using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    public static Dictionary<string, Material> materials = new Dictionary<string, Material>();

    private void Awake()
    {
        Material[] mats = Resources.LoadAll<Material>("Materials/");
        foreach(Material material in mats)
        {
            materials.Add(material.name, material);
        }
    }
}
