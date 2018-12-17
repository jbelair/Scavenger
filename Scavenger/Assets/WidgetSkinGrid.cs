using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSkinGrid : MonoBehaviour
{
    public WidgetSkinSelector prefab;
    public WidgetSkinSelector prefabShield;

    public EntityRef target;
    public List<WidgetSkinSelector> widgets = new List<WidgetSkinSelector>();
    
    public void Initialise(bool isShield)
    {
        List<SkinDefinition.Skin> skins = new List<SkinDefinition.Skin>();
        string unlockedSkins = PlayerSave.Active.Get("unlocked skins").value;
        string discoveredSkins = PlayerSave.Active.Get("discovered skins").value;

        if (isShield)
        {
            foreach(SkinDefinition.Skin skin in Skins.skins.Values)
            {
                if (skin.name.Contains("shield"))
                    skins.Add(skin);
            }
        }
        else
        {
            foreach (SkinDefinition.Skin skin in Skins.skins.Values)
            {
                if (skin.name.Contains("ship"))
                    skins.Add(skin);
            }
        }

        foreach(SkinDefinition.Skin skin in skins)
        {
            WidgetSkinSelector inst = isShield ? Instantiate(prefabShield, transform) : Instantiate(prefab, transform);
            inst.skin = skin.skin;
            inst.isMenu = true;
            inst.isShield = isShield;
            inst.isUnlocked = unlockedSkins.Contains(skin.name);
            inst.isDiscovered = discoveredSkins.Contains(skin.name) || skin.oneIn < 100;
            inst.grid = this;
            inst.target = target;
            inst.Initialise(skin);
            widgets.Add(inst);
        }
    }

    public void Clear()
    {
        foreach(WidgetSkinSelector widget in widgets)
        {
            Destroy(widget.gameObject);
        }

        widgets.Clear();
    }
}
