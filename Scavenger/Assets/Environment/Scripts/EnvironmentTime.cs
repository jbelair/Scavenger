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
        time = Environment.environmentTime;
        if (minutesToTime > 0)
            Environment.environmentTimeSpeed = Mathf.Min(1, Mathf.Max(0, 1f / (minutesToTime * 60)));
        else
            Environment.environmentTimeSpeed = minutesToTime;
        timeCounter = Environment.environmentTimeCounter;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime * Environment.environmentTimeSpeed;

        if (timeCounter >= 1)
        {
            timeCounter -= 1;

            time++;
        }

        Environment.environmentTimeCounter = timeCounter;
        Environment.environmentTime = time;
    }

    private void OnDestroy()
    {
        if (active == this)
            active = null;
    }
}
