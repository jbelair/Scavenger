using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;

    // Use this for initialization
    void Start()
    {
        if (!star)
        {
            star = GetComponent<MeshRenderer>().sharedMaterial = new Material(starMaterials[Random.Range(0, starMaterials.Length)]);
        }

        star.SetFloat("_Kelvin", environment[name + " Kelvin"]);
        star.SetFloat("_KelvinRange", environment[name + " Kelvin Range"]);
        transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) + 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
