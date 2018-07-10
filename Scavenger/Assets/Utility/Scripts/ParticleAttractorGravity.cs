using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractorGravity : MonoBehaviour
{
    public float G = EnvironmentRules.G;
    public float mass = 1;
    public float maximumDistance;
    public ParticleSystem[] particleSystems;

    // Use this for initialization
    void Start()
    {
        //float distance = 1;
        //float g = (EnvironmentRules.G * mass * EnvironmentRules.PlanetMassRatio) / (distance * distance);
        //while (g > 0.001f && distance < 1000)
        //{
        //    distance += EnvironmentRules.PlanetDistanceRatio;
        //}
        //maximumDistance = distance;
    }

    // Update is called once per frame
    public float averageForce = 0;
    void Update()
    {
        averageForce = 0;
        int particleCount = 0;
        float g = G * mass;
        foreach (ParticleSystem system in particleSystems)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[system.particleCount];
            system.GetParticles(particles);
            particleCount += system.particleCount;
            for(int i = 0; i < particles.Length; i++)
            {
                Vector3 delta = transform.position - particles[i].position;
                float distance = delta.magnitude * EnvironmentRules.PlanetDistanceRatio;
                float f = (g * particles[i].GetCurrentSize(system)) / (distance * distance);
                averageForce += f;
                particles[i].velocity += f * delta.normalized * Time.deltaTime;
            }
            system.SetParticles(particles, particles.Length);
        }
        averageForce /= particleCount;
    }
}
