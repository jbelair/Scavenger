using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneSwitch : MonoBehaviour
{
    public string scene = "";
    private AsyncOperation op;

    public void Switch(string scene)
    {
        if (op == null)
        {
            this.scene = scene;

            PlayerUEI player = FindObjectOfType<PlayerUEI>();
            Destroy(player.GetComponentInChildren<Ship>().gameObject);
            DontDestroyOnLoad(player);

            op = SceneManager.LoadSceneAsync(scene);
            op.completed += Op_completed;
        }
    }

    void Op_completed(AsyncOperation async)
    {
        op = null;
    }
}
