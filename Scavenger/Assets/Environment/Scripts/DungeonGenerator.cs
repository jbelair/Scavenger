using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Risk { None, Low, Medium, High, Extreme, Fatal };

public class DungeonGenerator : MonoBehaviour
{
    public static List<GameObject> widgets = new List<GameObject>();

    public int hash;
    public bool generates = false;
    public DungeonType dungeonType;
    public string dungeonTarget;
    public float riskLevel = 0;
    public string risk;

    public CameraNode node;
    public GameObject widget;

    // Use this for initialization
    void Start()
    {
        if (generates)
        {
            node = GetComponentInChildren<CameraNode>();
            risk = StringHelper.RiskIntToString(Mathf.FloorToInt(riskLevel));

            widget = UIManager.active.Button("system navigation", UIManager.Layer.Back, "Signal", Vector2.zero, Transition, transform);
            widgets.Add(widget);
            widget.GetComponent<WidgetSignal>().Set(dungeonType);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Destroy(widget);
    }

    public void Transition()
    {
        Camera.main.GetComponent<MoveTo>().TransitionFrame(node.transform, 2.5f);

        //foreach(GameObject widg in widgets)
        //    widg.SetActive(false);
        UIManager.active.AddScreen("system dungeon");
        UIManager.active.RemoveScreen("system navigation");
        UIManager.active.RemoveScreen("systems navigation");

        widget = UIManager.active.Element("system dungeon", UIManager.Layer.Mid, "Signal Description");//, new Vector2(1920/2f, 0f));
        widget.GetComponent<WidgetSignalDescription>().Set(dungeonType);
    }
}
