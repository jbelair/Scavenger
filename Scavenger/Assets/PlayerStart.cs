using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public bool initialised = false;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Poll());
    }

    IEnumerator Poll()
    {
        while (!initialised)
        {
            yield return new WaitForSeconds(0.01f);

            Player player = FindObjectOfType<Player>();

            if (player)
            {
                player.transform.position = transform.position;
                initialised = true;
            }
        }
        yield return null;
    }
}
