using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSkinSelector : MonoBehaviour
{
    public WidgetSkinGrid grid;
    public EntityRef target;
    public Image border;
    public Transform colours;
    public string skin = "";
    public bool isShield = false;
    public bool isMenu = false;
    public bool isUnlocked = false;
    public bool isDiscovered = false;

    public void Update()
    {
        if (!isMenu)
        {
            if (isShield)
            {
                if (Players.players[0].statistics.Has("shield skin"))
                    Initialise(Skins.Get(Players.players[0].statistics["shield skin"].Get<string>()));
                else
                    Initialise(Skins.Get(InventoryShips.active.activeShips[InventoryShips.active.index].definition.shield));
            }
            else
            {
                if (Players.players[0].statistics.Has("ship skin"))
                    Initialise(Skins.Get(Players.players[0].statistics["ship skin"].Get<string>()));
                else
                    Initialise(Skins.Get(InventoryShips.active.activeShips[InventoryShips.active.index].definition.skin));
            }
        }
    }

    public void Initialise(SkinDefinition.Skin skin)
    {
        if (isShield)
        {
            colours.transform.GetChild(0).GetComponent<Graphic>().color = skin.colours[0];
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                colours.transform.GetChild(i).GetComponent<Graphic>().color = skin.colours[i];
            }
        }

        if (graphics == null)
        {
            graphics = GetComponentsInChildren<Graphic>();
        }

        if (isMenu)
        {
            foreach (Graphic graphic in graphics)
            {
                if (!isUnlocked)
                {
                    if (!isDiscovered)
                    {
                        graphic.color = Color.black;
                    }
                    else
                    {
                        graphic.color = graphic.color.A(graphic.color.a * 0.5f);
                    }
                }
            }
        }
    }

    private Graphic[] graphics;
    public void Initialise(ShipDefinition ship)
    {
        this.skin = isShield ? ship.shield : ship.skin;
        SkinDefinition.Skin skin = Skins.Get(this.skin);

        border.color = Schemes.Scheme(StringHelper.RarityIntToString(skin.oneIn)).colour;

        Initialise(skin);
    }

    public void Hover()
    {
        if (isDiscovered)
        {
            // This is selecting for the player, and should override whatever default skin the ship would have.
            if (target != null)
            {
                if (isShield)
                {
                    target.Entity.statistics["shield skin"].Set(skin);
                    target.Entity.statistics["shield material"].Set(new Material(Materials.materials[Skins.Get(skin).skin]));
                }
                else
                {
                    target.Entity.statistics["ship skin"].Set(skin);
                    target.Entity.statistics["ship material"].Set(new Material(Materials.materials[Skins.Get(skin).skin]));
                }

                if (isMenu)
                {
                    if (isShield)
                        InventoryShips.active.activeShips[InventoryShips.active.index].shield.sharedMaterial = target.Entity.statistics["shield material"].Get<Material>();
                    else
                        InventoryShips.active.activeShips[InventoryShips.active.index].renderer.sharedMaterial = target.Entity.statistics["ship material"].Get<Material>();
                }
            }
        }
    }

    public void Select()
    {
        if (isUnlocked)
        {
            if (target != null && isMenu)
                grid.Clear();
        }
    }
}
