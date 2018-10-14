using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Environment
{
    public static string language = Literals.defaultLanguage;
    public static int environmentTime = 0;
    public static float environmentTimeSpeed = 1/30f;
    public static float environmentTimeCounter = 0;

    public static float maximumDungeons = 5;
    public static float scanRadius = 15;
    public static float jumpRadius = 5;
    public static float jumpFuel = 15;
    public static float jumpFuelMax = 15;
    public static float jumpDistance = 0;
    
    public static Vector3 systemCoordinates;
    public static float systemCoordinatesDepth;
    public static Vector3 selectedCoordinates;
    public static GameObject systemOverride;
    public static bool generateStars = true;
    public static bool populateStars = true;
    public static bool generatePlanets = true;
    public static bool populatePlanets = true;
    public static bool generateMoons = true;
    public static bool populateMoons = true;
    public static bool generateDungeons = true;
    public static bool populateDungeons = true;

    public static string sceneName;
    public static AsyncOperation sceneLoading;
}
