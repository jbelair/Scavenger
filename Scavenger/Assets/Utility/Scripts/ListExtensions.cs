using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int index = Random.Range(0, list.Count);
            list[i] = list[index];
            list[index] = temp;
        }
    }
}
