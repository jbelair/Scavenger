using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipDefinition definition;
    public GameObject model;
    public CameraNode node;
    public int index;
    public WidgetShipSelector widgetShipSelector;
    public WidgetShipSelector widgetShipProgressor;
    public new MeshRenderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        node.transform.position = renderer.bounds.size*0.75f;//.Multiply(new Vector3(-1,-1,1));
        widgetShipSelector = UIManager.active.Button("menu play new grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, new UnityEngine.Events.UnityAction(Select), model.transform).GetComponent<WidgetShipSelector>();
        widgetShipSelector.index = index;

        widgetShipProgressor = UIManager.active.Button("menu progress grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, null, model.transform).GetComponent<WidgetShipSelector>();
        widgetShipProgressor.index = index;
        widgetShipProgressor.isSelectingCurrentShip = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Select()
    {
        InventoryShips.active.Set(index, true);
        InventoryShips.active.Set("Stack");
        UIManager.active.AddScreen("menu play new");
        UIManager.active.RemoveScreen("menu play new grid ship");
    }
}
