using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ProjectileMovement
{
    public delegate Vector2 MoveProjectile(ProjectileMovementUEI projectile);

    public static Dictionary<ProjectileMovementUEI.MovementFormat, MoveProjectile> Movements = new Dictionary<ProjectileMovementUEI.MovementFormat, MoveProjectile>();

    static ProjectileMovement()
    {
        Movements.Add(ProjectileMovementUEI.MovementFormat.Shot, new MoveProjectile(Shot));
        Movements.Add(ProjectileMovementUEI.MovementFormat.Wander, new MoveProjectile(Wander));
        Movements.Add(ProjectileMovementUEI.MovementFormat.Home, new MoveProjectile(Home));
        Movements.Add(ProjectileMovementUEI.MovementFormat.Steer, new MoveProjectile(Steer));
        Movements.Add(ProjectileMovementUEI.MovementFormat.Boomerang, new MoveProjectile(Boomerang));
        Movements.Add(ProjectileMovementUEI.MovementFormat.RangeWander, new MoveProjectile(RangeWander));
        Movements.Add(ProjectileMovementUEI.MovementFormat.RangeHome, new MoveProjectile(RangeHome));
        Movements.Add(ProjectileMovementUEI.MovementFormat.RangeSteer, new MoveProjectile(RangeSteer));
    }

    public static void Move(ProjectileMovementUEI projectile)
    {
        projectile.rigidbody.MovePosition(Movements[projectile.format].Invoke(projectile));
    }

    /// <summary>
    /// Shot takes a projectile and moves it along its forward at the specified velocity, accelerating by the specified acceleration, and capping at the specified maximum velocity.
    /// </summary>
    /// <param name="projectile">The projectile executing Shot movement logic.</param>
    public static Vector2 Shot(ProjectileMovementUEI projectile)
    {
        projectile.velocity += projectile.acceleration * Time.deltaTime;

        if (projectile.velocity > projectile.velocityMax + projectile.velocityRelative)
            projectile.velocity = projectile.velocityMax + projectile.velocityRelative;

        return projectile.rigidbody.position + (Vector2)projectile.transform.up * projectile.velocity;

        //projectile.rigidbody.MovePosition();
    }

    /// <summary>
    /// Wander takes a projectile and performs shot logic, adding lateral and radial wander to the projectile.
    /// </summary>
    /// <param name="projectile">The projectile executing Wander movement logic.</param>
    public static Vector2 Wander(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);

        if (projectile.lateralWander > 0)
        {
            if (projectile.lateralWanderCurrent >= projectile.lateralWanderTime)
            {
                projectile.lateralWanderCurrent -= projectile.lateralWanderTime + UnityEngine.Random.Range(0.0f, projectile.lateralWanderTime) * projectile.lateralWanderRandomness;
                projectile.lateralWanderOffset = UnityEngine.Random.Range(-1.0f, 1.0f) * (Vector2)projectile.transform.right * projectile.acceleration * projectile.lateralWander;
            }

            position += projectile.lateralWanderOffset * Time.deltaTime / projectile.lateralWanderTime;
            projectile.lateralWanderCurrent += Time.deltaTime;
        }

        if (projectile.radialWander > 0)
        {
            if (projectile.radialWanderCurrent >= projectile.radialWanderTime)
            {
                projectile.radialWanderCurrent -= projectile.radialWanderTime + UnityEngine.Random.Range(0.0f, projectile.radialWanderTime) * projectile.radialWanderRandomness;
                projectile.radialWanderOffset = Mathf.Clamp(UnityEngine.Random.Range(-1.0f, 1.0f) * Mathf.PI * 2.0f * projectile.radialWander, -projectile.turnRate * projectile.radialWanderTime, projectile.turnRate * projectile.radialWanderTime);
            }

            projectile.rigidbody.MoveRotation(projectile.rigidbody.rotation + projectile.radialWanderOffset * Time.deltaTime / projectile.radialWanderTime * Mathf.Rad2Deg);
            projectile.radialWanderCurrent += Time.deltaTime;
        }

        return position;
    }

    /// <summary>
    /// Home takes a projectile and performs shot logic, adding forward targeting of either a target GameObject or Transform.
    /// </summary>
    /// <param name="projectile">The projectile executing Home movement logic.</param>
    public static Vector2 Home(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }

    /// <summary>
    /// Steer takes a projectile and performs shot logic, adding forward to match player aiming input.
    /// </summary>
    /// <param name="projectile">The projectile executing Steer movement logic.</param>
    public static Vector2 Steer(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }

    /// <summary>
    /// Boomerang takes a projectile and performs shot logic, reversing course at a specified range, before switching to Home logic on the owner of the projectile.
    /// </summary>
    /// <param name="projectile">The projectile executing Boomerang movement logic.</param>
    public static Vector2 Boomerang(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }

    /// <summary>
    /// RangeWander takes a projectile and performs shot logic, switching to perform Wander logic after the specified range.
    /// </summary>
    /// <param name="projectile">The projectile executing RangeWander movement logic.</param>
    public static Vector2 RangeWander(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }

    /// <summary>
    /// RangeHome takes a projectile and performs shot logic, switching to Home logic after the specified range.
    /// </summary>
    /// <param name="projectile"></param>
    public static Vector2 RangeHome(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }

    /// <summary>
    /// RangeSteer takes a projectile and performs shot logic, switching to Steer logic after the specified range.
    /// </summary>
    /// <param name="projectile"></param>
    public static Vector2 RangeSteer(ProjectileMovementUEI projectile)
    {
        Vector2 position = Shot(projectile);
        return position;
    }
}
