using System.Collections.Generic;
using UnityEngine;

public class CameraNode : MonoBehaviour
{
    public static List<CameraNode> nodes = new List<CameraNode>();

    public bool deparent = true;
    public bool defaultNode = false;

    public void Start()
    {
        nodes.Add(this);

        if (deparent)
        {
            transform.SetParent(null, true);
        }

        if (defaultNode)
        {
            Camera.main.GetComponent<MoveTo>().TransitionFrame(transform);
        }
    }

    public void OnDestroy()
    {
        nodes.Remove(this);
    }
}
