using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WidgetMessage : MonoBehaviour
{
    public string message;
    public TextMeshProUGUI text;

    public void Set(string message)
    {
        this.message = message;
        text.SetText(message);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
