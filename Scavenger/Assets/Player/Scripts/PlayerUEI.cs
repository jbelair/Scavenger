using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class PlayerUEI : MonoBehaviour
{
    public Statistics statistics;
    public List<SkillUEI> unitySkills = new List<SkillUEI>();
    public bool isAlive = false;

    private void Start()
    {
        isAlive = true;

        if (statistics == null)
            statistics = GetComponentInParent<Statistics>();
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        isAlive = false;
    }
}
