using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetScheme : MonoBehaviour
{
    public static WidgetScheme active;
    public static List<SchemeContainer> lastSchemes;

    [System.Serializable]
    public struct SchemeContainer
    {
        public string name;
        public Color colour;
        public Sprite symbol;
    }

    public List<SchemeContainer> schemes;
    public bool forceLoad = false;

    public SchemeContainer Scheme(string name)
    {
        return schemes.Find(scheme => scheme.name == name);
    }

    public void Awake()
    {
        active = this;
        if (schemes == null || forceLoad)
            schemes = lastSchemes;
    }

    public void OnDestroy()
    {
        if (active == this)
            active = null;

        lastSchemes = schemes;
    }
}
