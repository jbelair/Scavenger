using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOrbit : MonoBehaviour
{
    public float G = EnvironmentRules.G;
    public Transform center;
    public float mass = 1;
    public ParticleSystem system;
    private float lastDeltaTime = 0;
    // Use this for initialization
    void Start()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 delta = center.position - particles[i].position;
            float distance = delta.magnitude;
            float g = G * mass * particles[i].GetCurrentSize(system);
            Vector3 direction = Vector3.Cross(delta.normalized, Vector3.forward);
            particles[i].velocity = direction * Mathf.Sqrt(g / EnvironmentRules.PlanetDistance(distance));
        }
        system.SetParticles(particles, particles.Length);
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(particles);
        bool[] existing = new bool[particles.Length];
        
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].GetCurrentSize(system) < particles[i].startSize)
            {
                Vector3 delta = center.position - particles[i].position;
                float distance = delta.magnitude;
                float g = G * mass * particles[i].startSize;
                Vector3 direction = Vector3.Cross(delta.normalized, Vector3.forward);
                particles[i].velocity = direction * Mathf.Sqrt(g / EnvironmentRules.PlanetDistance(distance));
            }
        }

        system.SetParticles(particles, particles.Length);
    }
}
