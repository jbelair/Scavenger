using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Schemes : MonoBehaviour
{
    public static Schemes active;
    public static Dictionary<string, Scheme> lookup;

    public List<Scheme> schemes;
    public bool forceLoad = false;

    public static Scheme Scheme(string name)
    {
        if (!lookup.ContainsKey(name))
            return lookup["default"];

        return lookup[name];
    }

    public static void Scheme(Scheme scheme)
    {
        if (lookup.ContainsKey(scheme.name))
            lookup[scheme.name] = scheme;
        else
            lookup.Add(scheme.name, scheme);
    }

    public void Awake()
    {
        active = this;

        if (lookup == null)
            Load();

        if (schemes == null || forceLoad)
        {
            schemes.AddRange(lookup.Values);
        }
    }

    public void OnDestroy()
    {
        if (active == this)
            active = null;
    }

    [ExposeMethodInEditor]
    public void Load()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>("UI/Schemes");

        schemes = new List<Scheme>();
        lookup = new Dictionary<string, Scheme>();

        foreach(TextAsset asset in assets)
        {
            //Debug.Log(asset.name );
            SchemeDefinitions scheme = JsonUtility.FromJson<SchemeDefinitions>(asset.text);
            foreach(SchemeDefinition def in scheme.definitions)
            {
                Color colour = Color.white;
                ColorUtility.TryParseHtmlString((def.colour[0] == '#') ? def.colour : "#" + def.colour, out colour);
                schemes.Add(new Scheme(def.name, asset.name, colour, Sprites.Get(def.symbol)));
                lookup.Add(def.name, schemes[schemes.Count - 1]);
            }
        }
    }

    [ExposeMethodInEditor]
    private void Save()
    {
        Dictionary<string, SchemeDefinitions> definition = new Dictionary<string, SchemeDefinitions>();

        foreach(Scheme scheme in schemes)
        {
            SchemeDefinition def = new SchemeDefinition() { name = scheme.name, symbol = scheme.symbol ? scheme.symbol.name : "", colour = ColorUtility.ToHtmlStringRGBA(scheme.colour) };

            if (!definition.ContainsKey(scheme.file))
                definition.Add(scheme.file, new SchemeDefinitions());

            definition[scheme.file].definitions.Add(def);
        }

        foreach (KeyValuePair<string, SchemeDefinitions> definitionPair in definition)
        {
            StreamWriter writer = File.CreateText("Assets/Resources/UI/Schemes/" + definitionPair.Key + ".json");
            writer.Write(JsonUtility.ToJson(definitionPair.Value, true));
            writer.Close();
        }
    }
}
