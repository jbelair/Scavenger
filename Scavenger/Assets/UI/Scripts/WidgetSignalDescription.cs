using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WidgetSignalDescription : MonoBehaviour
{
    public DungeonType type;
    public DungeonGenerator generator;

    public RawImage background;
    public Image engage;
    public TMP_Text textName;
    public TMP_Text textDescription;
    public WidgetSignalTagGrid grid;

    // Use this for initialization
    void Start()
    {
        Scheme rarityScheme = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString(type.oneIn));

        //if (background)
        //{
        //    background.color = rarityScheme.colour;
        //}

        if (textName)
        {
            textName.text = Literals.active[type.name];
            //textName.color = rarityScheme.colour;
        }

        if (textDescription)
        {
            textDescription.text = Literals.active[type.risk] + "\n" + Literals.active[StringHelper.RarityIntToString(type.oneIn)] + "\n" + Literals.active[type.description];
            //textDescription.color = rarityScheme.colour;
        }

        if (engage)
        {
            Scheme riskScheme = Schemes.Scheme(type.risk);
            //engage.color = riskScheme.colour;// * Color.gray;
        }

        if (grid)
        {
            grid.Set(type);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(DungeonType dungeonType, DungeonGenerator dungeonGenerator)
    {
        type = dungeonType;
        generator = dungeonGenerator;
    }

    private Transform[] transforms;
    public void Engage()
    {
        Environment.jumpDistance = 0;
        Vector3 position = generator.transform.position;
        Quaternion rot = Quaternion.Euler(90, 0, 0);

        List<EnvironmentBasedStar> stars = /*(generator.dungeonTarget != "Star") ? */new List<EnvironmentBasedStar>(generator.transform.parent.GetComponentsInChildren<EnvironmentBasedStar>());// : new List<EnvironmentBasedStar>();
        transforms = new Transform[stars.Count];

        float distance = generator.dungeonType.distance > 0 ? generator.dungeonType.distance : 3f;

        Random.InitState(generator.hash);

        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), -1);

        int i = 0;
        if (!generator.dungeonTarget.Contains("Star"))
        {
            transforms = new Transform[1 + stars.Count];
            transforms[i++] = generator.transform;
            generator.transform.parent = null;
            generator.transform.position = offset * generator.transform.localScale.x * distance;
            generator.transform.rotation = rot;
        }

        foreach (EnvironmentBasedStar star in stars)
        {
            star.transform.parent = null;

            star.transform.position = (star.transform.position - position).XZY() + offset * generator.transform.localScale.x * distance;

            star.transform.rotation = rot;
            //float properPosition = star.transform.localScale.x * distance * generator.transform.localScale.x;
            //if (Mathf.Abs(star.transform.position.z) - star.transform.localScale.x < properPosition)
            //{
            //star.transform.position += properPosition.OOZ().Multiply(star.transform.position.z < 0 ? Vector3.back : Vector3.forward);
            //}

            if (star.transform.position.z > 0 || Mathf.Abs(star.transform.position.z) - star.transform.localScale.x < star.transform.localScale.x * (distance / 2f))
            {
                foreach (Transform child in star.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            //star.GetComponent<Light>().range *= 2;
            transforms[i++] = star.transform;
            DontDestroyOnLoad(star);
        }

        DontDestroyOnLoad(generator.gameObject);
        AsyncOperation op = SceneManager.LoadSceneAsync("Game.Environment", LoadSceneMode.Single);

        op.completed += Op_completed;
    }
    
    private void Op_completed(AsyncOperation obj)
    {
        Transform background = GameObject.Find("Background").transform;
        background.GetComponent<ParallaxBackground>().scale = EnvironmentRules.RadiusOfJupiter * generator.dungeonType.distance;
        foreach (Transform transform in transforms)
        {
            SceneManager.MoveGameObjectToScene(transform.gameObject, SceneManager.GetActiveScene());
            transform.SetParent(background, true);
            transform.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Background"));
        }

        FindObjectOfType<DungeonSceneAdd>().active = generator;
    }
}
