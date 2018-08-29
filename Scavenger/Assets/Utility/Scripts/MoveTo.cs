using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [System.Serializable]
    public class AnimationKey
    {
        public Transform from;
        public Vector3 fromPoint;
        public Quaternion fromRot;
        public Transform to;
        public Vector3 toPoint;
        public Quaternion toRot;
        public float duration;
        public float durationCurrent;

        public Vector3 refVelocity;

        public void Update(Transform transform)
        {
            float t = durationCurrent / duration;
            //float q = Mathf.Pow(t, 5);

            if (from != null && to != null)
            {
                transform.position = Vector3.Lerp(from.position, to.position, t);
                transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);
            }
            else if (from == null)
            {
                transform.position = Vector3.Lerp(fromPoint, to.position, t);
                transform.rotation = Quaternion.Slerp(fromRot, to.rotation, t);
            }
            else if (to == null)
            {
                transform.position = Vector3.Lerp(from.position, toPoint, t);
                transform.rotation = Quaternion.Slerp(from.rotation, toRot, t);
            }
            else
            {
                transform.position = Vector3.Lerp(fromPoint, toPoint, t);
                transform.rotation = Quaternion.Slerp(fromRot, toRot, t);
            }

            durationCurrent += Time.deltaTime;
        }

        public bool Ended()
        {
            return durationCurrent >= duration;
        }
    }

    public Transform restNode;
    public List<AnimationKey> frames = new List<AnimationKey>();
    private AnimationKey currentFrame;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentFrame != null)
        {
            currentFrame.Update(transform);

            if (currentFrame.Ended())
            {
                if (frames.Count > 1)
                {
                    frames.RemoveAt(0);
                    if (frames.Count > 0)
                        currentFrame = frames[0];
                }
            }
        }
        else
        {
            if (frames.Count > 0)
                currentFrame = frames[0];
        }
    }

    public void TransitionFrame(Transform to)
    {
        if (frames.Count > 0)
        {
            AnimationKey lastFrame = frames[frames.Count - 1];
            frames.Add(new AnimationKey { from = lastFrame.to, to = to, duration = 1f });
        }
        else
        {
            frames.Add(new AnimationKey { fromPoint = Camera.main.transform.position, fromRot = Camera.main.transform.rotation, to = to, duration = 1f });
        }
    }

    public void TransitionFrame(Transform to, float duration)
    {
        if (frames.Count > 0)
        {
            AnimationKey lastFrame = frames[frames.Count - 1];
            frames.Add(new AnimationKey { from = lastFrame.to, to = to, duration = duration });
        }
        else
        {
            frames.Add(new AnimationKey { fromPoint = Camera.main.transform.position, fromRot = Camera.main.transform.rotation, to = to, duration = duration });
        }
    }

    public void AddFrame(Transform to, float duration, bool returnToRestStateFirst, float returnDuration)
    {
        if (frames.Count > 0)
        {
            AnimationKey lastFrame = frames[frames.Count - 1];
            if (returnToRestStateFirst)
            {
                frames.Add(new AnimationKey { from = lastFrame.to, to = restNode, duration = returnDuration });
                frames.Add(new AnimationKey { from = restNode, to = to, duration = duration });
            }
            else
            {
                frames.Add(new AnimationKey { from = lastFrame.to, to = to, duration = duration });
            }
        }
        else
        {
            frames.Add(new AnimationKey { fromPoint = Camera.main.transform.position, fromRot = Camera.main.transform.rotation, to = to, duration = duration });
        }
    }
}
