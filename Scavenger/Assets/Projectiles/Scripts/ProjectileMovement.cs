using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ProjectileMovement
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
        Home,
        /// <summary>
        /// MovementFormat.Steer gathers user input data to determine what orientation the projectile should take, based on the direction of the players current targeting input.
        /// </summary>
        Steer,
        /// <summary>
        /// MovementFormat.Return uses a modified version of homing behaviour, where the projectile attempts to return to its owner.
        /// </summary>
        Return,
        /// <summary>
        /// MovementFormat.Orbit travels around an object trying to maintain a certain range orbit, at a certain speed.
        /// </summary>
        Orbit,
        /// <summary>
        /// MovementFormat.RangeWander travels out to the specified range as a shot before becoming wander.
        /// </summary>
    };

    public delegate Vector2 MoveProjectile(ProjectileMovementUEI move);

    public static Dictionary<MovementFormat, MoveProjectile> Movements = new Dictionary<MovementFormat, MoveProjectile>();

    static ProjectileMovement()
    {
        Movements.Add(MovementFormat.Shot, new MoveProjectile(Shot));
        Movements.Add(MovementFormat.Home, new MoveProjectile(Home));
        Movements.Add(MovementFormat.Steer, new MoveProjectile(Steer));
        Movements.Add(MovementFormat.Return, new MoveProjectile(Return));
        Movements.Add(MovementFormat.Orbit, new MoveProjectile(Orbit));
    }

    public static void Move(ProjectileMovementUEI move)
    {
        move.rigidbody.Get<Rigidbody2D>().MovePosition(Movements[(MovementFormat)move.format.Get<int>()].Invoke(move));
    }

    /// <summary>
    /// Shot takes a projectile and moves it along its forward at the specified velocity, accelerating by the specified acceleration, and capping at the specified maximum velocity.
    /// </summary>
    /// <param name="move">The projectile executing Shot movement logic.</param>
    public static Vector2 Shot(ProjectileMovementUEI move)
    {
        move.velocity.Add(move.acceleration.Get<float>() * Time.deltaTime);

        if (move.velocity.Get<float>() > move.maximumVelocity.Get<float>())
            move.velocity.Set(move.maximumVelocity.Get<float>());

        Rigidbody2D rigid = move.rigidbody.Get<Rigidbody2D>();
        Vector2 position = rigid.position + ((Vector2)move.transform.up * move.velocity.Get<float>() + move.relativeVelocity.Get<Vector2>()) * Time.deltaTime;

        if (move.projectile.statistics.Has("Lateral Wander"))
        {
            float lateralWander = move.lateralWander.Get<float>();
            float lateralWanderTime = move.lateralWanderTime.Get<float>();
            if (move.lateralWanderCurrent.Get<float>() >= lateralWanderTime)
            {
                move.lateralWanderCurrent.Set(move.lateralWanderCurrent.Get<float>() - lateralWanderTime + UnityEngine.Random.Range(0.0f, lateralWanderTime) * move.lateralWanderRandom.Get<float>());
                move.lateralWanderOffset.Set(UnityEngine.Random.Range(-1.0f, 1.0f) * (Vector2)move.transform.right * move.acceleration.Get<float>() * lateralWander);
            }

            position += move.lateralWanderOffset.Get<Vector2>() * Time.deltaTime / lateralWanderTime;
            move.lateralWanderCurrent.Set(move.lateralWanderCurrent.Get<float>() + Time.deltaTime);
        }

        if (move.projectile.statistics.Has("Radial Wander"))
        {
            float radialWander = move.radialWander.Get<float>();
            float radialWanderTime = move.radialWanderTime.Get<float>();
            if (move.radialWanderCurrent.Get<float>() >= radialWanderTime)
            {
                move.radialWanderCurrent.Set(move.radialWanderCurrent.Get<float>() - radialWanderTime + UnityEngine.Random.Range(0.0f, radialWanderTime) * move.radialWanderRandom.Get<float>());
                move.radialWanderOffset.Set((UnityEngine.Random.Range(-1.0f, 1.0f) * move.turnRate.Get<float>() * radialWander / radialWanderTime) * Mathf.Deg2Rad);
            }

            rigid.MoveRotation(rigid.rotation + move.radialWanderOffset.Get<float>() * Time.deltaTime);
            move.radialWanderCurrent.Set(move.radialWanderCurrent.Get<float>() + Time.deltaTime);
        }

        return position;
    }

    /// <summary>
    /// Home takes a projectile and performs shot logic, adding forward targeting of either a target GameObject or Transform.
    /// </summary>
    /// <param name="move">The projectile executing Home movement logic.</param>
    public static Vector2 Home(ProjectileMovementUEI move)
    {
        Vector2 position = Shot(move);
        Vector2 delta = move.transform.up;

        if (move.projectile.statistics.Has("Target"))
        {
            delta = move.target.Get<GameObject>().transform.position - move.transform.position;
        }

        delta.Normalize();

        // dotR = dot product of delta and projectile's right
        float dotR = Vector2.Dot(move.transform.right, delta);

        Rigidbody2D rigid = move.rigidbody.Get<Rigidbody2D>();
        float turnRate = move.turnRate.Get<float>();
        // as dotR approaches 1 the projectile's right side is facing the direction the projectile's forward needs to face
        // as dotR falls below 0 the left side is facing the direction the projectiles forward needs to face
        if (dotR <= 0)
        {
            // Since the left side is facing the correct direction, turn left.
            rigid.MoveRotation(rigid.rotation + turnRate * Time.deltaTime);
        }
        else
        {
            // Since the right side is facing the correct direction, turn right.
            rigid.MoveRotation(rigid.rotation - turnRate * Time.deltaTime);
        }

        return position;
    }

    /// <summary>
    /// Steer takes a projectile and performs shot logic, adding forward to match player aiming input.
    /// </summary>
    /// <param name="move">The projectile executing Steer movement logic.</param>
    public static Vector2 Steer(ProjectileMovementUEI move)
    {
        Vector2 position = Shot(move);
        return position;
    }

    /// <summary>
    /// Boomerang takes a projectile and performs shot logic, reversing course at a specified range, before switching to Home logic on the owner of the projectile.
    /// </summary>
    /// <param name="move">The projectile executing Boomerang movement logic.</param>
    public static Vector2 Return(ProjectileMovementUEI move)
    {
        Vector2 position = Shot(move);
        Vector2 delta = move.parent.Get<GameObject>().transform.position - move.transform.position;

        delta.Normalize();

        // dotR = dot product of delta and projectile's right
        float dotR = Vector2.Dot(move.transform.right, delta);

        Rigidbody2D rigid = move.rigidbody.Get<Rigidbody2D>();
        float turnRate = move.turnRate.Get<float>();
        // as dotR approaches 1 the projectile's right side is facing the direction the projectile's forward needs to face
        // as dotR falls below 0 the left side is facing the direction the projectiles forward needs to face
        if (dotR <= 0)
        {
            // Since the left side is facing the correct direction, turn left.
            rigid.MoveRotation(rigid.rotation + turnRate * Time.deltaTime);
        }
        else
        {
            // Since the right side is facing the correct direction, turn right.
            rigid.MoveRotation(rigid.rotation - turnRate * Time.deltaTime);
        }

        return position;
    }

    /// <summary>
    /// Orbit takes a projectile and performs shot logic, making sure its forward is always tangental to the specified target, or target transform.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public static Vector2 Orbit(ProjectileMovementUEI move)
    {
        Vector2 position = Shot(move);

        // Calculate the delta between current position 

        //Vector2 tangent = Vector3.Cross(projectile.target.transform.position - projectile.transform.position, Vector3.forward);
        //Vector2 forward = Vector3.RotateTowards(projectile.transform.forward, tangent.normalized, projectile.turnRate, 0);
        //projectile.rigidbody.MoveRotation(Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
        //projectile.rigidbody.MoveRotation(projectile.rigidbody.rotation);

        return position;
    }
}
