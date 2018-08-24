using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSignal : MonoBehaviour
{
    [System.Serializable]
    public class ImageContainer
    {
        public Image image;
        public Color tint;
    }

    public DungeonType type;

    public ImageContainer[] rarityImages;
    public ImageContainer[] riskImages;
    public Image[] iconImages;
    //public Image selected;
    //public Text text;

    // Use this for initialization
    void Start()
    {
        WidgetScheme.SchemeContainer rarityScheme = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(type.oneIn));
        WidgetScheme.SchemeContainer riskScheme = WidgetScheme.active.Scheme("Risk " + type.risk.ToString());

        foreach(ImageContainer rarity in rarityImages)
        {
            rarity.image.color = rarityScheme.colour * rarity.tint;
            //rarity.GetComponent<RectTransform>().localScale = Vector3.one * (1 + (int)type.risk / 2f);
        }

        foreach(ImageContainer risk in riskImages)
        {
            risk.image.color = riskScheme.colour * risk.tint;
        }

        foreach (Image icon in iconImages)
        {
            icon.sprite = WidgetScheme.active.Scheme(type.category).symbol;
        }

        //if (selected)
        //    selected.color = rarityScheme.colour;

        //if (text)
        //{
        //    text.color = rarityScheme.colour;
        //    text.text = StringHelper.RiskToString(type.risk);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(DungeonType dungeonType)
    {
        type = dungeonType;
    }
}
