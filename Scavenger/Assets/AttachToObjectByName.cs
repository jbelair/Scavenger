using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObjectByName : MonoBehaviour
{
    public string objectName = "";
    public GameObject target;

    // Use this for initialization
    void Awake()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (target == null)
        {
            target = GameObject.Find(objectName);
            yield return new WaitForSeconds(0.1f);
        }

        transform.SetParent(target.transform);

        yield return null;
    }
}
