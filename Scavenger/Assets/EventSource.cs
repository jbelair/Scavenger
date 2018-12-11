using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSource : MonoBehaviour
{
    [Serializable]
    public class TickEvent
    {
        public bool running = true;
        public float seconds;
        public UnityEvent tick;
    }

    public UnityEvent start;
    public UnityEvent update;
    public UnityEvent disable;
    public UnityEvent destroy;
    public List<TickEvent> ticks;
    public Dictionary<string, UnityEvent> dynamic;

    // Use this for initialization
    void Start()
    {
        start.Invoke();

        foreach (TickEvent tick in ticks)
        {
            StartCoroutine(Co_Tick(tick));
        }
    }

    // Update is called once per frame
    void Update()
    {
        update.Invoke();
    }

    void OnDisable()
    {
        disable.Invoke();
    }

    void OnDestroy()
    {
        destroy.Invoke();
    }

    IEnumerator Co_Tick(TickEvent tick)
    {
        while(tick.running)
        {
            tick.tick.Invoke();
            yield return new WaitForSeconds(tick.seconds);
        }

        yield return null;
    }
}
