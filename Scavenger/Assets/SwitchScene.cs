using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string scene;

    private void Start()
    {
        
    }

    public void Switch(string scene)
    {
        this.scene = scene;
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }
}
