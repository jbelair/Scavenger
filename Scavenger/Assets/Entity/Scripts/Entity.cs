using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Entity : TemporalBehaviour
{
    public Statistics statistics;
    public bool isCounting = false;

    public virtual void Awake()
    {
        statistics["stat_hull_delay_cur"].Set(statistics["stat_hull_delay"].Get<float>());
        statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>());
        statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>());
        statistics["stat_speed_max"].Set(10000f);
    }

    public virtual void Update()
    {
        Regenerate();
    }

    public virtual void Regenerate()
    {
        if (statistics.Has("stat_hull") && statistics["stat_hull_delay_cur"].Get<float>() <= 0)
        {
            statistics["stat_hull"].Add(statistics["stat_hull_reg"].Get<float>() * Time.deltaTime * timescale);
        }
        else
        {
            statistics["stat_hull_delay_cur"].Add(-Time.deltaTime * timescale);
        }

        if (statistics.Has("stat_armour") && statistics["stat_armour_delay_cur"].Get<float>() <= 0)
        {
            statistics["stat_armour"].Add(statistics["stat_armour_reg"].Get<float>() * Time.deltaTime * timescale);
        }
        else
        {
            statistics["stat_armour_delay_cur"].Add(-Time.deltaTime * timescale);
        }

        if (statistics.Has("stat_shield") && statistics["stat_shield_delay_cur"].Get<float>() <= 0)
        {
            statistics["stat_shield"].Add(statistics["stat_shield_reg"].Get<float>() * Time.deltaTime * timescale);
        }
        else
        {
            statistics["stat_shield_delay_cur"].Add(-Time.deltaTime * timescale);
        }
    }

    public virtual void Damage(DamageModule damage)
    {
        switch (damage.damageFormat)
        {
            case DamageModule.Damage.Hazard:
                break;
            case DamageModule.Damage.Attack:
                statistics["stat_hull_delay_cur"].Set(statistics["stat_hull_delay"].Get<float>());
                statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>());
                statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>());
                break;
            case DamageModule.Damage.Unknown:
                statistics["stat_hull_delay_cur"].Set(statistics["stat_hull_delay"].Get<float>() * 0.5f);
                statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>() * 0.5f);
                statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>() * 0.5f);
                break;
        }
    }
}
