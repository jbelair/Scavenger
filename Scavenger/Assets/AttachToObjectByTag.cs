using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttachToObjectByTag : MonoBehaviour
{
    public string scene = "";
    public new string tag = "";
    public GameObject target;

    public bool keepPosition = true;
    public bool localPosition = true;
    public bool keepRotation = true;
    public bool localRotation = true;
    public bool keepScale = true;

    // Use this for initialization
    void Awake()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (target == null)
        {
            yield return new WaitForEndOfFrame();
            if (scene != "")
            {
                List<GameObject> objs = new List<GameObject>(SceneManager.GetSceneByName(scene).GetRootGameObjects());
                foreach (GameObject obj in objs)
                {
                    foreach (Transform t in obj.transform)
                    {
                        if (t.CompareTag(tag))
                        {
                            target = t.gameObject;
                            break;
                        }
                    }

                    if (target)
                        break;
                }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag(tag);
            }
        }

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Vector3 scale = transform.localScale;

        transform.SetParent(target.transform);

        if (keepPosition)
        {
            if (localPosition)
                transform.localPosition = position;
            else
                transform.position = position;
        }

        if (keepRotation)
        {
            if (localRotation)
                transform.localRotation = rotation;
            else
                transform.rotation = rotation;
        }

        if (keepScale)
        {
            transform.localScale = scale;
        }

        yield return null;
    }
}
