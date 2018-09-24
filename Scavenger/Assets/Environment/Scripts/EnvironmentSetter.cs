using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnvironmentSetter : MonoBehaviour
{
    public bool generateStars = true;
    public bool populateStars = true;
    public bool generatePlanets = true;
    public bool populatePlanets = true;
    public bool generateMoons = true;
    public bool populateMoons = true;
    public bool generateDungeons = true;
    public bool populateDungeons = true;

    public void Awake()
    {
        Environment.generateStars = generateStars;
        Environment.populateStars = populateStars;
        Environment.generatePlanets = generatePlanets;
        Environment.populatePlanets = populatePlanets;
        Environment.generateMoons = generateMoons;
        Environment.populateMoons = populateMoons;
        Environment.generateDungeons = generateDungeons;
        Environment.populateDungeons = populateDungeons;
    }
}
