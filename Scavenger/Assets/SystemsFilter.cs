using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsFilter : MonoBehaviour
{
    public enum Filter { None, DistanceAsRarity, DistanceAsRisk, RarityAsRarity, RarityAsRisk, RiskAsRarity, RiskAsRisk };

    public static SystemsFilter active;
    public static Filter lastFilter = Filter.None; // TODO this needs to load from last game state
    public static string lastFilterTags = "";

    public Filter filter = Filter.DistanceAsRarity;
    public string filterTags = "";

    // Use this for initialization
    void Start()
    {
        if (lastFilter != Filter.None)
            filter = lastFilter;

        if (lastFilterTags != "")
            filterTags = lastFilterTags;

        active = this;
    }

    public void Set(string set)
    {
        switch(set)
        {
            case "Distance Rarity":
                filter = Filter.DistanceAsRarity;
                break;
            case "Distance Risk":
                filter = Filter.DistanceAsRisk;
                break;
            case "Rarity":
                filter = Filter.RarityAsRarity;
                break;
            case "Rarity Risk":
                filter = Filter.RarityAsRisk;
                break;
            case "Risk Rarity":
                filter = Filter.RiskAsRarity;
                break;
            case "Risk":
                filter = Filter.RiskAsRisk;
                break;
        }
    }

    public void SetTags(string set)
    {
        filterTags = set;
        lastFilterTags = filterTags;
    }

    private void OnDestroy()
    {
        lastFilter = filter;
    }
}
