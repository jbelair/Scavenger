using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;
    //public LineRendererCircle hot;
    //public LineRendererCircle warm;
    //public LineRendererCircle cold;

    // Use this for initialization
    void Start()
    {
        if (!star)
        {
            star = GetComponent<MeshRenderer>().sharedMaterial = new Material(starMaterials[Random.Range(0, starMaterials.Length)]);
        }

        star.SetFloat("_Kelvin", environment[name + " Kelvin"]);
        star.SetFloat("_KelvinRange", environment[name + " Kelvin Range"]);
        transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) * 2 + 1);

        float starKelvin = environment[name + " Kelvin"];
        float logKelvin = Mathf.Log10(starKelvin);

        SystemGenerator generator = environment.GetComponent<SystemGenerator>();

        // Goldilocks zone starts at 5 Celsius and goes to 40 Celsius
        // Hot is before that
        // And cold is after that

        float kelvin = 0;
        float distanceGoldilocksStart = 0;
        while (kelvin < 278f)
        {
            kelvin = generator.planetPlotKelvin.Evaluate(distanceGoldilocksStart / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
            distanceGoldilocksStart += 50;
        }
        float distanceGoldilocksEnds = distanceGoldilocksStart;
        while (kelvin < 313f)
        {
            kelvin = generator.planetPlotKelvin.Evaluate(distanceGoldilocksEnds / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
            distanceGoldilocksEnds += 50;
        }
        float distanceColdEnds = distanceGoldilocksEnds;
        while (kelvin > 0)
        {
            kelvin = generator.planetPlotKelvin.Evaluate(distanceColdEnds / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
            distanceColdEnds += 100;
        }

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
