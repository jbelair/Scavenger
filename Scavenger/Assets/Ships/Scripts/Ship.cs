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
    public bool isMenu = true;
    public WidgetShipSelector widgetShipSelector;
    public WidgetShipSelector widgetShipProgressor;
    public new MeshRenderer renderer;
    public bool isUnlocked = true;
    public bool isDiscovered = true;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();

        if (isMenu)
        {
            node.transform.position = renderer.bounds.size * 0.75f;
            widgetShipSelector = UIManager.active.Button("menu play new grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, new UnityEngine.Events.UnityAction(Select), model.transform).GetComponent<WidgetShipSelector>();
            widgetShipSelector.index = index;

            widgetShipProgressor = UIManager.active.Button("menu progress grid ship", UIManager.Layer.Mid, "widget ship select", Vector2.zero, new UnityEngine.Events.UnityAction(ProgressSelect), model.transform).GetComponent<WidgetShipSelector>();
            widgetShipProgressor.index = index;
            widgetShipProgressor.isSelectingCurrentShip = false;
        }
    }

    List<Graphic> graphics;
    // Update is called once per frame
    void LateUpdate()
    {
        if (isMenu)
        {
            widgetShipSelector.gameObject.SetActive(isUnlocked);

            if (graphics == null)
            {
                graphics = new List<Graphic>(widgetShipProgressor.gameObject.GetComponentsInChildren<Graphic>());
            }

            if (InventoryShips.active.activeMode.showLocked && !isUnlocked)
            {
                foreach (Graphic graphic in graphics)
                {
                    graphic.color = graphic.color.A(0.5f);
                }
            }
        }
        else
        {
            if (model == null)
            {
                if (definition.name == "")
                    definition = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value);

                model = Instantiate(Resources.Load<GameObject>("Ships/Models/" + definition.name), Vector3.zero, Quaternion.Euler(0, 0, 180), transform);

                renderer = model.GetComponentInChildren<MeshRenderer>();
                renderer.sharedMaterial = Materials.materials[Skins.Get(Players.players[0].statistics["ship material"]).skin];
            }
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
        UIManager.active.AddScreen("menu progress ship");
        UIManager.active.RemoveScreen("menu progress grid ship");
    }
}
