using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMaterial : MonoBehaviour
{
    public EntityRef target;

    public MeshRenderer mesh;
    public Ship ship;
    public bool isInitialised = false;

    private void Start()
    {
        StartCoroutine(Poll());
    }

    private void Update()
    {
        if (mesh.sharedMaterial)
            mesh.sharedMaterial.SetVector("_WorldPosition", transform.position);
    }

    IEnumerator Poll()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(0.1f);

            //if (!isInitialised && ship.definition.shield != "")
            //{
            //    isInitialised = true;
                mesh.sharedMaterial = target.Entity.statistics["shield material"].Get<Material>();
            //}

            float percentage = ((MinMaxStatistic)target.Entity.statistics["shield"].Get<object>()).Percentage;
            if (percentage < 0 || float.IsNaN(percentage))
                percentage = 0;

            //mesh.sharedMaterial.SetFloat("_ShellStrength", Mathf.Pow(percentage, 0.5f));
            //mesh.sharedMaterial.SetFloat("_CoreStrength", percentage * 0.25f);
            if (mesh.sharedMaterial)
                mesh.sharedMaterial.SetFloat("_TotalStrength", percentage);
        }
        yield return null;
    }
}
