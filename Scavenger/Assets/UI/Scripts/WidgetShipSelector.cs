using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetShipSelector : MonoBehaviour
{
    public int index;

    public void Set()
    {
        InventoryShips.active.Set(index, false);
    }
}
