using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Utility/Instantiable")]
public class Instantiable : MonoBehaviour
{
    public bool isDead = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        isDead = true;
    }
}
