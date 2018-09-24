using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMProNameToText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject target;

    public bool polling = true;
    public float pollingUpdate = 1f;

    // Use this for initialization
    void Start()
    {
        text.text = target.name;
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (polling)
        {
            text.text = target.name;
            yield return new WaitForSeconds(pollingUpdate);
        }
    }
}
