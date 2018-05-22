using UnityEngine;
using System.Collections;

public class LockUIItem : MonoBehaviour
{
    private Vector3 position;
    // Use this for initialization
    void Awake()
    {
        position = gameObject.GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (position != gameObject.GetComponent<RectTransform>().position)
        {
            gameObject.GetComponent<RectTransform>().position = position;
        }
    }
}