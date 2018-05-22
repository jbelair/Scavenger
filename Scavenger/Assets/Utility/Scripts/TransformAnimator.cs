using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransformSequence
{
    public string name;
    public enum Format { Lerp, Slerp, MoveTo };
    public Format format;
    public TransformKey[] transforms;
    public int index = 0;

    // Use this for initialization
    public void Start(Transform transform)
    {
        if (transforms.Length > 0)
        {
            transform.position = transforms[0].position;
            transform.rotation = Quaternion.Euler(transforms[0].rotation);
            transform.localScale = transforms[0].scale;
        }

        foreach(TransformKey key in transforms)
        {
            key.durationCurrent = 0;
        }
    }

    // Update is called once per frame
    public void Update(Transform transform)
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

public class TransformAnimator : MonoBehaviour
{
    [Header("State")]
    public string active = "";
    public string lastActive = "";

    [Header("Animations")]
    public TransformSequence[] transforms;

    public void SetActiveAnimation(string name)
    {
        active = name;
    }
    
    void Start()
    {
        transforms[NameToIndex(active)].Start(transform);

        lastActive = active;
    }

    void Update()
    {
        if (lastActive != active)
            transforms[NameToIndex(active)].Start(transform);
        else
            transforms[NameToIndex(active)].Update(transform);

        lastActive = active;
    }

    int NameToIndex(string name)
    {
        int i = 0;
        foreach (TransformSequence sequence in transforms)
        {
            if (name == sequence.name)
                return i;
            i++;
        }
        return 0;
    }
}
