using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShips : MonoBehaviour
{
    public static InventoryShips active;

    [System.Serializable]
    public class Mode
    {
        public string name;
        public Dimension X;
        public Dimension Y;
        public Dimension Z;

        public Vector3 rotation;
        public bool moveToFocus = true;
        public bool clampX = false;
        public bool clampY = false;
        public bool clampZ = false;
        public bool focused = false;
        public bool showLocked = false;
    }

    [System.Serializable]
    public class Dimension
    {
        public float scale = 0;
        public int maximum = -1;
    }

    public Ship shipPrefab;

    public List<Mode> modes;
    public Mode activeMode;
    public Transform focus;
    public Transform lastFocus;
    public int index;

    public Vector3 velocity;
    public float smoothSpeed = 1;

    public List<Ship> ships = new List<Ship>();
    public List<Ship> activeShips = new List<Ship>();

    // Use this for initialization
    void Start()
    {
        active = this;

        if (modes.Count > 0)
        {
            activeMode = modes[0];

            if (ships == null || ships.Count == 0)
            {
                int i = 0;
                foreach (KeyValuePair<string, GameObject> shipPair in Ships.ships)
                {
                    Ship ship = Instantiate(shipPrefab, transform);
                    ships.Add(ship);
                    ship.name = shipPair.Key;
                    ship.index = i;
                    ship.definition = Ships.definitions[ship.name];
                    ship.model = Instantiate(shipPair.Value, ship.transform);
                    ship.model.transform.localPosition = Vector3.zero;
                    ship.model.transform.rotation = new Quaternion();
                    MeshRenderer renderer = ship.model.GetComponentInChildren<MeshRenderer>();
                    renderer.sharedMaterial = Materials.materials[ship.definition.material];//new Material(Materials.materials[ship.definition.material]);

                    if (i == 0)
                    {
                        Camera.main.GetComponent<MoveTo>().AddFrame(ship.node.transform, 1, false, 0);
                    }
                    i++;
                }
            }

            Set();
            Set(index, activeMode.moveToFocus);
        }
    }

    private Mode lastMode;
    private Transform _lastFocus_focus;
    private Transform _lastFocus_lastFocus;
    // Update is called once per frame
    void Update()
    {
        if (focus && activeMode.moveToFocus)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, -focus.localPosition, ref velocity, smoothSpeed).Multiply(new Vector3(activeMode.clampX ? 0 : 1, activeMode.clampY ? 0 : 1, activeMode.clampZ ? 0 : 1));
        }
        else
            transform.localPosition = Vector3.zero;

        if (focus && activeMode.rotation.magnitude > 0)
            focus.Rotate(activeMode.rotation * Time.deltaTime);
        //else
        //    transform.rotation = new Quaternion();// Quaternion.Lerp(transform.rotation, new Quaternion(), 0.2f);

        if (lastMode != activeMode || _lastFocus_focus != focus || _lastFocus_lastFocus != lastFocus)
        {
            foreach (Ship ship in ships)
            {
                bool active = ship.transform == focus || ship.transform == lastFocus || !activeMode.focused;
                ship.gameObject.SetActive(active);
                if (active && !activeShips.Contains(ship))
                    activeShips.Add(ship);
                else if (activeShips.Contains(ship))
                    activeShips.Remove(ship);
            }
        }

        lastMode = activeMode;
        _lastFocus_focus = focus;
        _lastFocus_lastFocus = lastFocus;
    }

    public void Next()
    {
        index = (index + 1) % activeShips.Count;
        Set(index, true);
    }

    public void Previous()
    {
        if (--index < 0)
            index = activeShips.Count - 1;

        Set(index, true);
    }

    public void Search(string searchString)
    {
        foreach (Ship ship in activeShips)
        {
            ship.gameObject.SetActive(Literals.active[ship.definition.name].Contains(searchString));
        }
    }

    void Sort(float valueA, float valueB, bool smallestToLargest, int indexA, int indexB)
    {
        if (smallestToLargest)
        {
            if (valueB < valueA)
            {
                ships[indexA].transform.SetSiblingIndex(indexB);
                ships[indexB].transform.SetSiblingIndex(indexA);
            }
        }
        else
        {
            if (valueB > valueA)
            {
                ships[indexA].transform.SetSiblingIndex(indexB);
                ships[indexB].transform.SetSiblingIndex(indexA);
            }
        }
    }

    public void SortByValue(bool smallestToLargest)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            for (int j = i; j < ships.Count; j++)
            {
                Sort(ships[i].definition.value, ships[j].definition.value, smallestToLargest, i, j);
            }
        }

        Set();
    }

    public void SortByRarity(bool smallestToLargest)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            for (int j = i; j < ships.Count; j++)
            {
                Sort(ships[i].definition.oneIn, ships[j].definition.oneIn, smallestToLargest, i, j);
            }
        }

        Set();
    }

    public void SortByRisk(bool smallestToLargest)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            for (int j = i; j < ships.Count; j++)
            {
                Sort(FloatHelper.RiskStringToFloat(ships[i].definition.risk), FloatHelper.RiskStringToFloat(ships[j].definition.risk), smallestToLargest, i, j);
            }
        }

        Set();
    }

    public void Set(int index, bool moveTo)
    {
        lastFocus = focus;
        focus = ships[index].transform;

        this.index = index;
        if (moveTo)
            Camera.main.GetComponent<MoveTo>().AddFrame(ships[index].node.transform, 1, false, 0);
    }

    public void Set(string mode)
    {
        Mode newMode = modes.Find(m => m.name == mode);
        if (newMode != null)
            activeMode = newMode;

        Set();
    }

    public void Set()
    {
        transform.rotation = new Quaternion();

        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].transform.rotation = Quaternion.Euler(0, 0, 180);
            ships[i].isUnlocked = activeMode.showLocked || PlayerSave.Active().Get("unlocked ships").value.Contains(ships[i].definition.name + " ");
            ships[i].gameObject.SetActive(ships[i].isUnlocked);
            if (ships[i].isUnlocked && !activeShips.Contains(ships[i]))
            {
                activeShips.Add(ships[i]);
                activeShips[activeShips.Count - 1].index = i;
            }
            else if (activeShips.Contains(ships[i]))
            {
                activeShips.Remove(ships[i]);
            }
        }

        // Determine the configuration of the inventory from the dimensions
        // Find out how many dimensions have scale 0
        // This determines whether the ships are arrayed 1D, 2D, or 3D

        bool isX = activeMode.X.scale != 0;
        bool isY = activeMode.Y.scale != 0;
        bool isZ = activeMode.Z.scale != 0;

        // Arrange all ships along the X axis
        if (isX && !isY && !isZ)
            X();
        // Arrange all ships along the Y axis
        else if (!isX && isY && !isZ)
            Y();
        // Arrange all ships along the Z axis
        else if (!isX && !isY && isZ)
            Z();
        // Arrange all ships along the XY plane
        // Some Constraints
        else if (isX && isY && !isZ)
            XY();
        // Arrange all ships along the XZ plane
        // Some Constraints
        else if (isX && !isY && isZ)
            XZ();
        // Arrange all ships along the YZ plane
        // Some Constraints
        else if (!isX && isY && isZ)
            YZ();
        // Arrange all ships in the XYZ volume
        // Some Constraints
        else
            XYZ();
    }

    void X()
    {

    }

    void Y()
    {

    }

    void Z()
    {
        int index = 0;
        foreach (Ship ship in activeShips)
        {
            ship.transform.localPosition = Vector3.forward * activeMode.Z.scale * index++;
        }
    }

    void XY()
    {
        bool isXLimit = activeMode.X.maximum > -1;
        bool isYLimit = activeMode.Y.maximum > -1;

        // "Tall" box
        if (isXLimit)
        {

        }
        // "Wide" box
        else if (isYLimit)
        {
            int index = 0;
            int columns = Mathf.CeilToInt(ships.Count / (float)activeMode.Y.maximum);
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < activeMode.Y.maximum; j++)
                {
                    index = i * columns + j;
                    if (index < activeShips.Count)
                        activeShips[index].transform.localPosition = new Vector3((i - columns / 2) * activeMode.X.scale, (j - activeMode.Y.maximum / 2) * activeMode.Y.scale, 0);
                    else
                        break;
                }
            }
        }
        // Match to nearest square
        else
        {

        }
    }

    void XZ()
    {

    }

    void YZ()
    {

    }

    void XYZ()
    {

    }
}
