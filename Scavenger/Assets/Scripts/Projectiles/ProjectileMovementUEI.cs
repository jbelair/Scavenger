using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementUEI : MonoBehaviour
{
    public ProjectileUEI projectile;
    //[Header("Basics"), Tooltip("Owner is the creator of this projectile")]
    //public GameObject owner;
    //[Tooltip("Rigidbody is the 2D rigidbody of this projectile.")]
    //public new Rigidbody2D rigidbody;
    //[Tooltip("Skill is the current instance of a skill responsible for making this projectile")]
    //public SkillUEI skill;
    //public Vector2 startPosition;
    //[Header("Format"), Tooltip("Format defines what behaviour the projectile will use in game. Check the GDD for descriptions of each, or the enum in the code.")]
    //public ProjectileMovement.MovementFormat format = ProjectileMovement.MovementFormat.Shot;
    //[Header("Movement"), Tooltip("Velocity defines the starting speed this projectile will be traveling at.")]
    //public float velocity = 1000;
    //[Tooltip("Velocity Max defines the maximum velocity this projectile can achieve.")]
    //public float velocityMax = 1000;
    //[Tooltip("Velocity Relative defines the relative velocity of the owner when this projectile was made, which it should inherit.")]
    //public float velocityRelative = 0;
    //[Tooltip("Acceleration defines how many metres per second squared will be added to velocity until it reaches velocityMax.")]
    //public float acceleration = 1000;
    //[Header("Turnrate"), Tooltip("Turn Rate is used by any behaviours that cause the projectile to change orientation, and acts as a maximum limiter on rotation. In degrees per second")]
    //public float turnRate = 0;
    //[Header("Homing"), Tooltip("If the projectile needs to know about a target, because its behaviour needs it, this is what it will use.")]
    //public Transform target = null;
    //[Header("Lateral Wander"), Tooltip("Lateral Wander defines what percentage of the acceleration will be applied to make the projectile wander to its left or right locally.")]
    //public float lateralWander = 0;
    //[Tooltip("Lateral Wander Time defines how long it takes to add lateral velocity to the ship, defining how rapidly the projectile can wander left and right locally.")]
    //public float lateralWanderTime = 0;
    //[Range(0.0f, 1.0f), Tooltip("Lateral Wander Randomness defines the randomness of the duration of each lateral shift.")]
    //public float lateralWanderRandomness = 0;
    //[Header("Lateral Wander Debug")]
    //public Vector2 lateralWanderOffset;
    //public float lateralWanderCurrent = 0;
    //[Header("Radial Wander"), Tooltip("Radial Wander defines what maximum rotation in radians can be achieved in a second, causing the projectile to spiral and turn.")]
    //public float radialWander = 0;
    //[Tooltip("Radial Wander Time defines how long it takes to transition to the new radians orientation.")]
    //public float radialWanderTime = 0;
    //[Range(0.0f, 1.0f), Tooltip("Radial Wander Randomness defines the randomness of the duration of each radial shift.")]
    //public float radialWanderRandomness = 0;
    //[Header("Radial Wander Debug")]
    //public float radialWanderOffset = 0;
    //public float radialWanderCurrent = 0;

    public bool debug = false;

    // Use this for initialization
    void Start()
    {
        //rigidbody = projectile.statistics["Rigidbody"].Get<GameObject>().GetComponent<Rigidbody2D>();
        //startPosition = rigidbody.position;

        projectile.statistics["Starting Position"].Set(projectile.statistics["Rigidbody"].Get<GameObject>().GetComponent<Rigidbody2D>().position);
        projectile.statistics["Relative Velocity"].Set(projectile.statistics["Owner"].Get<GameObject>().GetComponent<Rigidbody2D>().velocity);
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileMovement.Move(this);
    }
}
