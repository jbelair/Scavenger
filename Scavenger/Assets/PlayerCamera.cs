using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera/Player")]
public class PlayerCamera : MonoBehaviour
{
    public PlayerUEI player;

    [Tooltip("How near to the player the camera can get")]
    public float near = 100;
    [Tooltip("How far from the player the camera can get")]
    public float far = 500;
    [Tooltip("What is the lowest angle the camera can approach")]
    public float low = 270;
    [Tooltip("What is the highest angle the camera can approach")]
    public float high = 180;
    [Tooltip("The speed the camera is able to move, elastically to catch up to the player")]
    public float speed = 100;

    [Tooltip("Will the camera attempt to move towards the cursor, and away from the player")]
    public bool followCursor = true;
    [Tooltip("How far away from the player is the camera allowed to chase the cursor. (0 no movement, 1 player must stay on edge of screen)"), Range(0,1)]
    public float cursorAttraction = 1;

    [Tooltip("What player statistic dictates the camera's current zoom level")]
    public string statisticZoom = "Zoom";
    public float zoomSteps = 32;
    public float zoomLevel;
    private Statistic zoom;

    public bool initialised = false;

    // Use this for initialization
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised)
            Initialise();
        else
            Position();
    }

    void Position()
    {
        // First determine the unaligned position the camera must have given the zoom level, near, far, high, and low.
        zoomLevel = Mathf.Clamp(zoomLevel + zoom, 0, zoomSteps);
        float z = zoomLevel / zoomSteps;
        float theta = Mathf.LerpAngle(low, high, z) * Mathf.Deg2Rad;
        Vector3 position = new Vector3(0, Mathf.Cos(theta), Mathf.Abs(Mathf.Sin(theta))) * Mathf.Lerp(near, far, z);

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + position, speed * Time.deltaTime);

        if (zoomLevel < zoomSteps)
            transform.LookAt(player.transform.position, new Vector3(0, 0, 1));
        else
            transform.LookAt(player.transform.position);
    }

    bool Initialise()
    {
        if (initialised)
            return false;
        else
        {
            if (player.statistics.Has(statisticZoom))
            {
                zoom = player.statistics[statisticZoom];
                Position();
                initialised = true;
                return true;
            }

            return false;
        }
    }
}
