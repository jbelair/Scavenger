using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Statistics statistics;

    // Use this for initialization
    private void Awake()
    {
        Players.players.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Players.players.Remove(this);
    }
}
