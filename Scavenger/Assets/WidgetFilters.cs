using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetFilters : MonoBehaviour
{
    public class Filter
    {
        public string target;
        public string name;
        public string filter;
        public string selected;
        public string deselected;
    }

    public WidgetFilter prefab;

    public List<WidgetFilter> filters = new List<WidgetFilter>();

    // Use this for initialization
    void Start()
    {
        // Load all the defined filter definitions from resources
        TextAsset[] definitions = Resources.LoadAll<TextAsset>("UI/Filters");
        foreach(TextAsset definition in definitions)
        {
            Filter filter = JsonUtility.FromJson<Filter>(definition.text);
            if (filter.target.Contains("Systems"))
            {
                WidgetFilter inst = Instantiate(prefab, transform);
                inst.name = inst.label.text = filter.name;
                inst.filter = filter.filter;
                inst.selected = filter.selected;
                inst.deselected = filter.deselected;
                inst.filters = this;
                filters.Add(inst);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        foreach(WidgetFilter filter in filters)
        {
            filter.isActive = SystemsFilter.active.filter == filter.filter;
            //filter.Select(false);
            filter.Select(filter.isActive);
            //filter.Refresh();
        }
    }
}
