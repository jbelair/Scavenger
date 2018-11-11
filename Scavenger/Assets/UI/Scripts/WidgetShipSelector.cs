using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetShipSelector : MonoBehaviour
{
    public int index;
    public bool isSelectingCurrentShip = true;

    public void Set()
    {
        InventoryShips.active.Set(index, false);
        if (isSelectingCurrentShip)
        {
            PlayerSave.Active().Add("ship", JsonUtility.ToJson(InventoryShips.active.ships[index].definition));
            Players.players[0].statistics["ship value"].Set(InventoryShips.active.ships[index].definition.value);
        }
    }
}
