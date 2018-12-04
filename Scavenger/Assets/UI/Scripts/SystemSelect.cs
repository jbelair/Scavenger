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
        //if (environment["Stars"].Get<int>() > 1)
        //{
            widget = UIManager.active.Button("systems", UIManager.Layer.Back, "System", Camera.main.WorldToScreenPoint(transform.position * 1000f), Transition, transform);
            widget.GetComponentInChildren<WidgetSystem>().Set(environment);
        //}
        //else
        //{
        //    Debug.Log("Deleting System -----\nNo Stars");
        //    Destroy(gameObject);
        //}
    }

    void Transition()
    {
        if (Environment.JumpFuel >= Environment.jumpDistance && Environment.jumpDistance <= Environment.JumpRadius)
        {
            if (!systems)
                systems = SystemsGenerator.active;

            Vector3 position = environment["System Coordinates"];
            Environment.JumpFuel = Environment.JumpFuel - Environment.jumpDistance;
            Players.players[0].statistics["stat_jump_fuel_cur"].Set(Environment.JumpFuel);
            Environment.selectedCoordinates = Environment.SystemCoordinates = position.XYO().ToInt();
            Environment.jumpDistance = 0;
            systems.lastPosition = systems.position.position = position.XY();
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
