using System;
using System.Collections.Generic;

[Serializable]
public struct ShipDefinition
{
    [Serializable]
    public struct Statistic
    {
        public string name;
        public float value;
        public string unit;
    }

    public string name;
    public string description;
    public string model;
    public string skin;
    public string shield;
    public string risk;
    public int oneIn;
    public int value;
    public bool starting;
    public List<Statistic> statistics;
}
