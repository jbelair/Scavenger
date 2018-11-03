using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetMoveToLayer : MonoBehaviour
{
    public UIScreen screen;

    public enum Layer { Back, Middle, Front };
    public Layer moveTo = Layer.Back;

    // Use this for initialization
    void Start()
    {
        Transform parent = transform.parent;
        UIScreen scrn = parent.gameObject.GetComponent<UIScreen>();
        while (!scrn && parent.parent)
        {
            parent = parent.parent;
            scrn = parent.gameObject.GetComponent<UIScreen>();
        }

        screen = scrn;

        switch(moveTo)
        {
            case Layer.Back:
                transform.SetParent(screen.back);
                break;
            case Layer.Middle:
                transform.SetParent(screen.mid);
                break;
            case Layer.Front:
                transform.SetParent(screen.front);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
