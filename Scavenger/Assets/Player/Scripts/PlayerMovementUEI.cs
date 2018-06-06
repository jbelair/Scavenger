using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementUEI : MonoBehaviour
{
    public Statistics statistics;

    public string statisticMovementInput = "Movement Input";
    public string statisticRigidbody2D = "Rigidbody";
    public string statisticAcceleration = "Acceleration";
    public string statisticMaximumSpeed = "Maximum Speed";
    public string statisticThrustDelta = "Thrust Delta";
    public string statisticManeuverability = "Maneuverability";

    private Statistic movementInput, 
        acceleration,
        maximumSpeed,
        thrustDelta,
        maneuverability;

    public new Rigidbody2D rigidbody;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementInput == null)
        {
            movementInput = statistics[statisticMovementInput];
            rigidbody = statistics[statisticRigidbody2D].Get<Rigidbody2D>();
            acceleration = statistics[statisticAcceleration];
            maximumSpeed = statistics[statisticMaximumSpeed];
            thrustDelta = statistics[statisticThrustDelta];
            maneuverability = statistics[statisticManeuverability];
        }
        // Check if movement input is being supplied
        Vector2 input = movementInput;

        if (input.magnitude > 0)
        {
            // Accelerate
            float inputAngle = Mathf.Atan2(-input.x, input.y) * Mathf.Rad2Deg;

            if (Mathf.Abs(rigidbody.rotation - inputAngle) < thrustDelta)
                rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, new Vector2(-Mathf.Sin(rigidbody.rotation * Mathf.Deg2Rad), Mathf.Cos(rigidbody.rotation * Mathf.Deg2Rad)) * maximumSpeed * input.magnitude, acceleration * Time.fixedDeltaTime);

            rigidbody.rotation = Mathf.MoveTowardsAngle(rigidbody.rotation, inputAngle, maneuverability * Time.fixedDeltaTime);
        }
        else
        {
            if (rigidbody.velocity.magnitude > 0)
            {
                // Decelerate
                rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, Vector2.zero, acceleration * Time.fixedDeltaTime);
            }
        }
    }

    public void SetMovement(Vector2 input)
    {
        movementInput.Set(input);
    }
}
