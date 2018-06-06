using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SkillSequenceKey
{
    public enum SequenceFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.hmocwil8njgm
        /// Instant sequence keys activate only once, when they stop receiving input from their skill.
        /// Instant sequence keys show aiming assists so long as they are receiving input.
        /// Instant sequence keys increase their duration as they continue to receive input.
        /// Activation instantiates all game objects on the sequence key that the sequence is currently evaluated to be within.
        /// </summary>
        Instant,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.6pv3g621q4x5
        /// Sequential charge sequence keys activate only once, when they stop receiving input from their skill.
        /// Sequential charge sequence keys show aiming assists so long as they are receiving input.
        /// Sequential charge sequence keys increase their duration as they continue to receive input.
        /// Activation instantiates all game objects on the sequence key that the sequence is currently evaluated to be within.
        /// </summary>
        SequentialCharge,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.yxleqhon0566
        /// Guaranteed charge sequence keys activate only once, when they stop receiving input from their skill, or when they exceed their duration.
        /// Guaranteed charge sequence keys show aiming assists so long as they are receiving input.
        /// Guaranteed charge sequence keys increase their duration as they continue to receive input.
        /// Activation instantiates all game objects on the sequence key that the sequence is currently evaluated to be within.
        /// </summary>
        GuaranteedCharge,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.dkk012neg5at
        /// Interruptible channel sequence keys activate when receiving input, and deactivate when input is stopped.
        /// Interruptible channel sequence keys increase their duration as they continue to receive input.
        /// Activation instantiates all game objects on the sequence key.
        /// Deactivation deinstantiates all game objects currently instantiated by activation.
        /// </summary>
        InterruptibleChannel,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.1w43wvswm6ou
        /// Non-Interruptible channel sequence keys activate when receiving input, and deactivate when the duration is exceeded, removing input does not deactivate them.
        /// Non-Interruptible channel sequence keys increase their duration so long as the player is alive, and they have been activated.
        /// Activation instantiates all game objects on the sequence key.
        /// Deactivation deinstantiates all game objects currently instantiated by activation.
        /// </summary>
        NonInterruptibleChannel,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.ty0ad2jkp5dn
        /// Delay sequence keys activate at the end of their duration, and will activate so long as they receive input from their skill.
        /// Delay sequence keys increase their duration so long as the player is alive, and they have received input.
        /// Activation instantiates all game objects on the sequence key that the sequence is currently evaluated to be within.
        /// </summary>
        Delay
    };

    [Tooltip("Defines the name for this skill sequence, this is purely for ease of use.")]
    public string name;
    [Header("Format"), Tooltip("This is the sequence format and defines how this sequence key progresses through activation, and input.")]
    public SequenceFormat format;
    [Tooltip("On what input in the skill will this be eligible for activation.")]
    public int interval = 0;
    [Tooltip("This is the duration this sequence key takes, and is the contribution to total sequence duration of this key.")]
    public float duration;
    [Tooltip("This is the current duration this sequence key shas been active through.")]
    public float durationCurrent;
    [Tooltip("This is the cooldown override this sequence key will set the skill to have for one activation, if this key is the key that activates.")]
    public float cooldownOverride;
    [Tooltip("This is the array of instantiables this key will create on activation.")]
    public Instantiable[] instantiables;
}
