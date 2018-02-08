using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SkillSequenceKey
{
    public string name;
    public float percentage;
    public float cooldownOverride;
    public GameObject[] instantiables;
}
