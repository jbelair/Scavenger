using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromStatistics : MonoBehaviour
{
    public EntityRef target;
    public string statisticX;
    public string statisticY;
    public string statisticZ;

    private void Awake()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(0.1f);
            if (target && target.Entity)
            {
                transform.localScale = new Vector3(target.Entity.statistics[statisticX], target.Entity.statistics[statisticY], target.Entity.statistics[statisticZ]);
            }
        }

        yield return null;
    }
}
