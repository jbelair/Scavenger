using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMaterialFromShip : MonoBehaviour
{
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

            if (!isInitialised && ship.definition.shield != "")
            {
                isInitialised = true;
                mesh.sharedMaterial = new Material(Materials.materials[Skins.Get(ship.definition.shield).skin]);
            }
            else if (!isInitialised)
            {
                isInitialised = true;
                mesh.sharedMaterial = new Material(Materials.materials[Skins.Get(Players.players[0].statistics["shield skin"].Get<string>()).skin]);
            }

            if (mesh.sharedMaterial)
            {
                float percentage = ship.definition.statistics.Find(s => s.name == "stat_shield").value / ship.definition.statistics.Find(s => s.name == "stat_shield_max").value;
                if (percentage < 0 || float.IsNaN(percentage))
                    percentage = 0;

                //mesh.sharedMaterial.SetFloat("_ShellStrength", Mathf.Pow(percentage, 0.5f));
                //mesh.sharedMaterial.SetFloat("_CoreStrength", percentage * 0.25f);
                mesh.sharedMaterial.SetFloat("_TotalStrength", percentage);
            }
        }
        yield return null;
    }
}
