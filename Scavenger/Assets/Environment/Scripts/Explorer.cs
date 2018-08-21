using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public struct Context
    {
        public Transform transform;
        public string keyword;
    }

    public List<string> keywords = new List<string>() { "star", "event", "anomaly", "planet", "fighter", "cruiser", "destroyer", "satellite", "asteroid", "asteroid belt", "ruin", "asteroid cloud", "planetoid", "singularity", "wormhole" };

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
