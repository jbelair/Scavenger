using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetGenerator : MonoBehaviour
{
    public static WidgetGenerator active;

    [System.Serializable]
    public class Widget
    {
        public string name;
        public GameObject widget;
    }

    public enum Layer { Front, Mid, Back };

    public Canvas canvas;
    public GameObject back;
    public GameObject mid;
    public GameObject front;
    public List<Widget> widgets;

    // Use this for initialization
    void Start()
    {
        active = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetLayer(Layer layer)
    {
        switch(layer)
        {
            case Layer.Back:
                return back.transform;
            case Layer.Mid:
                return mid.transform;
            case Layer.Front:
                return front.transform;
        }

        return back.transform;
    }

    public GameObject Button(Layer layer, string name, Vector2 screenPos, UnityEngine.Events.UnityAction action = null, Transform target = null)
    {
        Transform parent = GetLayer(layer);
        GameObject widget = widgets.Find(look => look.name == name).widget;
        widget = Instantiate(widget, parent);
        widget.GetComponent<RectTransform>().position = screenPos;

        if (action != null)
        {
            widget.GetComponent<Button>().onClick.AddListener(action);
        }

        if (target != null)
        {
            WidgetWorldPosition worldPosition = widget.GetComponent<WidgetWorldPosition>();
            worldPosition.target = target;
            worldPosition.canvas = canvas.GetComponent<RectTransform>();
        }

        return widget;
    }

    public GameObject Element(Layer layer, string name)
    {
        Transform parent = GetLayer(layer);
        GameObject widget = widgets.Find(look => look.name == name).widget;
        widget = Instantiate(widget, parent);
        //widget.GetComponent<RectTransform>().position = screenPos;
        return widget;
    }

    public void EnableDungeonWidgets()
    {
        foreach (GameObject widg in DungeonGenerator.widgets)
            widg.SetActive(true);
    }

    public void ClearBack()
    {
        foreach (Transform trans in back.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    public void ClearMid()
    {
        foreach(Transform trans in mid.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    public void ClearFront()
    {
        foreach (Transform trans in front.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    public void OnDestroy()
    {
        if (active == this)
            active = null;
    }
}
