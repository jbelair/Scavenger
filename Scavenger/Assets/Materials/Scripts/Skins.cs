using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Skins
{
    public static Dictionary<string, SkinDefinition.Skin> skins = null;

    static Skins()
    {
        Load();
    }

    public static void Load()
    {
        if (skins == null)
        {
            skins = new Dictionary<string, SkinDefinition.Skin>();

            TextAsset[] txts = Resources.LoadAll<TextAsset>("Skins/");
            foreach (TextAsset txt in txts)
            {
                SkinDefinition def = JsonUtility.FromJson<SkinDefinition>(txt.text);
                foreach (SkinDefinition.Skin skin in def.skins)
                {
                    skins.Add(skin.name, skin);

                    Material mat = Materials.materials[skin.skin];
                    if (mat.name.Contains("ship"))
                    {
                        skin.colours = new Color[8];
                        skin.colours[0] = mat.GetColor("_Col100").A(1);
                        skin.colours[1] = mat.GetColor("_Col75").A(1);
                        skin.colours[2] = mat.GetColor("_Col50").A(1);
                        skin.colours[3] = mat.GetColor("_Col25").A(1);
                        skin.colours[4] = mat.GetColor("_Col0").A(1);
                        skin.colours[5] = mat.GetColor("_GlowR").Vector3().normalized.RGB().A(1);
                        skin.colours[6] = mat.GetColor("_GlowG").Vector3().normalized.RGB().A(1);
                        skin.colours[7] = mat.GetColor("_GlowB").Vector3().normalized.RGB().A(1);
                    }
                    else if (mat.name.Contains("shield"))
                    {
                        skin.colours = new Color[1];
                        skin.colours[0] = mat.GetColor("_TintColor").Vector3().normalized.RGB().A(1);
                    }
                }
            }
        }
    }

    public static SkinDefinition.Skin Get(string name)
    {
        Load();

        if (skins.ContainsKey(name))
        {
            return skins[name];
        }
        else
            return skins["default"];
    }
}
