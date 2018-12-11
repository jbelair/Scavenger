using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class WidgetShipMenuDisplay : MonoBehaviour
{
    public WidgetStatistic statisticPrefab;

    public ShipDefinition ship;
    public bool isInitialised = false;

    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;
    public WidgetSkinSelector skin;
    public WidgetSkinSelector shield;
    public WidgetStatistic statValue;
    public WidgetStatistic statFuel;
    public WidgetStatistic statRange;
    public WidgetStatistic statSight;
    public GameObject stats;

    public List<WidgetStatistic> statistics = new List<WidgetStatistic>();

    private string lastDescription = "";

    public void Initialise()
    {
        statistics.Clear();

        Ship active = InventoryShips.active.activeShips[InventoryShips.active.index];
        string disabled = "<color=#" + ColorUtility.ToHtmlStringRGB(Schemes.Scheme("disabled").colour) + ">???";
        textName.text = active.isDiscovered ? Literals.active[ship.name] : disabled;
        string risk = Literals.active[ship.risk];
        Scheme riskScheme = Schemes.Scheme(ship.risk);
        string rarity = StringHelper.RarityIntToString(ship.oneIn);
        Scheme rarityScheme = Schemes.Scheme(rarity);
        textDesc.SetText(active.isDiscovered ? "<color=#" + ColorUtility.ToHtmlStringRGB(rarityScheme.colour) + ">" + Literals.active[rarity] +
            "\n<color=#" + ColorUtility.ToHtmlStringRGB(riskScheme.colour) + ">" + risk +
            "\n\n<color=#fff>" + Literals.active[ship.description] : disabled);
        lastDescription = textDesc.text;
        statValue.statistic_ref = "stat_matter";
        statValue.value.SetText(active.isUnlocked ? ship.value.ToString() : disabled);
        statValue.Initialise();

        skin.isShield = false;
        skin.Initialise(ship);
        shield.isShield = true;
        shield.Initialise(ship);

        foreach (ShipDefinition.Statistic stat in ship.statistics)
        {
            if (stat.name == "stat_jump_fuel")
            {
                statFuel.statistic_ref = "stat_jump_fuel";
                statFuel.value.SetText(active.isUnlocked ? stat.value.ToString() : disabled);
                statFuel.Initialise();
            }
            else if (stat.name == "stat_jump_range")
            {
                statRange.statistic_ref = "stat_jump_range";
                statRange.value.SetText(active.isUnlocked ? stat.value.ToString() : disabled);
                statRange.Initialise();
            }
            else if (stat.name == "stat_jump_view")
            {
                statSight.statistic_ref = "stat_jump_view";
                statSight.value.SetText(active.isUnlocked ? stat.value.ToString() : disabled);
                statSight.Initialise();
            }
            else if (stat.name == "stat_hull" || stat.name == "stat_armour" || stat.name == "stat_shield")
            {
                continue;
            }
            else if (active.isUnlocked && stat.name.Contains("mult"))
            {
                if (stat.value != 1)
                {
                    WidgetStatistic statWidget = Instantiate(statisticPrefab, stats.transform);
                    statWidget.statistic_ref = stat.name;
                    statWidget.value.SetText(stat.value + StringHelper.Units(stat.unit));
                    statistics.Add(statWidget);
                }
            }
            else
            {
                if (active.isDiscovered && stat.value != 0)
                {
                    WidgetStatistic statWidget = Instantiate(statisticPrefab, stats.transform);
                    statWidget.statistic_ref = stat.name;
                    statWidget.value.SetText(stat.value + StringHelper.Units(stat.unit));
                    statistics.Add(statWidget);
                }
            }
        }

        isInitialised = true;
    }

    public void Refresh()
    {
        foreach (WidgetStatistic stats in statistics)
        {
            DestroyImmediate(stats.gameObject);
        }

        isInitialised = false;
    }

    private ShipDefinition lastShip;
    // Update is called once per frame
    void Update()
    {
        if (InventoryShips.active && InventoryShips.active.activeShips.Count > 0)
            ship = InventoryShips.active.activeShips[InventoryShips.active.index].definition;

        if (ship.name != "" && !isInitialised)
            Initialise();

        if (lastShip.name != ship.name)
            Refresh();

        textDesc.SetText(lastDescription);
        lastShip = ship;
    }
}
