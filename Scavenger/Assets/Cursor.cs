using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Statistics statistics;
    public Statistic input;
    public string statisticInput;

    public float deadZone = 0.25f;
    public float speed;
    public Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
            input = statistics[statisticInput];
        else
        {
            Vector3 inputVector = input.Get<Vector3>();
            Vector3 screen = new Vector3(Screen.width / 2, Screen.height / 2);
            if ((inputVector - screen).magnitude >= deadZone * Screen.width)
            {
                transform.position = Vector3.SmoothDamp(transform.position, inputVector, ref velocity, speed);
            }
        }
    }
}
