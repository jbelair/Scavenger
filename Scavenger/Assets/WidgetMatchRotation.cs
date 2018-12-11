using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetMatchRotation : MonoBehaviour
{
    new public RectTransform transform;
    public Vector3 eulerAngles;
    public EntityRef entity;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(entity.Entity.transform.rotation.eulerAngles.Multiply(new Vector3(1, -1, 1)) + eulerAngles);
    }
}
