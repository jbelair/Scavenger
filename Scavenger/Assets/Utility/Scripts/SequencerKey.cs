using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerKey<T>
{
    public string name;
    public float duration;
    public float durationCurrent;
    public T obj;
}

[Serializable]
public class SequencerIntKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public int value;
}

[Serializable]
public class SequencerFloatKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public float value;
}

[Serializable]
public class SequencerVector2Key
{
    public string name;
    public float duration;
    public float durationCurrent;
    public Vector2 value;
}

[Serializable]
public class SequencerVector3Key
{
    public string name;
    public float duration;
    public float durationCurrent;
    public Vector3 value;
}

[Serializable]
public class SequencerQuaternionKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public Quaternion value;
}

[Serializable]
public class SequencerColourKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public Color value;
}

[Serializable]
public class SequencerStringKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public string value;
}