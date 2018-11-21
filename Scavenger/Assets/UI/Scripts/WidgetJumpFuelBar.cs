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
        barText.text = barConsumptionText.text = barEmptyText.text = (Mathf.Round(Environment.JumpFuel * 10f) / 10f).ToString();

        barMask.fillAmount = ((Environment.JumpFuel - Environment.jumpDistance) / Environment.JumpFuelMax);
        barConsumptionMask.fillAmount = (Environment.JumpFuelMax - Environment.JumpFuel + Environment.jumpDistance) / Environment.JumpFuelMax;
        barEmptyMask.fillAmount = (Environment.JumpFuelMax - Environment.JumpFuel) / Environment.JumpFuelMax;

        barConsumptionText.color = barConsumption.color = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.RoundToInt(Environment.jumpDistance / Environment.JumpRadius * 5f))).colour;
        barEmptyText.color = barEmpty.color = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.RoundToInt((1f - Environment.JumpFuel / Environment.JumpFuelMax) * 5f))).colour * new Color(0.25f,0.25f,0.25f,1);
    }
}
