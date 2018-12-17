using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    public EntityRef target;

    public SkillUEI skill;
    public string skillBinding;
    public Image active;
    public Image background;
    public Image icon;
    public Image cooldown;
    public Graphic[] rarityGraphics;

    private Statistic input;

    private void Start()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (!target.Entity || !target.Entity.statistics)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (!target.Entity.statistics.Has(skillBinding + " uei"))
        {
            skill = Resources.Load<SkillUEI>("Skills/Objects/" + target.Entity.statistics["skill 0"].Get<Skill>().name);
            skill = Instantiate(skill, target.Entity.transform);
            target.Entity.statistics[skillBinding + " uei"].Set(skill); // TODO may want to change this later
        }

        Set(skill);
        yield return null;
    }

    private void Update()
    {
        if (skill)
            cooldown.fillAmount = 1 - (skill.cooldownCurrent / skill.cooldown);
    }

    public void Set(SkillUEI skill)
    {
        this.skill = skill;
        if (skill != null)
        {
            skill.target = target;
            skill.binding = skillBinding;
            skill.statisticInput = skillBinding + " input";
            if (input == null && target.Entity.statistics.Has(skill.statisticInput))
                input = target.Entity.statistics[skill.statisticInput];

            if (input != null && active != null)
                active.gameObject.SetActive(input > 0f);

            Scheme scheme = Schemes.Scheme(StringHelper.RarityIntToString(skill.skill.oneIn));
            background.color = scheme.colour * Color.gray;
            foreach (Graphic graphic in rarityGraphics)
            {
                graphic.color = scheme.colour;
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(skill != null);
        }
    }
}
