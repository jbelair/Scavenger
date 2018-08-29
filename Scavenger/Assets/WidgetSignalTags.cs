using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSignalTags : MonoBehaviour
{
    public WidgetSignalTag tagPrefab;

    public DungeonType type;
    float radius = 48;

    public void Set(DungeonType type)
    {
        this.type = type;

        string[] split = type.tags.Split(' ');

        int i = 1;
        float theta = (Mathf.PI * 2) / 8f;
        foreach (string tag in split)
        {
            if (tag != "" && tag != type.category && i < 8 && WidgetScheme.active.Scheme(tag).colour != Color.white)
            {
                WidgetSignalTag tagInst = Instantiate(tagPrefab, transform);
                tagInst.GetComponent<RectTransform>().localPosition = new Vector2(Mathf.Cos(theta * i) * radius, Mathf.Sin(theta * i) * radius);
                tagInst.tag = tag;
                i++;
            }
        }
    }
}
