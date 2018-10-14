using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLinkedObject : MonoBehaviour
{
    //public enum Type { RectTransform, Transform };
    //public Type from;
    //public Transform fromT;
    //public Type to;
    public Transform toT;

    public Linkable link;

    // Use this for initialization
    void Start()
    {
        Linkable inst = Instantiate(link, toT);
        inst.Link(gameObject);
    }
}
