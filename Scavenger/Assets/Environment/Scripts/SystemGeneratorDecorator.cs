using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SystemGeneratorDecorator
{
    public string name;

    public enum Trigger { Always, Chance, Faction }
    [Header("Trigger")]
    public Trigger trigger;

    public int oneIn;

    public string factionName;

    [Header("System Generation Callbacks")]
    public UnityEvent system;
    public UnityEvent stars;
    public UnityEvent populateStars;
    public UnityEvent planets;
    public UnityEvent populatePlanets;
    public UnityEvent moons;
    public UnityEvent populateMoons;
    public UnityEvent dungeons;
    public UnityEvent populateDungeons;

    public Statistics statistics;

    public bool Happens()
    {
        switch(trigger)
        {
            case Trigger.Always:
                return true;
            case Trigger.Chance:
                return 1 == Random.Range(1, oneIn);
            case Trigger.Faction:
                // TODO: Implement faction check logic for system decorators
                return false;
            default:
                return false;
        }
    }
}
