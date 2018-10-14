using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linkable : MonoBehaviour
{
    public GameObject link;

    public void Link(GameObject link)
    {
        this.link = link;
    }

    public static implicit operator GameObject(Linkable link)
    {
        return link.link;
    }
}
