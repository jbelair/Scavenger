using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;

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

        kelvin = environment[name + " Kelvin"];
        kelvinRange = environment[name + " Kelvin Range"];

        star.SetFloat("_Kelvin", kelvin);
        star.SetFloat("_KelvinRange", kelvinRange);
        //transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) * 4 + 1);
        transform.localScale = Vector3.one * environment[name + " Radius"];

        float starKelvin = environment[name + " Kelvin"];
        float logKelvin = Mathf.Log10(starKelvin);

        // SystemGenerator generator = environment.GetComponent<SystemGenerator>();

        // Goldilocks zone starts at 5 Celsius and goes to 40 Celsius
        // Hot is before that
        // And cold is after that

        //float kelvin = 0;
        //float distanceGoldilocksStart = 0;
        //kelvin = generator.planetPlotKelvin.Evaluate(distanceGoldilocksStart / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
        //while (kelvin > 313f && distanceGoldilocksStart < 10000)
        //{
        //    kelvin = generator.planetPlotKelvin.Evaluate(distanceGoldilocksStart / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
        //    distanceGoldilocksStart += 50;
        //}
        //float distanceGoldilocksEnds = distanceGoldilocksStart;
        //while (kelvin > 278f && distanceGoldilocksEnds < 100000)
        //{
        //    kelvin = generator.planetPlotKelvin.Evaluate(distanceGoldilocksEnds / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
        //    distanceGoldilocksEnds += 50;
        //}
        //float distanceColdEnds = distanceGoldilocksEnds;
        //while (kelvin > 0 && distanceColdEnds < 1000000)
        //{
        //    kelvin = generator.planetPlotKelvin.Evaluate(distanceColdEnds / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
        //    distanceColdEnds += 100;
        //}

        //AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1f), new Keyframe(1f, 1f) });
        //hot.line.startWidth = hot.line.endWidth = distanceGoldilocksStart;
        //hot.line.widthCurve = curve;
        //hot.radius = hot.line.startWidth / 2f;
        //warm.line.startWidth = warm.line.endWidth = distanceGoldilocksEnds - distanceGoldilocksStart;
        //warm.line.widthCurve = curve;
        //warm.radius = warm.line.startWidth / 2f + hot.line.startWidth;
        //cold.line.startWidth = cold.line.endWidth = distanceColdEnds - distanceGoldilocksEnds;
        //cold.line.widthCurve = curve;
        //cold.radius = cold.line.startWidth / 2f + warm.line.startWidth + hot.line.startWidth;

        //hot.SetCircle();
        //warm.SetCircle();
        //cold.SetCircle();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
