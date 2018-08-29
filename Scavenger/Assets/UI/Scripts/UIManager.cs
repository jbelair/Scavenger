using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager active;

    [System.Serializable]
    public class Widget
    {
        public string name;
        public GameObject widget;
    }

    public enum Layer { Front, Mid, Back };

    public Canvas canvas;
    public List<UIScreen> screens;
    public List<Widget> widgets;

    // Use this for initialization
    void Start()
    {
        active = this;

        foreach(UIScreen screen in screens)
        {
            screen.gameObject.SetActive(screen.defaultScreen);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetLayer(string screen, Layer layer)
    {
        UIScreen scrn = screens.Find(search => search.name == screen);

        if (!scrn)
            return canvas.transform;

        switch(layer)
        {
            case Layer.Back:
                return scrn.back.transform;
            case Layer.Mid:
                return scrn.mid.transform;
            case Layer.Front:
                return scrn.front.transform;
        }

        return scrn.back.transform;
    }

    public GameObject Button(string screen, Layer layer, string name, Vector2 screenPos, UnityEngine.Events.UnityAction action = null, Transform target = null)
    {
        Transform parent = GetLayer(screen, layer);
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

    public GameObject Element(string screen, Layer layer, string name)
    {
        Transform parent = GetLayer(screen, layer);
        GameObject widget = widgets.Find(look => look.name == name).widget;
        widget = Instantiate(widget, parent);
        return widget;
    }

    public void SwitchScreen(string screen)
    {
        foreach (UIScreen scrn in screens)
        {
            scrn.gameObject.SetActive(scrn.name == screen);
        }
    }

    public void ClearBack(string screen)
    {
        UIScreen scrn = screens.Find(search => search.name == screen);

        if (!scrn)
            return;

        foreach (Transform trans in scrn.back.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    public void ClearMid(string screen)
    {
        UIScreen scrn = screens.Find(search => search.name == screen);

        if (!scrn)
            return;

        foreach (Transform trans in scrn.mid.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    public void ClearFront(string screen)
    {
        UIScreen scrn = screens.Find(search => search.name == screen);

        if (!scrn)
            return;

        foreach (Transform trans in scrn.front.transform)
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
