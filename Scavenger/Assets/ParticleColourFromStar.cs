using System;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleColourFromStar : MonoBehaviour
{
    [Serializable]
    public class ParticleSystemContainer
    {
        public ParticleSystem system;
        [Range(1, 10)]
        public float kelvinRange = 2;
        public bool invert = false;
        //[Range(1, 50)]
        //public float sizeRange = 10;
    }

    public GameObject star;
    public Material material;
    public ParticleSystemContainer[] particleSystems;
    public Texture2D texture;

    // Use this for initialization
    void Start()
    {
        if (!material)
        {
            material = star.GetComponent<MeshRenderer>().sharedMaterial;
            texture = (material.GetTexture("_Emissive") as Texture2D);
        }
    }

    public float lastKelvin = 0;
    public float lastKelvinRange = 0;
    public float lastKelvinMax = 0;
    // Update is called once per frame
    void Update()
    {
        float kelvin = material.GetFloat("_Kelvin");
        float kelvinRange = material.GetFloat("_KelvinRange");
        float kelvinMax = material.GetFloat("_KelvinMax");

        if (lastKelvin != kelvin || lastKelvinRange != kelvinRange || lastKelvinMax != kelvinMax)
        {
            lastKelvin = kelvin;
            lastKelvinRange = kelvinRange;
            lastKelvinMax = kelvinMax;

            foreach (ParticleSystemContainer container in particleSystems)
            {
                Color colourHigh = texture.GetPixelBilinear(((kelvin + kelvinRange * container.kelvinRange) / kelvinMax), 0);
                Color colourMedium = texture.GetPixelBilinear(((kelvin + kelvinRange) / kelvinMax), 0);
                Color colourLow = texture.GetPixelBilinear(((kelvin / container.kelvinRange) / kelvinMax), 0);

                //float sizeMin = kelvin / kelvinMax;
                //float sizeMax = sizeMin * container.sizeRange;

                //ParticleSystem.MainModule main = container.system.main;
                //main.startSize = new ParticleSystem.MinMaxCurve(sizeMin, sizeMax);

                ParticleSystem.ColorOverLifetimeModule colour = container.system.colorOverLifetime;

                GradientColorKey[] colourKeys;
                GradientAlphaKey[] alphaKeys;

                if (container.invert)
                {
                    colourKeys = new GradientColorKey[]
                    {

                        new GradientColorKey((colourLow * 8).Normalize(), 1f),
                        new GradientColorKey((colourMedium * 8).Normalize(), 0.75f),
                        new GradientColorKey((colourHigh * 8).Normalize(), 0)
                    };

                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0, 0),
                        new GradientAlphaKey(Mathf.Pow((kelvin + kelvinRange) / kelvinMax, 0.75f), 1)
                    };
                }
                else
                {
                    colourKeys = new GradientColorKey[]
                    {
                        new GradientColorKey((colourHigh * 8).Normalize(), 0),
                        new GradientColorKey((colourMedium * 8).Normalize(), 0.75f),
                        new GradientColorKey((colourLow * 8).Normalize(), 1f)
                    };

                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(Mathf.Pow((kelvin + kelvinRange) / kelvinMax, 0.75f), 0),
                        new GradientAlphaKey(0, 1)
                    };
                }

                colour.color = new ParticleSystem.MinMaxGradient(new Gradient()
                {
                    colorKeys = colourKeys,
                    alphaKeys = alphaKeys
                });
            }
        }
    }
}
