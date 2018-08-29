using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetWorldPosition : MonoBehaviour
{
    public static List<WidgetWorldPosition> worldPositions = new List<WidgetWorldPosition>();

    public Transform target;
    public RectTransform canvas;
    public RectTransform rectTransform;
    public bool pushAwayFromOthers = true;
    public bool preventOverlap = false;
    public float distance = 128f;
    public float speed = 0.1f;
    public float screenEdge = 128f;
    public Vector2 position;
    public Vector2 positionVelocity;

    // Use this for initialization
    void Start()
    {
        rectTransform = transform as RectTransform;
        Canvas.willRenderCanvases += CanvasUpdate;

        worldPositions.Add(this);

        if (canvas != null && target != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(target.position);

            if (pos.z >= 0)
            {
                foreach (WidgetWorldPosition other in worldPositions)
                {
                    if (other == this)
                        continue;

                    if (pushAwayFromOthers || (preventOverlap && other.rectTransform.RectOverlaps(rectTransform)))
                    {
                        Vector3 delta = rectTransform.position.XY() - other.rectTransform.position.XY();
                        pos = pos + delta.normalized * distance;
                    }
                }
            }

            transform.position = pos.XY();
        }
    }

    // Update is called once per frame
    void CanvasUpdate()
    {
        if (canvas != null && target != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(target.position);

            foreach (WidgetWorldPosition other in worldPositions)
            {
                if (other == this)
                    continue;

                if (pushAwayFromOthers || (preventOverlap && other.rectTransform.RectOverlaps(rectTransform)))
                {
                    Vector3 delta = rectTransform.position.XY() - other.rectTransform.position.XY();
                    pos = pos + delta.normalized * distance;
                }

                pos = new Vector3(Mathf.Clamp(pos.x, screenEdge, Screen.width - screenEdge), Mathf.Clamp(pos.y, screenEdge, Screen.height - screenEdge), 0);
            }

            position = pos.XY();
        }

        transform.position = Vector2.SmoothDamp(transform.position, position, ref positionVelocity, speed);//, 1000f, Time.deltaTime);
    }

    private void OnDestroy()
    {
        Canvas.willRenderCanvases -= CanvasUpdate;
        worldPositions.Remove(this);
    }
}