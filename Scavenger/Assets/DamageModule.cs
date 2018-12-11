using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModule : MonoBehaviour
{
    public enum Format { Normal, Percentage };
    public enum Damage { Hazard, Attack, Unknown };
    public StatisticUEI damage;
    public Damage damageFormat = Damage.Unknown;
    public Format format = Format.Normal;
    public float hullMultiplier = 1;
    public float armourPenetration;
    public float armourMultiplier = 1;
    public float shieldPenetration;
    public float shieldMultiplier = 1;
    //public StatisticModifier[] modifiers;
    public EntityRef target;
    private MinMaxStatistic hull;
    private MinMaxStatistic armour;
    private MinMaxStatistic shield;

    private Statistic dmg;

    public void Do()
    {
        if (target != null)
        {
            if (hull == null)
                hull = target.Entity.statistics["hull"].Get<object>() as MinMaxStatistic;
            if (hull == null)
                hull = new MinMaxStatistic() { value = target.Entity.statistics["stat_hull"], maximum = target.Entity.statistics["stat_hull_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) };
            if (armour == null)
                armour = target.Entity.statistics["armour"].Get<object>() as MinMaxStatistic;
            if (armour == null)
                armour = new MinMaxStatistic() { value = target.Entity.statistics["stat_armour"], maximum = target.Entity.statistics["stat_armour_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) };
            if (shield == null)
                shield = target.Entity.statistics["shield"].Get<object>() as MinMaxStatistic;
            if (shield == null)
                shield = new MinMaxStatistic() { value = target.Entity.statistics["stat_shield"], maximum = target.Entity.statistics["stat_shield_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) };
            if (dmg == null)
                dmg = damage.Initialise();

            float toShield = dmg.Get<float>() * (1 - shieldPenetration);
            float perShield = (float)shield.Value / shield.Maximum;
            if (float.IsNaN(perShield))
                perShield = 0;
            if (toShield > perShield)
                toShield = perShield;
            float toArmour = (dmg.Get<float>() - toShield) * (1 - armourPenetration);
            float perArmour = (float)armour.Value / armour.Maximum;
            if (float.IsNaN(perArmour))
                perArmour = 0;
            if (toArmour > perArmour)
                toArmour = perArmour;
            float toHull = dmg.Get<float>() - toArmour - toShield;

            toHull *= hullMultiplier * Time.deltaTime * target.Entity.timescale;
            toArmour *= armourMultiplier * Time.deltaTime * target.Entity.timescale;
            toShield *= shieldMultiplier * Time.deltaTime * target.Entity.timescale;

            if (format == Format.Percentage)
            {
                toHull *= hull.Maximum;
                toArmour *= armour.Maximum;
                toShield *= shield.Maximum;
            }
            //else
            //{
            //    //if (toShield > shield.Value.Get<float>())
            //    //{
            //    //    toArmour += toShield - shield.Value.Get<float>();
            //    //    toShield = shield.Value.Get<float>();
            //    //}

            //    //if (toArmour > armour.Value.Get<float>())
            //    //{
            //    //    toHull += toArmour - armour.Value.Get<float>();
            //    //    toArmour = armour.Value.Get<float>();
            //    //}
            //}

            shield.Value.Add(-toShield);
            armour.Value.Add(-toArmour);
            hull.Value.Add(-toHull);

            if (target.Entity.isCounting)
            {
                target.Entity.statistics["count_damage_all"].Add(dmg.Get<float>());
                target.Entity.statistics["count_damage_hull"].Add(toHull);
                target.Entity.statistics["count_damage_armour"].Add(toArmour);
                target.Entity.statistics["count_damage_shield"].Add(toShield);
            }

            target.Entity.Damage(this);
        }
    }
}
