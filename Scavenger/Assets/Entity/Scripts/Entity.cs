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
        statistics["stat_hull_reg_cur"].Set(statistics["stat_hull_reg"].Get<float>());
        statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>());
        statistics["stat_armour_reg_cur"].Set(statistics["stat_armour_reg"].Get<float>());
        statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>());
        statistics["stat_shield_reg_cur"].Set(statistics["stat_shield_reg"].Get<float>());
        statistics["stat_speed_max"].Set(10000f);
    }

    public virtual void Update()
    {
        Regenerate();
    }

    public virtual void Regenerate()
    {
        if (statistics.Has("stat_hull"))
        {
            if (statistics["stat_hull_delay_cur"].Get<float>() <= 0 && statistics["stat_hull"].Get<float>() < statistics["stat_hull_max"].Get<float>() * 0.5f)
                statistics["stat_hull_reg_cur"].Add(statistics["stat_hull_reg"].Get<float>() * (Time.deltaTime / statistics["stat_hull_delay"].Get<float>()));
            else
                statistics["stat_hull_reg_cur"].Set(Mathf.Lerp(statistics["stat_hull_reg_cur"], statistics["stat_hull_reg"].Get<float>(), 0.1f));

            statistics["stat_hull_reg_cur"].Max(statistics["stat_hull_reg"].Get<float>() * 10f);
            statistics["stat_hull"].Add(statistics["stat_hull_reg_cur"].Get<float>() * Time.deltaTime * timescale);
        }
        statistics["stat_hull_delay_cur"].Add(-Time.deltaTime * timescale);

        if (statistics.Has("stat_armour"))
        {
            if (statistics["stat_armour_delay_cur"].Get<float>() <= 0 && statistics["stat_armour"].Get<float>() < statistics["stat_armour_max"].Get<float>() * 0.5f)
                statistics["stat_armour_reg_cur"].Add(statistics["stat_armour_reg"].Get<float>() * (Time.deltaTime / statistics["stat_armour_delay"].Get<float>()));
            else
                statistics["stat_armour_reg_cur"].Set(Mathf.Lerp(statistics["stat_armour_reg_cur"], statistics["stat_armour_reg"].Get<float>(), 0.1f));

            statistics["stat_armour_reg_cur"].Max(statistics["stat_armour_reg"].Get<float>() * 10f);
            statistics["stat_armour"].Add(statistics["stat_armour_reg_cur"].Get<float>() * Time.deltaTime * timescale);
        }
        statistics["stat_armour_delay_cur"].Add(-Time.deltaTime * timescale);

        if (statistics.Has("stat_shield"))
        {
            if (statistics["stat_shield_delay_cur"].Get<float>() <= 0 && statistics["stat_shield"].Get<float>() < statistics["stat_shield_max"].Get<float>() * 0.5f)
                statistics["stat_shield_reg_cur"].Add(statistics["stat_shield_reg"].Get<float>() * (Time.deltaTime / statistics["stat_shield_delay"].Get<float>()));
            else
                statistics["stat_shield_reg_cur"].Set(Mathf.Lerp(statistics["stat_shield_reg_cur"], statistics["stat_shield_reg"].Get<float>(), 0.1f));

            statistics["stat_shield_reg_cur"].Max(statistics["stat_shield_reg"].Get<float>() * 10f);
            statistics["stat_shield"].Add(statistics["stat_shield_reg_cur"].Get<float>() * Time.deltaTime * timescale);
        }
        statistics["stat_shield_delay_cur"].Add(-Time.deltaTime * timescale);
    }

    public virtual void Damage(DamageModule damage)
    {
        switch (damage.damageFormat)
        {
            case DamageModule.Damage.Hazard:
                break;
            case DamageModule.Damage.Attack:
                statistics["stat_hull_delay_cur"].Set(statistics["stat_hull_delay"].Get<float>());
                statistics["stat_hull_reg_cur"].Set(statistics["stat_hull_reg"].Get<float>());
                statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>());
                statistics["stat_armour_reg_cur"].Set(statistics["stat_armour_reg"].Get<float>());
                statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>());
                statistics["stat_shield_reg_cur"].Set(statistics["stat_shield_reg"].Get<float>());
                break;
            case DamageModule.Damage.Unknown:
                statistics["stat_hull_delay_cur"].Set(statistics["stat_hull_delay"].Get<float>() * 0.5f);
                statistics["stat_hull_reg_cur"].Set(Mathf.Lerp(statistics["stat_hull_reg_cur"], statistics["stat_hull_reg"].Get<float>(), 0.5f));
                statistics["stat_armour_delay_cur"].Set(statistics["stat_armour_delay"].Get<float>() * 0.5f);
                statistics["stat_armour_reg_cur"].Set(Mathf.Lerp(statistics["stat_armour_reg_cur"], statistics["stat_armour_reg"].Get<float>(), 0.5f));
                statistics["stat_shield_delay_cur"].Set(statistics["stat_shield_delay"].Get<float>() * 0.5f);
                statistics["stat_shield_reg_cur"].Set(Mathf.Lerp(statistics["stat_shield_reg_cur"], statistics["stat_shield_reg"].Get<float>(), 0.5f));
                break;
        }
    }
}
