using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    public bool defaultScreen;

    public RectTransform back;
    public RectTransform mid;
    public RectTransform front;
    
    void Awake()
    {
        GetComponentInParent<UIManager>().screens.Add(this);
    }
}
