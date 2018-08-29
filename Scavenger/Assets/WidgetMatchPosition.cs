using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetMatchPosition : MonoBehaviour
{
    public RectTransform target;
    public Vector3 offset;
    public RectTransform self;

    // Use this for initialization
    void Start()
    {
        Canvas.willRenderCanvases += CanvasUpdate;

        self = transform as RectTransform;

        transform.parent = UIManager.active.canvas.transform;
        self.position = target.position;
    }

    void CanvasUpdate()
    {
        self.position = target.position + offset.Multiply(target.localScale);
    }

    private void OnDestroy()
    {
        Canvas.willRenderCanvases -= CanvasUpdate;
    }
}
