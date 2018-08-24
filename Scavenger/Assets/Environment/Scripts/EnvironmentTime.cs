using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnvironmentTime : MonoBehaviour
{
    public static EnvironmentTime active;

    public int time = 0;
    public int minutesToTime = 30;
    public float timeCounter = 0;

    private void Awake()
    {
        active = this;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= minutesToTime * 60)
        {
            timeCounter -= minutesToTime * 60;
            time++;
        }
    }

    private void OnDestroy()
    {
        if (active == this)
            active = null;
    }
}
