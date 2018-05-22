using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistTurret : MonoBehaviour
{
    public Statistics player;
    public LineRenderer[] lines;

	// Use this for initialization
	void Start ()
    {
        //lines = this.GetComponentsInChildren<LineRenderer>();
        //foreach (LineRenderer line in lines)
        //{
        //    line.SetPosition(0, Vector3.zero);
            Vector3 aim = transform.InverseTransformDirection(player["Aim Input"].Get<Vector2>());
        //    line.SetPosition(1, aim.normalized * player["Range"].Get<float>());
        //}
        transform.LookAt(transform.position + aim);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //foreach (LineRenderer line in lines)
        //{
        //    line.SetPosition(0, Vector3.zero);
            Vector3 aim = transform.InverseTransformDirection(player["Aim Input"].Get<Vector2>());
        //    line.SetPosition(1, aim.normalized * player["Range"].Get<float>());
        //}
        transform.LookAt(transform.position + aim);
    }
}
