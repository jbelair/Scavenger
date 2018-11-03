using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMProMatchScheme : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string scheme;

    public bool polling = true;
    public float pollingUpdate = 0.2f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while(polling)
        {
            string newScheme = StringHelper.SchemeParse(text.text);
            if (scheme != newScheme)
            {
                scheme = newScheme;
                text.color = Schemes.Scheme(scheme).colour;
            }
            yield return new WaitForSeconds(pollingUpdate);
        }
    }
}
