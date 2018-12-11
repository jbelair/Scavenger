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

            Players.players[0].statistics["ship"] = new Statistic("ship", Statistic.ValueType.Object, InventoryShips.active.activeShips[index].definition);

            Players.players[0].statistics["ship material"] = new Statistic("ship material", Statistic.ValueType.String, InventoryShips.active.activeShips[index].definition.skin);
            Players.players[0].statistics["shield material"] = new Statistic("shield material", Statistic.ValueType.String, InventoryShips.active.activeShips[index].definition.shield);

            foreach (ShipDefinition.Statistic stat in InventoryShips.active.activeShips[index].definition.statistics)
            {
                string statistic = StringHelper.RemoveTrailingSpaces(stat.name);
                if (!Players.players[0].statistics.Has(statistic))
                    Players.players[0].statistics[statistic] = new Statistic(statistic, Statistic.ValueType.Float, stat.value);
                else
                    Players.players[0].statistics[statistic].Set(stat.value);
            }

            Players.players[0].statistics["ship value"].Set(InventoryShips.active.activeShips[index].definition.value);
            Players.players[0].statistics["jump view"].Set(InventoryShips.active.activeShips[index].definition.statistics.Find(s => s.name == "stat_jump_view").value);
        }
    }
}
