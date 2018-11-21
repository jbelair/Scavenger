using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetSignalDescription : MonoBehaviour
{
    public DungeonType type;

    public RawImage background;
    public Image engage;
    public TMP_Text textName;
    public TMP_Text textDescription;
    public WidgetSignalTagGrid grid;

    // Use this for initialization
    void Start()
    {
        if (type.name == "")
        {
            Destroy(gameObject);
            return;
        }

        Scheme rarityScheme = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString(type.oneIn));

        //if (background)
        //{
        //    background.color = rarityScheme.colour;
        //}

        if (textName)
        {
            textName.text = Literals.active[type.name];
            //textName.color = rarityScheme.colour;
        }

        if (textDescription)
        {
            textDescription.text = Literals.active[type.risk] + "\n" + Literals.active[StringHelper.RarityIntToString(type.oneIn)] + "\n" + Literals.active[type.description];
            //textDescription.color = rarityScheme.colour;
        }

        if (engage)
        {
            Scheme riskScheme = Schemes.Scheme(type.risk);
            //engage.color = riskScheme.colour;// * Color.gray;
        }

        if (grid)
        {
            grid.Set(type);
        }
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
