using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SkillSequence
{
    public enum SequenceFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.r0i3onhqfoaz
        /// </summary>
        Building,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.dzo2l5kd0bop
        /// </summary>
        Looping,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.8bp8jubk33yv
        /// </summary>
        Returning,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.x4tj6nqiusdp
        /// </summary>
        Decaying
    };
    public enum InputFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.j7v6xfvt7six
        /// Active sequences are sequences that receive input at all times the skill is receiving input.
        /// </summary>
        Active,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.kkycqbo99o4f
        /// Passive sequences are sequences that receive input at all times that the player is alive.
        /// </summary>
        Passive,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.j3yf4nkxzphp
        /// ToggleOn sequences are sequences that receive input at all times that the skill is toggled on.
        /// </summary>
        ToggleOn,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.j3yf4nkxzphp
        /// ToggleOff sequences are sequences that receive input at all times that the skill is toggled off.
        /// </summary>
        ToggleOff
    };

    [Tooltip("Defines the name for this skill sequence, this is purely for ease of use.")]
    public string name;
    [Header("Skill"),Tooltip("This is the skill that owns this sequence, and can either be connected in editor, or left to runtime, where skill will automatically do it if it hasn't been done already.")]
    public SkillUEI skill;
    [Header("Format"),Tooltip("This is the input format this sequence will respond to, and defines how this skill becomes activated.")]
    public InputFormat inputFormat;
    [Tooltip("This is the number of times this sequence has been activated, and dicates what skill sequence keys will activate, by their interval number.")]
    public int inputs = 0;
    [Tooltip("This is the format in which this sequence progresses across its duration")]
    public SequenceFormat sequenceFormat;
    [Header("Sequence"),Tooltip("This is the list of keys the sequence evaluates across.")]
    public List<SkillSequenceKey> keys = new List<SkillSequenceKey>();
    [Header("References"),Tooltip("This is the list of all instantiated objects this sequence is responsible for, in the event of Instant, SequentialCharge, GuaranteedCharge, and Delay.")]
    public List<GameObject> instantiatedObjects = new List<GameObject>();
    [Header("State"),Tooltip("This defines if this sequence is currently active or not, though not what behaviour activation performs.")]
    public bool isActivated;

    /// <summary>
    /// Called each frame by the skill managing this sequence, Update() determines whether this sequence should do anything each frame or not.
    /// </summary>
    public void Update()
    {
    
    }

    /// <summary>
    /// Called whenever the skill managing this sequence receives input.
    /// </summary>
    /// <param name="passive">If this is true, this is a passive input.</param>
    public void Input(bool passive = false)
    {
        if (passive)
        {
            if (inputFormat == InputFormat.Passive)
            {
                isActivated = true;
            }
        }
        else
        {
            if (inputFormat == InputFormat.Active)
            {

            }
            else if (inputFormat == InputFormat.ToggleOn || inputFormat == InputFormat.ToggleOff)
            {

            }
        }
    }

    /// <summary>
    /// Called whenever the skill managing this sequence stops receiving input.
    /// Either from death, or ended player input.
    /// </summary>
    /// <param name="isDead">If this is true, the player has died, and all sequences should respect that.</param>
    public void EndInput(bool isDead = false)
    {
        if (inputFormat == InputFormat.Active)
        {

        }
        else if (inputFormat == InputFormat.Passive || inputFormat == InputFormat.ToggleOn || inputFormat == InputFormat.ToggleOff)
        {

        }
    }

    public void Evaluate()
    {

    }
}
