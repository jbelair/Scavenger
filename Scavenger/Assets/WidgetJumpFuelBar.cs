using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetJumpFuelBar : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public Image barMask;
    public Image bar;
    public TextMeshProUGUI barConsumptionText;
    public Image barConsumptionMask;
    public Image barConsumption;
    public TextMeshProUGUI barEmptyText;
    public Image barEmptyMask;
    public Image barEmpty;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        barText.text = barConsumptionText.text = barEmptyText.text = (Mathf.Round(Environment.jumpFuel * 10f) / 10f).ToString();

        barMask.fillAmount = ((Environment.jumpFuel - Environment.jumpDistance) / Environment.jumpFuelMax);
        barConsumptionMask.fillAmount = (Environment.jumpFuelMax - Environment.jumpFuel + Environment.jumpDistance) / Environment.jumpFuelMax;
        barEmptyMask.fillAmount = (Environment.jumpFuelMax - Environment.jumpFuel) / Environment.jumpFuelMax;

        barConsumptionText.color = barConsumption.color = WidgetScheme.Scheme(StringHelper.RiskIntToString(Mathf.RoundToInt(Environment.jumpDistance / Environment.jumpRadius * 5f))).colour;
        barEmptyText.color = barEmpty.color = WidgetScheme.Scheme(StringHelper.RiskIntToString(Mathf.RoundToInt((1f - Environment.jumpFuel / Environment.jumpFuelMax) * 5f))).colour * new Color(0.25f,0.25f,0.25f,1);
    }
}
