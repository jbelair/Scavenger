using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class ProjectileUEI : MonoBehaviour
{
    public Statistics statistics;

    public void Start()
    {
        if (statistics == null)
            statistics = GetComponentInParent<Statistics>();
    }
}
