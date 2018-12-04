using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetMatchPosition : MonoBehaviour
{
    public RectTransform target;
    public Vector3 offset;
    public RectTransform self;
    public bool deparent = true;

    // Use this for initialization
    void Start()
    {
        Canvas.willRenderCanvases += CanvasUpdate;

        self = transform as RectTransform;

        if (deparent)
            transform.SetParent(UIManager.active.canvas.transform);

        self.position = target.position;
    }

    void CanvasUpdate()
    {
        if (target)
            self.position = target.position + offset.Multiply(target.localScale);
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Canvas.willRenderCanvases -= CanvasUpdate;
    }
}
