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

            if (!Players.players[0].statistics.Has("ship skin"))
                Players.players[0].statistics["ship skin"] = new Statistic("ship skin", Statistic.ValueType.String, InventoryShips.active.activeShips[index].definition.skin);
            else
            {
                if (Players.players[0].statistics["ship skin"].Get<string>() == InventoryShips.active.activeShips[InventoryShips.active.lastIndex].definition.skin)
                    Players.players[0].statistics["ship skin"].Set(InventoryShips.active.activeShips[index].definition.skin);
            }

            if (!Players.players[0].statistics.Has("shield skin"))
                Players.players[0].statistics["shield skin"] = new Statistic("shield skin", Statistic.ValueType.String, InventoryShips.active.activeShips[index].definition.shield);
            else
            {
                if (Players.players[0].statistics["shield skin"].Get<string>() == InventoryShips.active.activeShips[InventoryShips.active.lastIndex].definition.shield)
                    Players.players[0].statistics["shield skin"].Set(InventoryShips.active.activeShips[index].definition.shield);
            }

            InventoryShips.active.activeShips[index].renderer.sharedMaterial = Materials.materials[Skins.Get(Players.players[0].statistics["ship skin"].Get<string>()).skin];
            Players.players[0].statistics["ship material"].Set(InventoryShips.active.activeShips[index].renderer.sharedMaterial);

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
