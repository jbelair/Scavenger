using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUEI : MonoBehaviour
{

    public enum AimingFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.cmavt98q6mf8
        /// </summary>
        None,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.upq53p0i72c
        /// </summary>
        Turret,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.yu0wln91efpj
        /// </summary>
        Salvo,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.1zamks50esg8
        /// </summary>
        Fixed,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.fohleoj15py5
        /// </summary>
        Target,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.szmxh0a7bxtw
        /// </summary>
        TargetArea
    };
    public enum CooldownFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.foher6jgww86
        /// </summary>
        Standard,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.btansm4nsw6d
        /// </summary>
        Stacking,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.ubz8ua3ggq0l
        /// </summary>
        Magazine
    };

    public string skillName;
    public string description;
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
        foreach(SkillSequence sequence in sequences)
        {
            sequence.Input(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SkillSequence sequence in sequences)
        {
            sequence.Update();
        }
    }

    void OnDestroy()
    {
        foreach (SkillSequence sequence in sequences)
        {
            sequence.EndInput(true);
        }
    }

    public void StartInput(Vector3 input)
    {
        foreach (SkillSequence sequence in sequences)
        {
            sequence.Input();
        }
    }

    public void EndInput(Vector3 input)
    {
        foreach (SkillSequence sequence in sequences)
        {
            sequence.EndInput(false);
        }
    }
}
