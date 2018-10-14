﻿using System.Collections;
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
        WidgetScheme.SchemeContainer rarityScheme = WidgetScheme.Scheme("Rarity " + StringHelper.RarityIntToString(type.oneIn));

        //if (background)
        //{
        //    background.color = rarityScheme.colour;
        //}

        if (textName)
        {
            textName.text = Literals.literals[Environment.language][type.name];
            //textName.color = rarityScheme.colour;
        }

        if (textDescription)
        {
            textDescription.text = Literals.literals[Environment.language][type.risk] + "\n" + Literals.literals[Environment.language][StringHelper.RarityIntToString(type.oneIn)] + "\n" + Literals.literals[Environment.language][type.description];
            //textDescription.color = rarityScheme.colour;
        }

        if (engage)
        {
            WidgetScheme.SchemeContainer riskScheme = WidgetScheme.Scheme(type.risk);
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
