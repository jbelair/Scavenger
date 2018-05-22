using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EditSnapshot : MonoBehaviour
{
    public Camera camera;
    public int supersize = 1;

    private void Awake()
    {
        if (!camera)
            camera = GetComponentInParent<Camera>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && Input.GetKeyUp(KeyCode.F12))
        {
            //Debug.Log(Application.dataPath);
            string[] splits = Application.dataPath.Split('/');
            // I will always have /Assets as the last element; I just want that one gone;
            string path = "";// Application.dataPath;
            for(int j = 0; j < splits.Length - 1; j++)
            {
                path += splits[j] + "/";
            }
            path = path + "Captures/Screenshots/";
            Debug.Log(path);
            List<string> names = new List<string>(Directory.GetFiles(path));
            Debug.Log(names.Count);
            int i = 0;
            string name = path + "_editScreenshot" + i.ToString() + ".png";
            //Debug.Log(names[i]);
            while (names.Contains(name))
            {
                i++;
                name = path + "_editScreenshot" + i.ToString() + ".png";
                //Debug.Log(names[i]);
            }
            Debug.Log("Screen capture: " + i.ToString());
            name = "_editScreenshot" + i.ToString();
            ScreenCapture.CaptureScreenshot("Captures/Screenshots/" + name + ".png", supersize);
        }
    }
}
