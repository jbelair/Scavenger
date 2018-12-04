using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetFullscreenShipViewer : MonoBehaviour
{
    public enum Axis { None, X, IX, Y, IY, Z, IZ };
    public Transform target;
    public float sensitivity;
    public float rotationSpeed = 15;
    public Axis mouseX;
    public Axis mouseY;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    public float zoomCurrent = 0;
    public float zoomMaximum = 100;
    private Vector3 velocity;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && InventoryShips.active.activeShips[InventoryShips.active.index].isUnlocked)
        {
            target = InventoryShips.active.focus;

            Vector2 delta =  mousePosition - Input.mousePosition.XY();

            //Quaternion start = target.rotation;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                switch (mouseX)
                {
                    case Axis.X:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.right * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.right, delta.x * sensitivity);
                        break;
                    case Axis.IX:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.left * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.right, delta.x * sensitivity);
                        break;
                    case Axis.Y:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.up * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.up, delta.x * sensitivity);
                        break;
                    case Axis.IY:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.down * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.up, delta.x * sensitivity);
                        break;
                    case Axis.Z:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.forward * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.forward, delta.x * sensitivity);
                        break;
                    case Axis.IZ:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.back * delta.x * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.forward, delta.x * sensitivity);
                        break;
                }
            }
            else
            {
                switch (mouseY)
                {
                    case Axis.X:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.right * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.right, delta.y * sensitivity);
                        break;
                    case Axis.IX:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.left * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.right, delta.y * sensitivity);
                        break;
                    case Axis.Y:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.up * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.up, delta.y * sensitivity);
                        break;
                    case Axis.IY:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.down * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.up, delta.y * sensitivity);
                        break;
                    case Axis.Z:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.forward * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.forward, delta.y * sensitivity);
                        break;
                    case Axis.IZ:
                        target.localRotation = Quaternion.RotateTowards(target.localRotation, Quaternion.Euler(Vector3.back * delta.y * sensitivity + target.localRotation.eulerAngles), rotationSpeed * Time.deltaTime);
                        //target.Rotate(Vector3.forward, delta.y * sensitivity);
                        break;
                }
            }
            //InventoryShips.active.activeMode.rotation = target.rotation.eulerAngles - start.eulerAngles;

            //mousePosition = Input.mousePosition;
        }

        zoomCurrent = Mathf.Clamp(zoomCurrent + Players.players[0].statistics["Scroll"].Get<float>(), 0, zoomMaximum);
        CameraNode node = InventoryShips.active.activeShips[InventoryShips.active.index].node;
        float zoom = Mathf.Lerp(minZoom, maxZoom, Mathf.Clamp01(zoomCurrent / zoomMaximum));
        node.transform.position = Vector3.SmoothDamp(node.transform.position, InventoryShips.active.activeShips[InventoryShips.active.index].renderer.bounds.size * zoom, ref velocity, Time.deltaTime);
    }

    public bool isDragging = false;
    public Vector2 mousePosition;
    public void Begin()
    {
        mousePosition = Input.mousePosition;
        isDragging = true;
    }

    public void End()
    {
        isDragging = false;
    }

    public void Reset()
    {
        target.rotation = Quaternion.Euler(0, 0, 180);
    }
}
