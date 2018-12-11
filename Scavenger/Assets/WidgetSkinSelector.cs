using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSkinSelector : MonoBehaviour
{
    public EntityRef target;
    public Image border;
    public Transform colours;
    public string skin = "";
    public bool isShield = false;

    public void Initialise(ShipDefinition ship)
    {
        SkinDefinition.Skin skin = (isShield) ? Skins.Get(ship.shield) : Skins.Get(ship.skin);
        this.skin = skin.name;

        border.color = Schemes.Scheme(StringHelper.RarityIntToString(skin.oneIn)).colour;

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
    }
}
