using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetScheme : MonoBehaviour
{
    public static WidgetScheme active;

    [System.Serializable]
    public struct SchemeContainer
    {
        public string name;
        public Color colour;
        public Sprite symbol;
    }

    public List<SchemeContainer> schemes;

    public SchemeContainer Scheme(string name)
    {
        return schemes.Find(scheme => scheme.name == name);
    }

    public void Start()
    {
        active = this;
    }

    public void OnDestroy()
    {
        if (active == this)
            active = null;
    }
}
