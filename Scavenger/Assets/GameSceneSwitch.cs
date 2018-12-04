using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneSwitch : MonoBehaviour
{
    public string scene = "";

    public void Switch(string scene)
    {
        this.scene = scene;

        PlayerUEI player = FindObjectOfType<PlayerUEI>();
        Destroy(player.GetComponentInChildren<Ship>().gameObject);
        DontDestroyOnLoad(player);

        SceneManager.LoadSceneAsync(scene);
    }
}
