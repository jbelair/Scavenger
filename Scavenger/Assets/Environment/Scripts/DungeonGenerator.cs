using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public enum Risk { None, Low, Medium, High, Extreme, Fatal };
    public int hash;
    public bool generates = false;
    public DungeonType dungeonType;
    public string dungeonCategory;
    public float riskLevel = 0;
    public Risk risk;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        risk = (Risk)Mathf.FloorToInt(riskLevel);
    }
}
