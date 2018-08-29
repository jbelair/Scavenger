using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnvironmentSetter : MonoBehaviour
{
    public bool generateGOStars = true;
    public bool generateGOPlanets = true;
    public bool generateGOMoons = true;
    public bool generateGODungeons = true;

    public void Awake()
    {
        Environment.generateGOStars = generateGOStars;
        Environment.generateGOPlanets = generateGOPlanets;
        Environment.generateGOMoons = generateGOMoons;
        Environment.generateGODungeons = generateGODungeons;
    }
}
