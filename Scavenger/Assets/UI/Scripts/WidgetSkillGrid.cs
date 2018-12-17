using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSkillGrid : MonoBehaviour
{
    public UIScreen screen;
    public string widgetName = "widget skill display grid";
    public TextMeshProUGUI focusDescription;
    public RectTransform viewport;
    public RectTransform window;
    public GridLayoutGroup grid;
    public RectTransform gridRect;
    public RectTransform focus;
    public int index = 0;
    public float time = 0.1f;
    public float offsetY = 24;
    public Vector3 velocity;
    public float yOffset = 0;
    public float yGoal = 0;
    public Rect viewRectDiagnostic;
    public Rect gridRectDiagnostic;
    public bool isSelectingCurrentSkills = true;
    public string selectingScreen;

    public List<WidgetSkillDisplay> skills = new List<WidgetSkillDisplay>();

    // Use this for initialization
    void Start()
    {
        screen = GetComponentInParent<UIScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skills.Count < 1)
            Set();

        if (focus)
        {
            //yGoal = Mathf.Clamp(-focus.position.y - offsetY, 0, self.rect.height / 2f - startPosition.y / 2f);//, 0, self.rect.height - startPosition.y * 2f);
            //self.position = Vector3.SmoothDamp(self.position, new Vector3(self.position.x, yGoal, self.position.z) + startPosition, ref velocity, time);
            //yOffset = self.position.y;
            viewRectDiagnostic = viewport.rect;
            gridRectDiagnostic = gridRect.rect;
            yGoal = Mathf.Clamp(-(focus.localPosition.y + grid.padding.top) - viewport.rect.height / 2f, 0, Mathf.Max(0, gridRect.rect.height - viewport.rect.height));
            Vector3 target = new Vector3(window.rect.width / 2f, yGoal, 0);// -(focus.localPosition).OYO() + ((float)).XOO() - ((float)grid.padding.top).OYO() + startPosition;
            // TODO resolve this gross hack:
            // This will resolve the issue I am having where the skill grid in progress though literally a direct copy of the same skill grid in new game behaves differently.
            // It is offset up the viewports height, same as the new game grid, except this results in the skills being place across the top of the screen in progress, and the correct behaviour in new game.
            // If however in progress I do not add the viewport height it works as expected.
            if (isSelectingCurrentSkills)
                target += Vector3.up * viewport.rect.height / 2f;
            grid.transform.localPosition = Vector3.SmoothDamp(grid.transform.localPosition, target, ref velocity, time);//Vector3.SmoothDamp(scrollRect.content.localPosition, (scrollRect.viewport.localPosition + focus.localPosition).Multiply(-Vector3.up), ref velocity, time);
            yOffset = grid.transform.localPosition.y;
        }
    }

    public void Set(int i)
    {
        index = i;
        focus = skills[index].self;
        string disabled = "<color=#" + ColorUtility.ToHtmlStringRGB(Schemes.Scheme("disabled").colour) + ">???";
        string rarity = StringHelper.RarityIntToString(skills[index].definition.oneIn);
        string rarityHex = ColorUtility.ToHtmlStringRGBA(Schemes.Scheme(rarity).colour);
        string riskHex = ColorUtility.ToHtmlStringRGBA(Schemes.Scheme(skills[index].definition.risk).colour);
        focusDescription.SetText(skills[index].isUnlocked ? Literals.active[skills[index].definition.name] + 
            "\n\n<color=#" + rarityHex + ">" + Literals.active[rarity] + 
            "\n<color=#" + riskHex + ">" + Literals.active[skills[index].definition.risk] + 
            "\n\n<color=#" + ColorUtility.ToHtmlStringRGBA(Schemes.Scheme("default").colour) + ">" + Literals.active[skills[index].definition.description] : disabled);
    }

    public void Set()
    {
        if (Skills.skills.Count > 0)
        {
            int i = 0;
            foreach (Skill skill in Skills.skills.Values)
            {
                bool unlocked = PlayerSave.Active.Get("unlocked skills").value.Contains(skill.name + " ");
                bool discovered = PlayerSave.Active.Get("discovered skills").value.Contains(skill.name + " ");

                if (!isSelectingCurrentSkills || unlocked)
                {
                    WidgetSkillDisplay widget;
                    if (isSelectingCurrentSkills)
                        widget = UIManager.active.Button(screen.name, UIManager.Layer.Mid, widgetName, Vector2.zero, new UnityEngine.Events.UnityAction(Select)).GetComponent<WidgetSkillDisplay>();
                    else
                        widget = UIManager.active.Button(screen.name, UIManager.Layer.Mid, widgetName, Vector2.zero).GetComponent<WidgetSkillDisplay>();

                    widget.isSelectingCurrentSkill = isSelectingCurrentSkills;
                    widget.isUnlocked = unlocked;
                    widget.isDiscovered = discovered;
                    widget.transform.SetParent(grid.transform);
                    widget.definition = skill;
                    widget.index = i;
                    widget.grid = this;
                    skills.Add(widget);
                    i++;
                }
            }
        }

        SortByValue(true);
    }

    public void Screen(string screen)
    {
        selectingScreen = screen;
    }

    public void Select()
    {
        UIManager.active.AddScreen(selectingScreen);
        UIManager.active.RemoveScreen("menu play new grid skill");
    }

    public void ProgressSelect()
    {
        
    }
    
    public void Search(string searchString)
    {
        foreach(WidgetSkillDisplay skill in skills)
        {
            skill.gameObject.SetActive(Literals.active[skill.definition.name].Contains(searchString));
        }
    }

    void Sort(float valueA, float valueB, bool smallestToLargest, int indexA, int indexB)
    {
        if (smallestToLargest)
        {
            if (valueB < valueA)
            {
                WidgetSkillDisplay temp = skills[indexA];
                skills[indexA] = skills[indexB];
                skills[indexB] = temp;
            }
        }
        else
        {
            if (valueB > valueA)
            {
                WidgetSkillDisplay temp = skills[indexB];
                skills[indexB] = skills[indexA];
                skills[indexA] = temp;
            }
        }
        skills[indexA].index = indexA;
        skills[indexB].index = indexB;
        skills[indexA].transform.SetSiblingIndex(indexA);
        skills[indexB].transform.SetSiblingIndex(indexB);
    }

    public void SortByValue(bool smallestToLargest)
    {
        for(int i = 0; i < skills.Count; i++)
        {
            for (int j = i; j < skills.Count; j++)
            {
                Sort(skills[i].definition.value, skills[j].definition.value, smallestToLargest, i, j);
            }
        }
    }

    public void SortByRarity(bool smallestToLargest)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = i; j < skills.Count; j++)
            {
                Sort(skills[i].definition.oneIn, skills[j].definition.oneIn, smallestToLargest, i, j);
            }
        }
    }

    public void SortByRisk(bool smallestToLargest)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = i; j < skills.Count; j++)
            {
                Sort(FloatHelper.RiskStringToFloat(skills[i].definition.risk), FloatHelper.RiskStringToFloat(skills[j].definition.risk), smallestToLargest, i, j);
            }
        }
    }
}
