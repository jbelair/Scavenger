using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetSignalDescription : MonoBehaviour
{
    public DungeonType type;

    public RawImage background;
    public TMP_Text textName;
    public TMP_Text textDescription;

    // Use this for initialization
    void Start()
    {
        if (background)
        {
            background.color = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(type.oneIn)).colour;
        }

        if (textName)
            textName.text = type.name;

        if (textDescription)
            textDescription.text = "Risk: " + type.risk.ToString() + "\n" + "Rarity: " + StringHelper.RarityIntToString(type.oneIn) + "\n\n" + type.description;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(DungeonType dungeonType)
    {
        type = dungeonType;
        //signalName = dungeonType.name;
        //signalDescription = "Scanning...\n" + dungeonType.description;
        //risk = dungeonType.risk;
    }
}
