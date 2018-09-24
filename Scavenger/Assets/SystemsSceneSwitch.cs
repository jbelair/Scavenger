using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemsSceneSwitch : MonoBehaviour
{
    public Statistics environment;
    
    public void Transition()
    {
        Environment.selectedCoordinates = Environment.systemCoordinates = environment["System Coordinates"];
        Environment.jumpDistance = 0;
        SceneManager.LoadScene("Game.Load", LoadSceneMode.Single);
        Environment.sceneName = "Game.Systems";
    }
}
