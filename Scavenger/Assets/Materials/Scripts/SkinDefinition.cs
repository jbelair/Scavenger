using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SkinDefinition
{
    [Serializable]
    public class Skin
    {
        public string name;
        public string skin;
        public int oneIn;
        public bool starting;
        public Color[] colours;
    }

    public List<Skin> skins = new List<Skin>();
}
