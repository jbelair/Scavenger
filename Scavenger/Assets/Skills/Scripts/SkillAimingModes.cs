using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SkillAimingModes
{
    public enum AimingFormat
    {
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.cmavt98q6mf8
        /// </summary>
        None,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.upq53p0i72c
        /// </summary>
        Turret,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.yu0wln91efpj
        /// </summary>
        Salvo,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.1zamks50esg8
        /// </summary>
        Fixed,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.fohleoj15py5
        /// </summary>
        Target,
        /// <summary>
        /// https://docs.google.com/document/d/1FAIi3QXjfaQ891FoMCdOS5AyXqLMzY-hTI6uLsCBVA0/edit#heading=h.szmxh0a7bxtw
        /// </summary>
        TargetArea
    };

    public static void Aim(Transform player, Transform spawn, AimingFormat format, Vector3 inputV3, Vector2 inputV2, params Vector2[] salvoAngles)
    {
        switch(format)
        {
            case AimingFormat.None:
                break;
            case AimingFormat.Turret:
                if (inputV2.magnitude > 0)
                {
                    Vector3 aim = inputV2;
                    aim = new Vector3(-aim.x, aim.y);
                    spawn.up = aim;
                }
                else if (inputV3.magnitude > 0)
                {
                    Vector3 aim = inputV3 - player.transform.position;
                    spawn.up = aim;
                }
                break;
            case AimingFormat.Salvo:
                break;
            case AimingFormat.Fixed:
                break;
            case AimingFormat.Target:
                break;
            case AimingFormat.TargetArea:
                break;
        }
    }
}
