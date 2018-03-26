using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistUEI : MonoBehaviour
{
    public Statistics player;
    public SkillUEI.AimingFormat format;
    public GameObject[] profiles;

    // Use this for initialization
    void Start()
    {
        int i = 0;
        foreach (GameObject profile in profiles)
        {
            profile.SetActive(i == (int)format);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (format)
        {
            case SkillUEI.AimingFormat.None:
                //transform.position = Vector3.zero;
                break;
            case SkillUEI.AimingFormat.Turret:
                //Vector2 input = player["Aim Input"].Get<Vector2>();
                //transform.position = input;
                //transform.position =
                break;
        }

        int i = 0;
        foreach (GameObject profile in profiles)
        {
            profile.SetActive(i == (int)format);
            i++;
        }

        //transform.position = player["Aim Input"]
    }
}
