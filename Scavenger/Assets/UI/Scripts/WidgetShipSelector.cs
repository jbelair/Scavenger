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
            PlayerSave.Active.Add("ship", JsonUtility.ToJson(InventoryShips.active.activeShips[index].definition));
            Players.players[0].statistics["ship value"].Set(InventoryShips.active.activeShips[index].definition.value);
            Players.players[0].statistics["jump view"].Set(InventoryShips.active.activeShips[index].definition.statistics.Find(s => s.name == "stat_jump_view").value);
            Players.players[0].statistics["stat_jump_fuel"].Set(InventoryShips.active.activeShips[index].definition.statistics.Find(s => s.name == "stat_jump_fuel").value);
            Players.players[0].statistics["stat_jump_fuel_cur"].Set(InventoryShips.active.activeShips[index].definition.statistics.Find(s => s.name == "stat_jump_fuel").value);
            Players.players[0].statistics["stat_jump_range"].Set(InventoryShips.active.activeShips[index].definition.statistics.Find(s => s.name == "stat_jump_range").value);
        }
    }
}
