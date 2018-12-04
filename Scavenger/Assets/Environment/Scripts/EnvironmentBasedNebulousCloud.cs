using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasedNebulousCloud : MonoBehaviour
{
    public Statistics environment;
    public ParticleSystem[] nebulae;

    // Use this for initialization
    void Start()
    {
        transform.localScale = Vector3.one * SystemsGenerator.active.scale;
        float density = environment[name + " Density"];
        Texture2D map = Maps.Map("nebulae");
        Vector3Int coordinates = (environment["System Coordinates"].Get<Vector3>() + new Vector3(map.width / 2f - 1, map.height / 2f - 1, 0)).ToInt();
        Color mapSample = map.GetPixel(coordinates.x, coordinates.y);

        int nebulaMaxParticles = Mathf.RoundToInt(mapSample.Vector3().Greatest() * density);
        uint i = 0;
        foreach (ParticleSystem nebula in nebulae)
        {
            nebula.Stop();
            nebula.Clear();
            ParticleSystem.MainModule nebulaMain = nebula.main;
            nebulaMain.startColor = new ParticleSystem.MinMaxGradient(mapSample);
            nebula.randomSeed = (uint)environment["System Hash"].Get<int>() + i++;
            nebulaMain.maxParticles = nebulaMaxParticles;
            nebula.Simulate(nebulaMain.duration);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
