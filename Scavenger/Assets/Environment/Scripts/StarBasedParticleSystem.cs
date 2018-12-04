using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class StarBasedParticleSystem : MonoBehaviour
{
    public ParticleSystem particles;
    new public EnvironmentBasedStar.ParticleSystemContainer particleSystem;

    public void Start()
    {
        particles = GetComponent<ParticleSystem>();

        particles.Clear();

        StartCoroutine(Poll());
        //GetComponentInParent<EnvironmentBasedStar>().particleSystems.Add(particleSystem);
    }

    IEnumerator Poll()
    {
        EnvironmentBasedStar star = GetComponentInParent<EnvironmentBasedStar>();

        while (star == null)
        {
            yield return new WaitForSeconds(0.1f);
            star = GetComponentInParent<EnvironmentBasedStar>();
        }

        if (!particleSystem.system)
            particleSystem.system = particles;

        star.particleSystems.Add(particleSystem);

        yield return null;
    }
}
