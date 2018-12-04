using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnvironmentBasedAsteroidCloud : MonoBehaviour
{
    public Statistics environment;
    public ParticleSystem asteroids;
    public ParticleSystem dust;

    public float innerRadius = 1;
    public float outerRadius = 0;

    public void Start()
    {
        if (name == "Oort Cloud")
        {
            innerRadius = environment["Oort Cloud Inner Radius"];
            outerRadius = environment["Oort Cloud Outer Radius"];
            int numberOfAsteroids = environment["Metallicity"];

            transform.localScale = Vector3.one * outerRadius;

            asteroids.Stop();
            dust.Stop();
            asteroids.Clear();
            dust.Clear();

            ParticleSystem.ShapeModule asteroidsShape = asteroids.shape;
            ParticleSystem.ShapeModule dustShape = dust.shape;
            asteroidsShape.radiusThickness = dustShape.radiusThickness = innerRadius;

            ParticleSystem.MainModule asteroidsMain = asteroids.main;
            ParticleSystem.MainModule dustMain = dust.main;
            asteroids.randomSeed = (uint)environment["System Hash"].Get<int>();
            asteroidsMain.maxParticles = Mathf.Max(25, 50 * numberOfAsteroids);
            dust.randomSeed = (uint)environment["System Hash"].Get<int>();
            dustMain.maxParticles = Mathf.Max(1, 2 * numberOfAsteroids);
            //ParticleSystem.EmissionModule dustEmission = dust.emission;
            //dustEmission.rateOverTime = new ParticleSystem.MinMaxCurve(numberOfAsteroids / 10f, numberOfAsteroids);

            //asteroids.Play();
            //dust.Play();

            asteroids.Simulate(asteroidsMain.duration);
            dust.Simulate(dustMain.duration);
        }
    }
}
