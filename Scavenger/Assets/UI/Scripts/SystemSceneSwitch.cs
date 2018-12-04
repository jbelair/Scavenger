using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemSceneSwitch : MonoBehaviour
{
    public Statistics environment;
    public GameObject widget;

    // Use this for initialization
    void Start()
    {
        if (environment["Stars"].Get<int>() > 1)
        {
            widget = UIManager.active.Button("systems navigation", UIManager.Layer.Back, "System", Camera.main.WorldToScreenPoint(transform.position * 1000f), Transition, transform);
            widget.GetComponentInChildren<WidgetSystem>().Set(environment);
        }
        //else
        //    Destroy(gameObject);
    }

    void Transition()
    {
        if (Environment.JumpFuel >= Environment.jumpDistance)
        {
            Vector3 position = environment["System Coordinates"];
            Environment.JumpFuel = Environment.JumpFuel - Environment.jumpDistance;
            Environment.selectedCoordinates = Environment.SystemCoordinates = position.ToInt();
            Environment.jumpDistance = 0;
            //SystemsGenerator.active.Regenerate();
            SceneManager.LoadScene("Game.Load", LoadSceneMode.Single);
            Environment.sceneName = "Game.System";
        }
    }

    private void OnDestroy()
    {
        if (environment["Stars"].Get<int>() > 1)
            Destroy(widget);
    }
}
