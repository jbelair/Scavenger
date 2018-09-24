using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Image filled;
    // Use this for initialization
    void Start()
    {
        Environment.sceneLoading = SceneManager.LoadSceneAsync(Environment.sceneName, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        filled.fillAmount = Environment.sceneLoading.progress;
    }
}
