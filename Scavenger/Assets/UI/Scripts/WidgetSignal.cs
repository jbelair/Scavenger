using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI label;
    public TextMeshProUGUI rarityLabel;
    public TextMeshProUGUI riskLabel;
    public WidgetSignalTags tags;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(type.name);
        //Debug.Log(StringHelper.RarityIntToString(type.oneIn));
        Scheme rarityScheme = Schemes.Scheme(StringHelper.RarityIntToString(type.oneIn));
        //Debug.Log(type.risk);
        Scheme riskScheme = Schemes.Scheme(type.risk);

        foreach(ImageContainer rarity in rarityImages)
        {
            rarity.image.color = rarityScheme.colour * rarity.tint;
        }

        foreach(ImageContainer risk in riskImages)
        {
            risk.image.color = riskScheme.colour * risk.tint;
        }

        foreach (Image icon in iconImages)
        {
            icon.sprite = Schemes.Scheme(type.category).symbol;
        }

        if (label)
        {
            label.text = Literals.active[type.name];
        }

        if (rarityLabel)
        {
            rarityLabel.text = Literals.active[StringHelper.RarityIntToString(type.oneIn)];
            rarityLabel.color = rarityScheme.colour;
        }

        if (riskLabel)
        {
            riskLabel.text = Literals.active[type.risk];
            riskLabel.color = riskScheme.colour;
        }

        if (tags)
        {
            tags.Set(type);
        }
    }

    public void Set(DungeonType dungeonType)
    {
        type = dungeonType;
    }
}
