using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSignalTagGrid : MonoBehaviour
{
    public WidgetSignalTag signalTagPrefab;
    public DungeonType type;
    public RectTransform grid;

    public void Set(DungeonType type)
    {
        this.type = type;

        foreach(GameObject child in grid.transform)
        {
            Destroy(child);
        }

        string tags = type.tags;
        foreach(string tag in type.tags.Split(' '))
        {
            if (tag != "")
            {
                WidgetSignalTag signalTag = Instantiate(signalTagPrefab, grid);
                signalTag.tag = tag;
            }
        }
    }
}
