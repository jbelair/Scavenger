using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetFilter : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI label;
    public Image labelShadow;
    public Button button;
    public WidgetFilters filters;

    public string filter;
    public bool isActive = false;
    public bool isSelected = false;
    public string selected = "selected";
    public string deselected = "deselected";
    private Scheme schemeSelected;
    private Scheme schemeDeselected;

    // Use this for initialization
    void Start()
    {
        schemeSelected = Schemes.Scheme(selected);
        schemeDeselected = Schemes.Scheme(deselected);

        isActive = SystemsFilter.active.filter == filter;

        Refresh();
    }

    public void Set()
    {
        SystemsFilter.active.Set(filter);
        filters.Reset();
    }

    public void Select(bool select)
    {
        if (select != isSelected)
        {
            isSelected = select;

            isActive = SystemsFilter.active.filter == filter;

            if (select)
                SystemsFilter.active.Set(filter);

            filters.Reset();

            Refresh();
        }
    }

    public void Refresh()
    {
        if (isSelected || isActive)
        {
            icon.sprite = schemeSelected.symbol;
            icon.color = schemeSelected.colour;

            label.gameObject.SetActive(true);
            labelShadow.gameObject.SetActive(true);
        }
        else
        {
            icon.sprite = schemeDeselected.symbol;
            icon.color = schemeDeselected.colour;

            label.gameObject.SetActive(false);
            labelShadow.gameObject.SetActive(false);
        }
    }
}
