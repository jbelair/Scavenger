using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsFilter : MonoBehaviour
{
    public static List<string> Filters = new List<string>();

    public static SystemsFilter active;
    public static string lastFilter = ""; // TODO this needs to load from last game state
    public static string lastFilterTags = "";

    public string filter = "";
    public string filterTags = "";

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("filter"))
            filter = PlayerPrefs.GetString("filter", "Distance");

        if (PlayerPrefs.HasKey("filter tags"))
            filterTags = PlayerPrefs.GetString("filter tags", "");

        if (lastFilter != "")
            filter = lastFilter;

        if (lastFilterTags != "")
            filterTags = lastFilterTags;

        active = this;
    }

    public void Set(string set)
    {
        filter = set;
    }

    public void SetTags(string set)
    {
        filterTags = set;
        lastFilterTags = filterTags;
    }

    private void OnDestroy()
    {
        lastFilter = filter;
        lastFilterTags = filterTags;

        PlayerPrefs.SetString("filter", filter);
        PlayerPrefs.SetString("filter tags", filterTags);
        //PlayerPrefs.Save();
    }
}
