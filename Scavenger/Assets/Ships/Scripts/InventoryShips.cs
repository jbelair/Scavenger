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
    public int index;

    public Vector3 velocity;
    public float smoothSpeed = 1;

    public List<Ship> ships = new List<Ship>();

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (focus && activeMode.moveToFocus)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, -focus.localPosition, ref velocity, smoothSpeed).Multiply(new Vector3(activeMode.clampX ? 0 : 1, activeMode.clampY ? 0 : 1, activeMode.clampZ ? 0 : 1));
        }
        else
            transform.localPosition = Vector3.zero;

        if (activeMode.rotation.magnitude > 0)
            transform.Rotate(activeMode.rotation * Time.deltaTime);
        else
            transform.rotation = new Quaternion();// Quaternion.Lerp(transform.rotation, new Quaternion(), 0.2f);
    }

    public void Next()
    {
        index = (index + 1) % transform.childCount;
        Set(index, true);
    }

    public void Previous()
    {
        if (--index < 0)
            index = transform.childCount - 1;
        Set(index, true);
    }

    public void Set(int index, bool moveTo)
    {
        focus = transform.GetChild(index);
        this.index = index;
        if (moveTo)
            Camera.main.GetComponent<MoveTo>().AddFrame(focus.GetComponentInChildren<Ship>().node.transform, 1, false, 0);

        Players.players[0].statistics["value"].Set(ships[index].definition.value);
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
        // Determine the configuration of the inventory from the dimensions
        // Find out how many dimensions have scale 0
        // This determines whether the ships are arrayed 1D, 2D, or 3D

        bool isX = activeMode.X.scale != 0;
        bool isY = activeMode.Y.scale != 0;
        bool isZ = activeMode.Z.scale != 0;

        //bool isXLimit = x.maximum > -1;
        //bool isYLimit = y.maximum > -1;
        //bool isZLimit = z.maximum > -1;

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
        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].transform.localPosition = Vector3.forward * activeMode.Z.scale * i;
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
            int columns = Mathf.CeilToInt(ships.Count / (float)activeMode.Y.maximum);
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < activeMode.Y.maximum; j++)
                {
                    int ind = (i * activeMode.Y.maximum) + j;
                    if (ind < ships.Count)
                    {
                        ships[ind].transform.localPosition = new Vector3((i - columns / 2) * activeMode.X.scale, (j - activeMode.Y.maximum / 2) * activeMode.Y.scale, 0);
                    }
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
