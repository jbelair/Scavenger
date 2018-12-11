using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetHazards : MonoBehaviour
{
    public WidgetHazard prefab;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        Hazard[] hazards = null;
        while (hazards == null || hazards.Length < 1)
        {
            hazards = FindObjectsOfType<Hazard>();
            yield return new WaitForSeconds(0.1f);
        }

        foreach (Hazard hazard in hazards)
        {
            WidgetHazard inst = Instantiate(prefab, transform);
            inst.name = hazard.name;
            inst.icon.sprite = Sprites.Get(hazard.name);
            if (inst.text != null)
                inst.text.SetText(Literals.active[hazard.name]);
        }

        yield return new WaitForSeconds(2f);



        yield return null;
    }
}
