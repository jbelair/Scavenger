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
    
    public static DungeonTypeArray dungeons;

    private void Awake()
    {
        active = this;

        if (dungeons == null)
        {
            dungeons = new DungeonTypeArray();

            List<string> categories = new List<string>();
            List<string> targets = new List<string>();
            List<string> tags = new List<string>();

            TextAsset[] signalsFiles = Resources.LoadAll<TextAsset>("Dungeons");

            foreach (TextAsset signalsFile in signalsFiles)
            {
                DungeonTypeArray loadedArray = JsonUtility.FromJson<DungeonTypeArray>(signalsFile.ToString());
                dungeons.signals.AddRange(loadedArray.signals.Where(
                    dungeon => dungeon.category != null 
                    && dungeon.description != null 
                    && dungeon.generator != null 
                    && dungeon.name != null 
                    && dungeon.risk != null 
                    && dungeon.tags != null 
                    && dungeon.target != null));

                foreach (DungeonType type in loadedArray.signals)
                {
                    if (!categories.Contains(type.category))
                        categories.Add(type.category);

                    if (!targets.Contains(type.target))
                        targets.Add(type.target);

                    string[] split = StringHelper.TagParseAll(type.tags);
                    foreach(string tag in split)
                    {
                        if (!tags.Contains(tag) && tag[0] != '$')
                            tags.Add(tag);
                    }
                }

            }

            DungeonType.categories = categories.ToArray();
            DungeonType.targets = targets.ToArray();
            DungeonType.allTags = tags.ToArray();
        }
    }

    private void OnDestroy()
    {
        if (this == active)
            active = null;
    }
}
