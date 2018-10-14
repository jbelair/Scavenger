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
        WidgetScheme.SchemeContainer rarityScheme = WidgetScheme.Scheme(StringHelper.RarityIntToString(type.oneIn));
        WidgetScheme.SchemeContainer riskScheme = WidgetScheme.Scheme(type.risk);

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
            icon.sprite = WidgetScheme.Scheme(type.category).symbol;
        }

        if (label)
        {
            label.text = Literals.literals[Environment.language][type.name];
        }

        if (rarityLabel)
        {
            rarityLabel.text = Literals.literals[Environment.language][StringHelper.RarityIntToString(type.oneIn)];
        }

        if (riskLabel)
        {
            riskLabel.text = Literals.literals[Environment.language][type.risk];
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
