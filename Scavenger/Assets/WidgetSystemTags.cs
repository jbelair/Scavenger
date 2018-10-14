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
            if (type.name == null || type.description == null || type.category == null || type.risk == null || type.target == null || type.generator == null || type.tags == null)
                continue;

            List<string> split = new List<string>
            {
                StringHelper.RarityIntToString(type.oneIn)
            };
            split.AddRange(type.tags.Split(' '));
            
            foreach (string tag in split)
            {
                Color colour = WidgetScheme.Scheme(tag).colour;
                if (tag != "" 
                    && tag != type.category
                    && i < 5 
                    && colour != Color.white 
                    && colour != WidgetScheme.Scheme("rarity_abundant").colour 
                    && colour != WidgetScheme.Scheme("rarity_common").colour 
                    && colour != WidgetScheme.Scheme("rarity_uncommon").colour
                    && colour != WidgetScheme.Scheme("rarity_rare").colour
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
