using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DungeonLoader : MonoBehaviour
{
    [Serializable]
    public class DungeonTypeArray
    {
        public List<DungeonType> signals = new List<DungeonType>();
    }

    public static DungeonLoader active;

    public TextAsset[] signalsFiles;
    public DungeonTypeArray dungeons;

    private void Start()
    {
        active = this;
        //dungeons = JsonUtility.FromJson<DungeonTypeArray>(signalsFile.ToString());
        foreach(TextAsset signalsFile in signalsFiles)
        {
            DungeonTypeArray loadedArray = JsonUtility.FromJson<DungeonTypeArray>(signalsFile.ToString());
            dungeons.signals.AddRange(loadedArray.signals);
        }
    }

    private void OnDestroy()
    {
        if (this == active)
            active = null;
    }
}
