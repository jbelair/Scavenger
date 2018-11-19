using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ships : MonoBehaviour
{
    [Serializable]
    public struct Definitions
    {
        public List<ShipDefinition> definitions;
    }

    public static Dictionary<string, GameObject> ships = new Dictionary<string, GameObject>();
    public static Dictionary<string, ShipDefinition> definitions = new Dictionary<string, ShipDefinition>();

    public List<GameObject> loaded = new List<GameObject>();

    private void Awake()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>("Ships/");

        foreach(TextAsset asset in assets)
        {
            Definitions defs = JsonUtility.FromJson<Definitions>(asset.text);
            foreach(ShipDefinition def in defs.definitions)
            {
                if (ships.ContainsKey(def.name))
                    continue;

                definitions.Add(def.name, def);
                ships.Add(def.name, Resources.Load<GameObject>(def.model));
                loaded.Add(ships[def.name]);
            }
        }
    }
}
