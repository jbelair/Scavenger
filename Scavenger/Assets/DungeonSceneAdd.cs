using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonSceneAdd : MonoBehaviour
{
    public DungeonGenerator active;
    public SpriteRenderer sunScreen;
    public string loadedScene = "";
    public bool isInitialised = false;

    public void Initialise()
    {
        if (!isInitialised)
        {
            // Instantiate all hazards
            if (active)
            {
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

                if (active.dungeonTarget.Contains("Star"))
                {
                    sunScreen.color = Color.black.A(0.5f);
                }
                else
                {
                    sunScreen.color = Color.clear;
                }

                // Set the active scene to the newly loaded scene (so that all instantiates now occur in this scene)
                //SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedScene));

                // Move Camera to the dungeon
                //Transform trans = Camera.main.transform;
                //while (trans.parent != null)
                //    trans = trans.parent;
                //SceneManager.MoveGameObjectToScene(trans.gameObject, SceneManager.GetActiveScene());

                // Move Environment to the dungeon
                //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

                // Move Player to the dungeon
                Player player = FindObjectOfType<Player>();
                SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
                player.GetComponentInChildren<PlayerMovementUEI>().enabled = true;

                isInitialised = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Initialise();
    }
}
