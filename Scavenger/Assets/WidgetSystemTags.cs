using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSystemTags : MonoBehaviour
{
    public WidgetSignalTag tagPrefab;

    public float radius = 22;
    public List<string> tags;

    public void Set(List<DungeonType> types)
    {
        tags = new List<string>();
        int i = 1;
        float theta = (Mathf.PI * 2) / 8f;
        foreach (DungeonType type in types)
        {
            List<string> split = new List<string>
            {
                "Rarity " + StringHelper.RarityIntToString(type.oneIn)
            };
            split.AddRange(type.tags.Split(' '));
            
            foreach (string tag in split)
            {
                Color colour = WidgetScheme.active.Scheme(tag).colour;
                if (tag != "" 
                    && tag != type.category
                    && i < 5 
                    && colour != Color.white 
                    && colour != WidgetScheme.active.Scheme("Rarity Abundant").colour 
                    && colour != WidgetScheme.active.Scheme("Rarity Common").colour 
                    && colour != WidgetScheme.active.Scheme("Rarity Uncommon").colour
                    && colour != WidgetScheme.active.Scheme("Rarity Rare").colour
                    //&& (tag == "Respawn"
                    //|| tag == "Repair"
                    //|| tag == "Conflict")
                    && !tags.Contains(tag))
                {
                    WidgetSignalTag tagInst = Instantiate(tagPrefab, transform);
                    tagInst.GetComponent<RectTransform>().localPosition = new Vector2(Mathf.Cos(theta * (i + 1f)) * radius, Mathf.Sin(theta * (i + 1f)) * radius);
                    tagInst.tag = tag;
                    tags.Add(tag);
                    i++;
                }
            }
        }
    }
}
