using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSelect : MonoBehaviour
{
    public SystemsGenerator systems;
    public Statistics environment;
    public GameObject widget;

    // Use this for initialization
    void Start()
    {
        if (environment["Stars"].Get<int>() > 1)
        {
            widget = UIManager.active.Button("systems", UIManager.Layer.Back, "System", Camera.main.WorldToScreenPoint(transform.position * 1000f), Transition, transform);
            widget.GetComponentInChildren<WidgetSystem>().Set(environment);
        }
        else
            Destroy(gameObject);
    }

    void Transition()
    {
        if (Environment.jumpFuel >= Environment.jumpDistance)
        {
            if (!systems)
                systems = SystemsGenerator.active;

            Vector3 position = environment["System Coordinates"];
            Environment.jumpFuel = Environment.jumpFuel - Environment.jumpDistance;
            Environment.selectedCoordinates = Environment.systemCoordinates = position.XY();
            Environment.jumpDistance = 0;
            systems.lastPosition = systems.position.position = position.XY() * systems.scale;
            UIManager.active.ClearBack("system navigation");
            UIManager.active.AddScreen("system navigation");
            systems.Regenerate();
        }
    }

    private void OnDestroy()
    {
        if (widget)
        {
            widget.SendMessage("Die");
            Destroy(widget);
        }
    }
}
