using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSystemTags : MonoBehaviour
{
    public WidgetSignalTag tagPrefab;

    public int startIndex = 1;
    public int endIndex = 5;
    public float radius = 22;
    public List<string> tags;

    public void Set(List<DungeonType> types)
    {
        tags = new List<string>();
        int i = startIndex;
        float theta = (Mathf.PI * 2) / 8f;
        foreach (DungeonType type in types)
        {
            if (type.name == null || type.description == null || type.category == null || type.risk == null || type.target == null || type.generator == null || type.tags == null)
                continue;

            List<string> split = new List<string>
            {
                StringHelper.RarityIntToString(type.oneIn)
            };
            split.AddRange(type.tags.Split(new string[] { " ", ", ", "," }, System.StringSplitOptions.RemoveEmptyEntries));
            
            foreach (string tag in split)
            {
                Color colour = WidgetScheme.Scheme(tag).colour;
                if (tag != "" 
                    && tag != type.category
                    && i < endIndex
                    && colour != Color.white 
                    && colour != WidgetScheme.Scheme("rarity_abundant").colour 
                    && colour != WidgetScheme.Scheme("rarity_common").colour 
                    && colour != WidgetScheme.Scheme("rarity_uncommon").colour
                    && colour != WidgetScheme.Scheme("rarity_rare").colour
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
