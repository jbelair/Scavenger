using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSignalTags : MonoBehaviour
{
    public WidgetSignalTag tagPrefab;

    public DungeonType type;
    public float radius = 48;
    public int tags = 8;

    public void Set(DungeonType type)
    {
        this.type = type;

        List<string> split = new List<string>
        {
            "Rarity " + StringHelper.RarityIntToString(type.oneIn)
        };
        split.AddRange(type.tags.Split(' '));

        int i = 1;
        float theta = (Mathf.PI * 2f) / tags;
        foreach (string tag in split)
        {
            if (tag != "" && tag != type.category && i < tags)// && WidgetScheme.active.Scheme(tag).colour != Color.white)
            {
                WidgetSignalTag tagInst = Instantiate(tagPrefab, transform);
                tagInst.GetComponent<RectTransform>().localPosition = new Vector2(Mathf.Cos(theta * i) * radius, Mathf.Sin(theta * i) * radius);
                tagInst.tag = tag;
                i++;
            }
        }
    }
}
