﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Schemes : MonoBehaviour
{
    public static Schemes active;
    public static List<Scheme> lastSchemes;

    public List<Scheme> schemes;
    public bool forceLoad = false;

    public static Scheme Scheme(string name)
    {
        return active.schemes.Find(scheme => scheme.name == name);
    }

    public void Awake()
    {
        active = this;

        if (lastSchemes == null)
            Load();

        if (schemes == null || forceLoad)
            schemes = lastSchemes;
    }

    public void OnDestroy()
    {
        if (active == this)
            active = null;

        lastSchemes = schemes;
    }

    [ExposeMethodInEditor]
    public void Load()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>("UI/Schemes");

        schemes = new List<Scheme>();

        foreach(TextAsset asset in assets)
        {
            SchemeDefinitions scheme = JsonUtility.FromJson<SchemeDefinitions>(asset.text);
            foreach(SchemeDefinition def in scheme.definitions)
            {
                Color colour = Color.white;
                ColorUtility.TryParseHtmlString((def.colour[0] == '#') ? def.colour : "#" + def.colour, out colour);
                schemes.Add(new Scheme() { name = def.name, symbol = Resources.Load<Sprite>("UI/Icons/" + def.symbol), colour = colour });
            }
        }
    }

    [ExposeMethodInEditor]
    private void Save()
    {
        SchemeDefinitions definition = new SchemeDefinitions();

        foreach(Scheme scheme in schemes)
        {
            definition.definitions.Add(new SchemeDefinition() { name = scheme.name, symbol = scheme.symbol ? scheme.symbol.name : "", colour = ColorUtility.ToHtmlStringRGBA(scheme.colour) });
        }

        StreamWriter writer = File.CreateText("Assets/Resources/UI/Schemes/schemes.json");
        writer.Write(JsonUtility.ToJson(definition,true));
        writer.Close();
    }
}
