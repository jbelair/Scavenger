using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementUEI : MonoBehaviour
{
    public Statistics statistics;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if movement input is being supplied
        Vector2 input = statistics["Movement Input"].Get<Vector2>();
        Rigidbody2D rigid = statistics["Rigidbody"].Get<Rigidbody2D>();

        if (input.magnitude > 0)
        {
            // Accelerate
            rigid.velocity = Vector2.MoveTowards(rigid.velocity, input * statistics["Maximum Velocity"].Get<float>() * input.magnitude, statistics["Acceleration"].Get<float>() * Time.fixedDeltaTime);
            rigid.rotation = Mathf.MoveTowardsAngle(rigid.rotation, Mathf.Atan2(-rigid.velocity.x, rigid.velocity.y) * Mathf.Rad2Deg, statistics["Turn Rate"].Get<float>() * Time.fixedDeltaTime);
        }
        else
        {
            if (rigid.velocity.magnitude > 0)
            {
                // Decelerate
                rigid.velocity = Vector2.MoveTowards(rigid.velocity, Vector2.zero, statistics["Acceleration"].Get<float>() * Time.fixedDeltaTime);
            }
        }
    }
}
