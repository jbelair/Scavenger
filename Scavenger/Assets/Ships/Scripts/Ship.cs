using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public ShipDefinition definition;
    public GameObject model;
    public CameraNode node;
    public int index;
    public WidgetShipSelector widgetShipSelector;
    public WidgetShipSelector widgetShipProgressor;
    public new MeshRenderer renderer;
    public bool isUnlocked = true;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        node.transform.position = renderer.bounds.size*0.75f;//.Multiply(new Vector3(-1,-1,1));
        widgetShipSelector = UIManager.active.Button("menu play new grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, new UnityEngine.Events.UnityAction(Select), model.transform).GetComponent<WidgetShipSelector>();
        widgetShipSelector.index = index;
        widgetShipSelector.gameObject.SetActive(isUnlocked);

        widgetShipProgressor = UIManager.active.Button("menu progress grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, new UnityEngine.Events.UnityAction(ProgressSelect), model.transform).GetComponent<WidgetShipSelector>();
        widgetShipProgressor.index = index;
        widgetShipProgressor.isSelectingCurrentShip = false;
    }

    List<Graphic> graphics;

    // Update is called once per frame
    void LateUpdate()
    {
        if (graphics == null)
        {
            graphics = new List<Graphic>(GetComponentsInChildren<Graphic>());
        }

        foreach (Graphic graphic in graphics)
        {
            if (!isUnlocked)
                graphic.color = graphic.color.A(0.5f);
        }
    }

    void Select()
    {
        InventoryShips.active.Set(index, true);
        InventoryShips.active.Set("Stack");
        UIManager.active.AddScreen("menu play new");
        UIManager.active.RemoveScreen("menu play new grid ship");
    }

    void ProgressSelect()
    {
        InventoryShips.active.Set(index, true);
        InventoryShips.active.Set("Stack All");
        UIManager.active.AddScreen("menu progress grid ship fullscreen");
        UIManager.active.RemoveScreen("menu progress grid ship");
    }
}
