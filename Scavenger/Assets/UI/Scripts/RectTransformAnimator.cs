using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformAnimator : MonoBehaviour
{
    [System.Serializable]
    public class Key
    {
        public enum Format { None, Lerp, Slerp };
        public string name;
        public Format format = Format.None;
        public float duration = 1f;
        public float durationCurrent = 0f;
        public bool positioned = false;
        public Vector3 position;
        public bool rotated = false;
        public Vector3 rotation;
        public bool scaled = false;
        public Vector3 scale = Vector3.one;
        public bool dimensionScaled = false;
        public Vector2Int dimensions;

        public void Update(RectTransform transform, Key previous)
        {
            if (previous != null)
            {
                float t = durationCurrent / duration;

                if (positioned)
                    transform.position = Vector3.Lerp(previous.position, position, t);

                if (rotated)
                    transform.Rotate(Vector3.Lerp(previous.rotation, rotation, t));

                if (scaled)
                    transform.localScale = Vector3.Lerp(previous.scale, scale, t);

                if (dimensionScaled)
                    transform.sizeDelta = Vector2.Lerp(previous.dimensions, dimensions, t);
            }
            else
            {
                if (positioned)
                    transform.position = position;

                if (rotated)
                    transform.Rotate(rotation);

                if (scaled)
                    transform.localScale = scale;

                if (dimensionScaled)
                    transform.sizeDelta = dimensions;
            }
        }
    }
    public RectTransform target;

    public string active = "";
    public string lastActive = "";

    public List<Key> keys = new List<Key>();
    public Key activeKey;
    public Key previousKey;

    // Use this for initialization
    void Start()
    {
        activeKey = keys.Find(key => key.name == active);
        activeKey.Update(target, null);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeKey.durationCurrent < activeKey.duration)
            activeKey.durationCurrent += Time.deltaTime;

        activeKey.Update(target, previousKey);
    }

    public void Animate(string str)
    {
        lastActive = active;
        previousKey = activeKey;
        previousKey.durationCurrent = 0;

        active = str;
        activeKey = keys.Find(key => key.name == active);
        activeKey.durationCurrent = 0;
        activeKey.Update(target, previousKey);
    }
}
