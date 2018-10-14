using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetWorldPosition : MonoBehaviour
{
    public enum Format { Instant, Smooth };
    public static List<WidgetWorldPosition> worldPositions = new List<WidgetWorldPosition>();

    public Format format;
    public Transform target;
    public RectTransform canvas;
    public RectTransform rectTransform;
    public bool pushAwayFromOthers = true;
    public bool preventOverlap = false;
    public float distance = 128f;
    public float speed = 0.1f;
    public Vector4 screenEdge = new Vector4(0, 96f, 0, 0);
    public Vector2 position;
    public Vector2 positionVelocity;

    public bool[] defaultEnables;
    public GameObject[] children;

    public Vector3 pos;
    public Vector3 viewPos;

    // Use this for initialization
    void Start()
    {
        rectTransform = transform as RectTransform;
        Canvas.willRenderCanvases += CanvasUpdate;

        worldPositions.Add(this);

        if (canvas != null && target != null)
        {
            pos = Camera.main.WorldToScreenPoint(target.position).XY();

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

        children = new GameObject[transform.childCount];
        defaultEnables = new bool[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
            defaultEnables[i] = children[i].activeInHierarchy;
        }
    }

    // Update is called once per frame
    void CanvasUpdate()
    {
        if (canvas != null && target != null)
        {
            viewPos = Camera.main.WorldToViewportPoint(target.position);
            pos = Camera.main.WorldToScreenPoint(target.position).XY();

            if (viewPos.z > 0)
            //if (BoundsHelper.PointWithin(Camera.main.WorldToScreenPoint(target.position.XY()), new Vector2(Screen.width, Screen.height)))
            {
                if (children != null && children.Length > 0 && !children[0].activeInHierarchy)
                    EnableChildren(true);

                foreach (WidgetWorldPosition other in worldPositions)
                {
                    if (other == this)
                        continue;

                    if (pushAwayFromOthers && other.pushAwayFromOthers || (preventOverlap && other.rectTransform.RectOverlaps(rectTransform)))
                    {
                        Vector3 delta = rectTransform.position.XY() - other.rectTransform.position.XY();
                        pos = pos + delta.normalized * distance;
                    }

                    if (screenEdge.magnitude > 0)
                        pos = new Vector3(Mathf.Clamp(pos.x, screenEdge.x, Screen.width - screenEdge.z), Mathf.Clamp(pos.y, screenEdge.y, Screen.height - screenEdge.w), 0);
                }

                position = pos.XY();
            }
            else
            {
                EnableChildren(false);
            }
        }

        if (format == Format.Smooth)
            transform.position = Vector2.SmoothDamp(transform.position, position, ref positionVelocity, speed);//, 1000f, Time.deltaTime);
        else if (format == Format.Instant)
            transform.position = position;
    }

    private void OnDestroy()
    {
        Canvas.willRenderCanvases -= CanvasUpdate;
        worldPositions.Remove(this);
    }

    private void EnableChildren(bool enable)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (defaultEnables[i])
                children[i].SetActive(enable);
        }
    }
}