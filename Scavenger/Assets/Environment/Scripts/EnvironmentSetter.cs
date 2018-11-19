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

    void Awake()
    {
        Set();

        Environment.scanRadius = Players.players[0].statistics["stat_jump_view"].Get<float>();
        Environment.jumpFuel = Players.players[0].statistics["stat_jump_fuel_cur"].Get<float>();
        Environment.jumpFuelMax = Players.players[0].statistics["stat_jump_fuel"].Get<float>();
        Environment.jumpRadius = Players.players[0].statistics["stat_jump_range"].Get<float>();
    }

    void Start()
    {
        Set();
    }

    public void Set()
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
