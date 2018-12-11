using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetStatisticBarController : MonoBehaviour
{
    public WidgetStatisticBar[] bars;

    private void Awake()
    {
        StartCoroutine(Poll());
    }

    // Update is called once per frame
    IEnumerator Poll()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(0.25f);
            foreach (WidgetStatisticBar bar in bars)
            {
                if (bar.value.Maximum.Get<float>() <= 0)
                {
                    bar.gameObject.SetActive(false);
                    bar.textCurrentMaximum.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    bar.gameObject.SetActive(true);
                    bar.textCurrentMaximum.transform.parent.gameObject.SetActive(true);
                }
            }
        }

        yield return null;
    }
}
