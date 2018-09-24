using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SystemsCamera : MonoBehaviour
{
    new public Camera camera;
    public Statistics statistics;

    public Statistic mousePosition;
    public Statistic scroll;
    public Statistic scrollCount;
    public Statistic zoomMinimum;

    public Vector3 zoomInOffset = new Vector3(0, -1.4f, -1f);
    public Vector3 zoomOutOffset = new Vector3(0, 0, -2f);

    // Use this for initialization
    void Start()
    {
        scroll = statistics["Scroll"];
        scrollCount = statistics["Scroll Count"] = new Statistic("Scroll Count", Statistic.ValueType.Float, 100f);
        zoomMinimum = statistics["Zoom Minimum"];
        mousePosition = statistics["Mouse Position"];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = mousePosition;
        pos = pos.Multiply(new Vector2(1f / Screen.width, 1f / Screen.height));
        scrollCount.Set(Mathf.Max(0, Mathf.Min(100, scrollCount.Get<float>() + scroll.Get<float>())));
        float jumpRange = Mathf.Max(zoomMinimum.Get<float>(), Environment.jumpRadius * scrollCount.Get<float>() / 100f);
        camera.transform.position = new Vector3(0f, 0f, -2.5f) * jumpRange * 1000f;
        //camera.transform.position += new Vector3(pos.x * 2f - 1f, pos.y * 2f - 1f, 0) * Environment.jumpRadius * 1000f;
        pos = new Vector2(pos.x * 2f - 1f, pos.y * 2f - 1f);
        //Ray ray = Camera.main.ScreenPointToRay((Vector2)mousePosition);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit);
        //camera.transform.position += hit.point;
    }
}
