using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SystemsCamera : MonoBehaviour
{
    public CameraNode node;
    public Statistics statistics;

    public ShipDefinition ship;
    public ShipDefinition.Statistic stat_jumpView;
    public Vector3 coordinate;

    public Statistic mousePosition;
    public Statistic scroll;
    public Statistic scrollCount;
    public Statistic zoomMinimum;

    public Vector3 zoomInOffset = new Vector3(0, -1.4f, -1f);
    public Vector3 zoomOutOffset = new Vector3(0, 0, -2f);

    public CameraNode target;

    public SystemsGenerator generator;

    public Vector3 currentVelocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        scroll = statistics["Scroll"];
        scrollCount = statistics["Scroll Count"] = new Statistic("Scroll Count", Statistic.ValueType.Float, 100f);
        zoomMinimum = statistics["Zoom Minimum"];
        mousePosition = statistics["Mouse Position"];

        ship = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value);
        stat_jumpView = ship.statistics.Find(s => s.name == "stat_jump_view");

        coordinate = JsonUtility.FromJson<Vector3>(PlayerSave.Active.Get("system coordinates").value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = mousePosition;
        pos = pos.Multiply(new Vector2(1f / Screen.width, 1f / Screen.height));

        scrollCount.Set(Mathf.Max(0, Mathf.Min(200, scrollCount.Get<float>() + scroll.Get<float>())));

        float jumpRange = Mathf.Max(zoomMinimum.Get<float>(), stat_jumpView.value * scrollCount.Get<float>() / 200f);

        Vector3 position = Environment.systemCoordinatesDepth.OOZ() + coordinate.OOZ() + Vector3.Lerp(zoomInOffset, zoomOutOffset, scrollCount.Get<float>() / 200f) * jumpRange * 1000f;
        node.transform.position = Vector3.SmoothDamp(node.transform.position, position, ref currentVelocity, 0.1f);
        
        //camera.transform.position += new Vector3(pos.x * 2f - 1f, pos.y * 2f - 1f, 0) * Environment.jumpRadius * 1000f;
        //pos = new Vector2(pos.x * 2f - 1f, pos.y * 2f - 1f);
        //Ray ray = Camera.main.ScreenPointToRay((Vector2)mousePosition);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit);
        //camera.transform.position += hit.point;

        if (generator == null)
            generator = SystemsGenerator.active;

        if (generator.systems.ContainsKey(coordinate))
            transform.LookAt(Environment.systemCoordinatesDepth.OOZ(), -Vector3.forward);
    }
}
