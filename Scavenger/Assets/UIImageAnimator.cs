using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageAnimator : MonoBehaviour
{
    public Image image;
    public string active;
    public string lastActive;

    [Serializable]
    public class Key
    {
        public string name;
        public string sprite;
        public float duration;
        public float durationCurrent;

        public Key(Key key)
        {
            name = key.name;
            sprite = key.sprite;
            duration = key.duration;
            durationCurrent = 0;
        }
    }
    public List<Key> frames = new List<Key>();

    public List<Key> sequence = new List<Key>();

    // Use this for initialization
    void Start()
    {
        if (active != "")
            Animate(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (sequence.Count > 0)
        {
            if (sequence[0].durationCurrent > sequence[0].duration && sequence.Count > 1)
            {
                sequence.RemoveAt(0);
            }
            sequence[0].durationCurrent += Time.deltaTime;
            active = sequence[0].name;
            image.sprite = Sprites.Get(sequence[0].sprite);
        }

        lastActive = active;
    }

    public void Animate(string str)
    {
        sequence.Add(new Key(frames.Find(key => key.name == str)));
    }
}
