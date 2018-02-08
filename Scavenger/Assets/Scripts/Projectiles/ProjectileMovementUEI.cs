using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementUEI : MonoBehaviour
{
    public enum MovementFormat
    {
        /// <summary>
        /// MovementFormat.Shot is the standard projectile behaviour, and creates a MovementFormatBehaviour that moves the projectile in a straight line along its current forward axis.
        /// </summary>
        Shot,
        /// <summary>
        /// MovementFormat.Wander uses a modified version of shot behaviour, where it refers to the lateral and radial wander parameters to either add lateral velocity or to rotate the forward axis.
        /// </summary>
        Wander,
        /// <summary>
        /// MovementFormat.Home uses a modified version of shot behaviour, where the projectile continuously attempts to align its forward axis in the direction of the target or target transform.
        /// </summary>
        Home,
        /// <summary>
        /// MovementFormat.Steer gathers user input data to determine what orientation the projectile should take, based on the direction of the players current targeting input.
        /// </summary>
        Steer,
        /// <summary>
        /// MovementFormat.Boomerang travels out to the specified range before returning to the player's current location.
        /// </summary>
        Boomerang,
        /// <summary>
        /// MovementFormat.RangeWander travels out to the specified range as a shot before becoming wander.
        /// </summary>
        RangeWander,
        /// <summary>
        /// MovementFormat.RangeHome travels out to the specified range as a shot before becoming homing.
        /// </summary>
        RangeHome,
        /// <summary>
        /// MovementFormat.RangeInput travels out to the specified range as a shot before becoming steered.
        /// </summary>
        RangeSteer,
    };

    [Tooltip("Owner is the creator of this projectile")]
    public GameObject owner;
    [Tooltip("Rigidbody is the 2D rigidbody of this projectile.")]
    public new Rigidbody2D rigidbody;
    [Tooltip("Skill is the current instance of a skill responsible for making this projectile")]
    public SkillUEI skill;
    [Tooltip("Format defines what behaviour the projectile will use in game. Check the GDD for descriptions of each, or the enum in the code.")]
    public MovementFormat format = MovementFormat.Shot;
    [Tooltip("Velocity defines the starting speed this projectile will be traveling at.")]
    public float velocity = 1000;
    [Tooltip("Velocity Max defines the maximum velocity this projectile can achieve.")]
    public float velocityMax = 1000;
    [Tooltip("Velocity Relative defines the relative velocity of the owner when this projectile was made, which it should inherit.")]
    public float velocityRelative = 0;
    [Tooltip("Acceleration defines how many metres per second squared will be added to velocity until it reaches velocityMax.")]
    public float acceleration = 1000;
    [Tooltip("Turn Rate is used by any behaviours that cause the projectile to change orientation, and acts as a maximum limiter on rotation. In radians per second")]
    public float turnRate = 0;
    [Tooltip("Range defines the range parameter for some behaviours, causing them to alter projectile behaviour past this range.")]
    public float range = 1000;
    [Tooltip("If the projectile needs to know about a target, because its behaviour needs it, this is what it will use.")]
    public GameObject target = null;
    [Tooltip("If the projectile needs to know about a transform, because its behaviour needs it, this is what it will use.")]
    public Transform targetTransform = null;
    [Tooltip("Lateral Wander defines what percentage of the acceleration will be applied to make the projectile wander to its left or right locally.")]
    public float lateralWander = 0;
    [Tooltip("Lateral Wander Time defines how long it takes to add lateral velocity to the ship, defining how rapidly the projectile can wander left and right locally.")]
    public float lateralWanderTime = 0;
    [Range(0.0f,1.0f),Tooltip("Lateral Wander Randomness defines the randomness of the duration of each lateral shift.")]
    public float lateralWanderRandomness = 0;
    public Vector2 lateralWanderOffset;
    public float lateralWanderCurrent = 0;
    [Tooltip("Radial Wander defines what maximum rotation in radians can be achieved in a second, causing the projectile to spiral and turn.")]
    public float radialWander = 0;
    [Tooltip("Radial Wander Time defines how long it takes to transition to the new radians orientation.")]
    public float radialWanderTime = 0;
    [Range(0.0f,1.0f),Tooltip("Radial Wander Randomness defines the randomness of the duration of each radial shift.")]
    public float radialWanderRandomness = 0;
    public float radialWanderOffset = 0;
    public float radialWanderCurrent = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProjectileMovement.Move(this);
    }
}
