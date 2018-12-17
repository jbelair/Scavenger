using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromShip : MonoBehaviour
{
    public Ship target;
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
            if (target && target.definition.name != "")
            {
                transform.localScale = new Vector3(target.definition.statistics.Find(s => s.name == statisticX).value, target.definition.statistics.Find(s => s.name == statisticY).value, target.definition.statistics.Find(s => s.name == statisticZ).value);
            }
        }

        yield return null;
    }
}
