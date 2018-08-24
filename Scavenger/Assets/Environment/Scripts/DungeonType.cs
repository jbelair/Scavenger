using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonType
{
    public string name;
    [TextArea(3, 10)]public string description;
    public string category;
    public string risk;
    public int oneIn;
    public string target;
    public string generator;

    private class DungeonComparer : IComparer<DungeonType>
    {
        public int Compare(DungeonType x, DungeonType y)
        {
            if (x.oneIn > y.oneIn)
                return 1;
            else if (x.oneIn == y.oneIn)
                return 0;
            else
                return -1;
        }

        public static IComparer<DungeonType> MakeDungeonComparer()
        {
            return (IComparer<DungeonType>)new DungeonComparer();
        }
    }

    public static DungeonType SelectByChance(List<DungeonType> dungeons)
    {
        if (dungeons.Count > 0)
        {
            DungeonType ret = dungeons[0];

            dungeons.Sort(DungeonComparer.MakeDungeonComparer());

            List<int> lowestProbabilities = new List<int>();
            int lowestProbability = 0;
            List<int> highestProbabilities = new List<int>();
            int highestProbability = int.MaxValue;

            for (int i = 0; i < dungeons.Count; i++)
            {
                int score = (1 == Random.Range(1, dungeons[i].oneIn)) ? dungeons[i].oneIn : 0;

                if (dungeons[i].oneIn < highestProbability)
                {
                    highestProbability = dungeons[i].oneIn;
                    highestProbabilities.Clear();
                    highestProbabilities.Add(i);
                }
                else if (dungeons[i].oneIn == highestProbability)
                {
                    highestProbabilities.Add(i);
                }

                if (score > 0)
                {
                    if (dungeons[i].oneIn > lowestProbability)
                    {
                        lowestProbability = dungeons[i].oneIn;
                        lowestProbabilities.Clear();
                        lowestProbabilities.Add(i);
                    }
                    else if (dungeons[i].oneIn == lowestProbability)
                    {
                        lowestProbabilities.Add(i);
                    }
                }
            }

            if (lowestProbabilities.Count > 0)
                ret = dungeons[lowestProbabilities[Random.Range(0, lowestProbabilities.Count)]];
            else
                ret = dungeons[highestProbabilities[Random.Range(0, highestProbabilities.Count)]];

            return ret;
        }
        else
            throw new System.Exception("Attempted to select a dungeon by chance but passed an empty list of dungeons");
    }
}
