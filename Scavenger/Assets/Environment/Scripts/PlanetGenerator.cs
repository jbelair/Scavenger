using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PlanetProfile
    {
        public Material[] surfaces;
        public Material[] clouds;
        public Material[] atmospheres;
    }

    [Header("Metallic Planets")]
    public PlanetProfile[] coldMetallicProfiles;
    public PlanetProfile[] warmMetallicProfiles;
    public PlanetProfile[] hotMetallicProfiles;

    [Header("Mixed Planets")]
    public PlanetProfile[] coldMixedProfiles;
    public PlanetProfile[] warmMixedProfiles;
    public PlanetProfile[] hotMixedProfiles;

    [Header("Gasseous Planets")]
    public PlanetProfile[] coldGasseousProfiles;
    public PlanetProfile[] warmGasseousProfiles;
    public PlanetProfile[] hotGasseousProfiles;

    [Header("Special Alternatives")]
    public GameObject[] asteroidBelts;
    public GameObject[] artefacts;
    public GameObject[] gasBelts;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GeneratePlanet(EnvironmentBasedPlanet planet)
    {
        // Given the parameters for this planet we need to decide if it is
        // A.   Metallic
        // B.   Mixed
        // C.   Gasseous
    }
}
