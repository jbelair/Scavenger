using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSceneAdd : MonoBehaviour
{
    public DungeonGenerator active;
    public string loadedScene = "";
    public AsyncOperation loader;

    // Update is called once per frame
    void Update()
    {
        if (active && loader == null)
        {
            //Scene scene = Resources.Load<Scene>("Dungeons/Scenes/" + active.dungeonType.name);
            loadedScene = Application.CanStreamedLevelBeLoaded(active.dungeonType.name) ? active.dungeonType.name : "signals_default";
            loader = SceneManager.LoadSceneAsync(loadedScene, LoadSceneMode.Additive);
            loader.completed += Op_completed;
        }
    }

    private void Op_completed(AsyncOperation obj)
    {
        // Instantiate all hazards
        if (active.dungeonType.hazards != null)
        {
            string[] split = active.dungeonType.hazards.Split(new char[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in split)
            {
                GameObject hazard = Resources.Load<GameObject>("Dungeons/Objects/" + str);
                if (hazard)
                {
                    hazard = Instantiate(hazard, Vector3.zero, new Quaternion());
                    hazard.name = str;
                    if (!hazard.GetComponentInChildren<Hazard>())
                        hazard.AddComponent<Hazard>();
                }
            }
        }

        // Set the active scene to the newly loaded scene (so that all instantiates now occur in this scene)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedScene));

        // Move Camera to the dungeon
        Transform trans = Camera.main.transform;
        while (trans.parent != null)
            trans = trans.parent;
        SceneManager.MoveGameObjectToScene(trans.gameObject, SceneManager.GetActiveScene());

        // Move Environment to the dungeon
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        // Move Player to the dungeon
        Player player = FindObjectOfType<Player>();
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
        player.GetComponentInChildren<PlayerMovementUEI>().enabled = true;
    }
}
