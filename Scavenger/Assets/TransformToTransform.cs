using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransformKey
{
    public float duration;
    public float durationCurrent;
    public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

public class TransformToTransform : MonoBehaviour
{
    public enum Format { Lerp, Slerp, MoveTo };
    public Format format;
    public TransformKey[] transforms;
    public int index = 0;

    // Use this for initialization
    void Start()
    {
        if (transforms.Length > 0)
        {
            transform.position = transforms[0].position;
            transform.rotation = Quaternion.Euler(transforms[0].rotation);
            transform.localScale = transforms[0].scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transforms.Length > 0)
        {
            TransformKey previous = transforms[((index - 1 >= 0) ? index - 1 : 0)];
            TransformKey current = transforms[index];
            float percentage = current.curve.Evaluate(current.durationCurrent / current.duration);

            switch (format)
            {
                case Format.Lerp:
                    transform.position = Vector3.Lerp(previous.position, current.position, percentage);
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(previous.rotation), Quaternion.Euler(current.rotation), percentage);
                    transform.localScale = Vector3.Lerp(previous.scale, current.scale, percentage);
                    break;
                case Format.Slerp:
                    transform.position = Vector3.Slerp(previous.position, current.position, percentage);
                    transform.rotation = Quaternion.Slerp(Quaternion.Euler(previous.rotation), Quaternion.Euler(current.rotation), percentage);
                    transform.localScale = Vector3.Slerp(previous.scale, current.scale, percentage);
                    break;
                case Format.MoveTo:
                    transform.position = Vector3.MoveTowards(previous.position, current.position, (previous.position - current.position).magnitude * 0.1f * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(Quaternion.Euler(previous.rotation), Quaternion.Euler(current.rotation), percentage);
                    transform.localScale = Vector3.Slerp(previous.scale, current.scale, (previous.position - current.position).magnitude * 0.1f * Time.deltaTime);
                    break;
            }
            current.durationCurrent += Time.deltaTime;
            if (current.durationCurrent > current.duration)
            {
                index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                if (index > transforms.Length - 1)
                    index = transforms.Length - 1;
                transforms[index].durationCurrent = current.durationCurrent - current.duration;
            }
        }
    }
}
