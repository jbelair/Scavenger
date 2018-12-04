using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Environment
{
    public static int environmentTime = 0;
    public static float environmentTimeSpeed = 1 / 30f;
    public static float environmentTimeCounter = 0;

    public static float maximumDungeons = 5;
    private static float _scanRadius = -1;
    public static float ScanRadius
    {
        get
        {
            if (_scanRadius < 0)
                _scanRadius = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value).statistics.Find(stat => stat.name == "stat_jump_view").value;

            return _scanRadius;
        }
    }
    private static float _jumpRadius = -1;
    public static float JumpRadius
    {
        get
        {
            if (_jumpRadius < 0)
                _jumpRadius = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value).statistics.Find(stat => stat.name == "stat_jump_range").value;

            return _jumpRadius;
        }
    }
    private static float _jumpFuel = -1;
    public static float JumpFuel
    {
        get
        {
            _jumpFuel = float.Parse(PlayerSave.Active.Get("fuel").value);

            if (_jumpFuel < 0)
                _jumpFuel = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value).statistics.Find(stat => stat.name == "stat_jump_fuel").value;
            PlayerSave.Active.Add("fuel", _jumpFuel.ToString());

            return _jumpFuel;
        }
        set
        {
            _jumpFuel = value;
            PlayerSave.Active.Add("fuel", _jumpFuel.ToString());
        }
    }
    private static float _jumpFuelMax = -1;
    public static float JumpFuelMax
    {
        get
        {
            if (_jumpFuelMax < 0)
                _jumpFuelMax = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value).statistics.Find(stat => stat.name == "stat_jump_fuel").value;

            return _jumpFuelMax;
        }
    }
    public static float jumpDistance = 0;
    private static Vector3 _systemCoordinates = Vector3.positiveInfinity;
    public static Vector3 SystemCoordinates
    {
        get
        {
            if (_systemCoordinates.x == float.PositiveInfinity)
                _systemCoordinates = JsonUtility.FromJson<Vector3>(PlayerSave.Active.Get("system coordinates").value);
            PlayerSave.Active.Add("system coordinates", JsonUtility.ToJson(_systemCoordinates));

            return _systemCoordinates;
        }
        set
        {
            _systemCoordinates = value;
            PlayerSave.Active.Add("system coordinates", JsonUtility.ToJson(_systemCoordinates));
        }
    }
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
