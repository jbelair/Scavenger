using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrollBinding : MonoBehaviour
{
    public Statistic scroll;
    public StatisticUEI scrollUEI;
    public float scrollCount = 0;
    public float scrollMin = 0;
    public float scrollMax = 100;
    public float scrollLast = 0;
    public float scrollDownThreshold = 10;
    public UnityEvent scrollDown;
    public float scrollUpThreshold = 10;
    public UnityEvent scrollUp;

    // Use this for initialization
    void Start()
    {
        scroll = Players.players[0].statistics["Scroll"];
        scrollUEI = scroll;
    }

    // Update is called once per frame
    void Update()
    {
        scrollCount += scroll.Get<float>();

        if (scrollMin >= 0)
            scrollCount = Mathf.Min(scrollCount, scrollMin);
        else if (scrollMax >= 0)
            scrollCount = Mathf.Max(scrollCount, scrollMax);

        if (scrollCount - scrollLast > scrollUpThreshold)
        {
            scrollLast = scrollCount;
            scrollUp.Invoke();
        }

        if (scrollCount - scrollLast < -scrollDownThreshold)
        {
            scrollLast = scrollCount;
            scrollDown.Invoke();
        }
    }
}
