using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;
    public Texture2D blackbodyRamp;
    public new Light light;

    public string statisticName;

    public float kelvin;
    public float kelvinRange;

    public LineRendererCircle hot;
    public LineRendererCircle warm;
    public LineRendererCircle cold;

    // Use this for initialization
    void Start()
    {
        if (!star)
        {
            star = GetComponent<MeshRenderer>().sharedMaterial = new Material(starMaterials[Random.Range(0, starMaterials.Length)]);
        }

        statisticName = name;

        kelvin = environment[statisticName + " Kelvin"];
        kelvinRange = environment[statisticName + " Kelvin Range"];
        transform.localScale = Vector3.one * environment[statisticName + " Radius"];

        if (!blackbodyRamp)
            blackbodyRamp = (star.GetTexture("_Emissive") as Texture2D);

        light.color = blackbodyRamp.GetPixelBilinear(((kelvin + kelvinRange) / star.GetFloat("_KelvinMax")), 0);
        light.intensity = Mathf.Log10(EnvironmentRules.StellarLuminosity(transform.localScale.x, kelvin + kelvinRange)) / 4f;

        Vector3 position = environment["System Coordinates"].Get<Vector3>();
        if (position.y > 0)
            name = position.x + "+" + position.y;
        else
            name = position.x + "-" + position.y;

        int numberOfStars = environment["Stars"];
        int brightness = numberOfStars - 1;
        for (int i = 0; i < numberOfStars; i++)
        {
            if (light.intensity > Mathf.Log10(EnvironmentRules.StellarLuminosity(environment["Star " + StringHelper.IndexIntToChar(i) + " Radius"], (float)environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] + environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin Range"])) / 4f)
                brightness--;
        }
        name += " " + StringHelper.IndexIntToChar(brightness);

        star.SetFloat("_Kelvin", kelvin);
        star.SetFloat("_KelvinRange", kelvinRange);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
