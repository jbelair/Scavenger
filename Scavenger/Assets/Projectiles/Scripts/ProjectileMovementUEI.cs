using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementUEI : MonoBehaviour
{
    public ProjectileUEI projectile;

    public string statisticPlayer = "Player";
    public string statisticRigidbody = "Rigidbody";
    public string statisticPlayerRigidbody = "Rigidbody";
    public string statisticFormat = "Format";
    public string statisticStartingPosition = "Starting Position";
    public string statisticVelocity = "Velocity";
    public string statisticMaximumVelocity = "Maximum Velocity";
    public string statisticRelativeVelocity = "Relative Velocity";
    public string statisticAcceleration = "Acceleration";
    public string statisticTurnRate = "Turn Rate";
    public string statisticLateralWander = "Lateral Wander";
    public string statisticLateralWanderTime = "Lateral Wander Time";
    public string statisticLateralWanderCurrent = "Lateral Wander Current";
    public string statisticLateralWanderOffset = "Lateral Wander Offset";
    public string statisticLateralWanderRandom = "Lateral Wander Random";
    public string statisticRadialWander = "Radial Wander";
    public string statisticRadialWanderTime = "Radial Wander Time";
    public string statisticRadialWanderCurrent = "Radial Wander Current";
    public string statisticRadialWanderOffset = "Radial Wander Offset";
    public string statisticRadialWanderRandom = "Radial Wander Random";
    public string statisticTarget = "Target";

    new public Statistic rigidbody;
    public Statistic player,
        startingPosition,
        format,
        velocity,
        maximumVelocity,
        relativeVelocity,
        acceleration,
        turnRate,
        lateralWander,
        lateralWanderTime,
        lateralWanderCurrent,
        lateralWanderOffset,
        lateralWanderRandom,
        radialWander,
        radialWanderTime,
        radialWanderCurrent,
        radialWanderOffset,
        radialWanderRandom,
        target;

    public bool debug = false;

    // Use this for initialization
    void Start()
    {
        player = projectile.statistics[statisticPlayer];
        rigidbody = projectile.statistics[statisticRigidbody];
        format = projectile.statistics[statisticFormat];
        startingPosition = projectile.statistics[statisticStartingPosition];
        velocity = projectile.statistics[statisticVelocity];
        maximumVelocity = projectile.statistics[statisticMaximumVelocity];
        relativeVelocity = projectile.statistics[statisticRelativeVelocity];
        acceleration = projectile.statistics[statisticAcceleration];
        if (projectile.statistics.Has(statisticTurnRate))
            turnRate = projectile.statistics[statisticTurnRate];
        if (projectile.statistics.Has(statisticLateralWander))
        {
            lateralWander = projectile.statistics[statisticLateralWander];
            lateralWanderTime = projectile.statistics[statisticLateralWanderTime];
            lateralWanderCurrent = projectile.statistics[statisticLateralWanderCurrent];
            lateralWanderOffset = projectile.statistics[statisticLateralWanderOffset];
            lateralWanderRandom = projectile.statistics[statisticLateralWanderRandom];
        }
        if (projectile.statistics.Has(statisticRadialWander))
        {
            radialWander = projectile.statistics[statisticRadialWander];
            radialWanderTime = projectile.statistics[statisticRadialWanderTime];
            radialWanderCurrent = projectile.statistics[statisticRadialWanderCurrent];
            radialWanderOffset = projectile.statistics[statisticRadialWanderOffset];
            radialWanderRandom = projectile.statistics[statisticRadialWanderRandom];
        }
        if (projectile.statistics.Has(statisticTarget))
            target = projectile.statistics[statisticTarget];

        startingPosition.Set(rigidbody.Get<Rigidbody2D>().position);
        relativeVelocity.Set(player.Get<PlayerUEI>().statistics[statisticPlayerRigidbody].Get<Rigidbody2D>().velocity);
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileMovement.Move(this);
    }
}
