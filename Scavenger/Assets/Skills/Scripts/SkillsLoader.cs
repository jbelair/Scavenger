using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillsLoader : MonoBehaviour
{
    public List<Skill> loaded = new List<Skill>();

    private void Awake()
    {
        Load();
    }

    [ExposeMethodInEditor]
    public void Load()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>("Skills/");
        loaded = new List<Skill>();
        Skills.skills = new List<Skill>();
        foreach (TextAsset asset in assets)
        {
            SkillDefinition definition = JsonUtility.FromJson<SkillDefinition>(asset.text);
            foreach (Skill skill in definition.definitions)
            {
                Skills.skills.Add(skill);
                loaded.Add(skill);
                Skills.skillNames.Add(skill.name);
            }
        }
    }

    [ExposeMethodInEditor]
    public void Save()
    {

    }
}
