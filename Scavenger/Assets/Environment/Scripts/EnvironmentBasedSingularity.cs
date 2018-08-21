using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasedSingularity : MonoBehaviour
{
    public Statistics environment;
    public GravityLenseSequencer gravityLense;
    public Transform accretionDisk;
    public Transform relativisticJets;
    public new Light light;
    public ParticleAttractorGravity particleAttractor;

    public string statisticName;

    public float radius;
    public float accretion;

    public void Start()
    {
        string star = "Star " + StringHelper.IndexIntToChar(environment["Singularity Star"].Get<int>());

        statisticName = name;

        radius = environment[star + " Radius"];
        accretion = environment[statisticName + " Accretion"];

        Vector3 position = environment["System Coordinates"].Get<Vector3>();
        if (position.y > 0)
            name = position.x + "+" + position.y + "X";
        else
            name = position.x + "-" + position.y + "X";

        gravityLense.sequence[0].radiusStart = gravityLense.sequence[0].radiusEnd = radius * 2f;
        particleAttractor.mass = gravityLense.sequence[0].eventHorizonStart = gravityLense.sequence[0].eventHorizonEnd = radius * 10f;
        gravityLense.effect.occluded = true;
        light.intensity = accretion;
        transform.localScale = Vector3.one * radius;
        accretionDisk.localScale = Vector3.one * accretion;
        if (accretionDisk.localScale.x == 0)
            accretionDisk.gameObject.SetActive(false);
        relativisticJets.localScale = Vector3.one * accretion;
        if (relativisticJets.localScale.x == 0)
            relativisticJets.gameObject.SetActive(false);
    }

    public void Update()
    {
        
    }
}
