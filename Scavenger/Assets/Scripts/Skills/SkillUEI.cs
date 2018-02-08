using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUEI : MonoBehaviour
{
    public enum InputFormat { Active, Passive, Toggle, ActiveToggle };
    public enum SkillFormat { Building, Looping, Returning, Decaying };
    public enum AimingFormat { None, Turret, Salvo, Fixed, Target, TargetArea };
    public enum CooldownFormat { Standard, Stacking, Magazine };

    public string skillName;
    public string description;
    public InputFormat inputFormat;
    public SkillFormat skillFormat;
    public AimingFormat aimingFormat;
    public CooldownFormat cooldownFormat;
    public float cooldown;
    public float cooldownCurrent;
    public int stacks;
    public int stacksMax;
    public float stackRecovery;
    public List<SkillSequence> sequences;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartInput(Vector3 input)
    {

    }

    public void ContinueInput(Vector3 input)
    {

    }

    public void EndInput(Vector3 input)
    {

    }
}
