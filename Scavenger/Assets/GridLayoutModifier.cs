using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutModifier : MonoBehaviour
{
    public GridLayoutGroup grid;

    [Serializable]
    public struct Setting
    {
        public string name;
        public Vector2 cellSize;

        public void Set(GridLayoutGroup grid)
        {
            grid.cellSize = cellSize;
        }
    }
    public List<Setting> settings = new List<Setting>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(string name)
    {
        settings.Find(setting => setting.name == name).Set(grid);
    }

    public void SetCellSize(Vector2 v)
    {
        grid.cellSize = v;
    }
}
