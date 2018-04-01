using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Sequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public int index = 0;

    protected int indexPrevious = 0;
    protected int indexCurrent = 0;

    public virtual void BaseStart()
    {
        indexCurrent = index;
        indexPrevious = index;
    }

    //public virtual void Update()
    //{
    //    switch (format)
    //    {
    //        case Format.PingPong:
    //            int ind = (index >= array.Length) ? 2 - index : index;
    //            indexPrevious = ((ind - 1 >= 0) ? ((index > array.Length) ? ind + 1 : ind - 1) : 0);
    //            indexCurrent = ind;
    //            if (current.durationCurrent > current.duration)
    //            {
    //                ind = index;
    //                index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
    //                if (index >= array.Length * 2)
    //                {
    //                    if (repetitions != 0)
    //                    {
    //                        repetitions--;
    //                        index = 0;
    //                    }
    //                    else
    //                        index = 0;
    //                }
    //                if (ind != index)
    //                    array[index].durationCurrent = current.durationCurrent - current.duration;
    //            }
    //            break;
    //        default:
    //            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
    //            current = array[index];
    //            current.durationCurrent += Time.deltaTime;
    //            if (current.durationCurrent > current.duration)
    //            {
    //                ind = index;
    //                index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
    //                if (index >= array.Length)
    //                {
    //                    if (repetitions != 0)
    //                    {
    //                        repetitions--;
    //                        index = 0;
    //                    }
    //                    else
    //                        index = array.Length - 1;
    //                }
    //                if (ind != index)
    //                    array[index].durationCurrent = current.durationCurrent - current.duration;
    //            }
    //            break;
    //    }
    //}
}

public class Sequencer<T>
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerKey<T>[] array;
    public int index = 0;

    public enum Type { OTHER, INT, FLOAT, VECTOR2, VECTOR3, QUATERNION, COLOUR };
    public Type type = Type.OTHER;
    public bool interpolateable = false;

    public T Start()
    {
        if (array.Length > 0)
        {
            if (array[0] is int)
                type = Type.INT;
            else if (array[0] is float)
                type = Type.FLOAT;
            else if (array[0] is Vector2)
                type = Type.VECTOR2;
            else if (array[0] is Vector3)
                type = Type.VECTOR3;
            else if (array[0] is Quaternion)
                type = Type.QUATERNION;
            else if (array[0] is Color)
                type = Type.COLOUR;
            else
                type = Type.OTHER;

            interpolateable = type != Type.OTHER;// || array[0] is float ||  ||  || ;
            return array[0].obj;
        }
        else
            return default(T);
    }

    public T Update()
    {
        if (array.Length > 0)
        {
            SequencerKey<T> previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerKey<T> current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.obj;
                case Interpolation.Linear:
                    float percentage = 0;
                    switch (type)
                    {
                        case Type.OTHER:
                            return current.obj;
                        case Type.INT:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            return (T)Convert.ChangeType(Mathf.Lerp(Convert.ToInt32(previous.obj), Convert.ToInt32(current.obj), percentage), percentage.GetType());
                        case Type.FLOAT:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            return (T)Convert.ChangeType(Mathf.Lerp(Convert.ToSingle(previous.obj), Convert.ToSingle(current.obj), percentage), percentage.GetType());
                        case Type.VECTOR2:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            System.Type t = Vector2.zero.GetType();
                            return (T)Convert.ChangeType(Vector2.Lerp((Vector2)Convert.ChangeType(previous.obj, t), (Vector2)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.VECTOR3:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Vector3.zero.GetType();
                            return (T)Convert.ChangeType(Vector3.Lerp((Vector3)Convert.ChangeType(previous.obj, t), (Vector3)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.QUATERNION:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Quaternion.identity.GetType();
                            return (T)Convert.ChangeType(Quaternion.Lerp((Quaternion)Convert.ChangeType(previous.obj, t), (Quaternion)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.COLOUR:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Color.white.GetType();
                            return (T)Convert.ChangeType(Color.Lerp((Color)Convert.ChangeType(previous.obj, t), (Color)Convert.ChangeType(current.obj, t), percentage), t);
                    }
                    break;
                case Interpolation.Spherical:
                    switch (type)
                    {
                        case Type.OTHER:
                            return current.obj;
                        case Type.INT:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            return current.obj = (T)Convert.ChangeType(Mathf.Lerp(Convert.ToInt32(previous.obj), Convert.ToInt32(current.obj), percentage), percentage.GetType());
                        case Type.FLOAT:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            return current.obj = (T)Convert.ChangeType(Mathf.Lerp(Convert.ToSingle(previous.obj), Convert.ToSingle(current.obj), percentage), percentage.GetType());
                        case Type.VECTOR2:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            System.Type t = Vector2.zero.GetType();
                            return current.obj = (T)Convert.ChangeType(Vector2.Lerp((Vector2)Convert.ChangeType(previous.obj, t), (Vector2)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.VECTOR3:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Vector3.zero.GetType();
                            return current.obj = (T)Convert.ChangeType(Vector3.Slerp((Vector3)Convert.ChangeType(previous.obj, t), (Vector3)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.QUATERNION:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Quaternion.identity.GetType();
                            return current.obj = (T)Convert.ChangeType(Quaternion.Slerp((Quaternion)Convert.ChangeType(previous.obj, t), (Quaternion)Convert.ChangeType(current.obj, t), percentage), t);
                        case Type.COLOUR:
                            previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                            current = array[index];
                            percentage = current.durationCurrent / current.duration;
                            t = Color.white.GetType();
                            return current.obj = (T)Convert.ChangeType(Color.Lerp((Color)Convert.ChangeType(previous.obj, t), (Color)Convert.ChangeType(current.obj, t), percentage), t);
                    }
                    break;
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return default(T);
    }
}

[Serializable]
public class IntSequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerIntKey[] array;
    public int index = 0;

    public int Start()
    {
        return array[0].value;
    }

    public int Update()
    {
        if (array.Length > 0)
        {
            SequencerIntKey previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerIntKey current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return (int)Mathf.Lerp((float)previous.value, (float)current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return (int)Mathf.Lerp((float)previous.value, (float)current.value, percentage);
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return 0;
    }
}

[Serializable]
public class FloatSequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerFloatKey[] array;
    public int index = 0;

    public float Start()
    {
        return array[0].value;
    }

    public float Update()
    {
        if (array.Length > 0)
        {
            SequencerFloatKey previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerFloatKey current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Mathf.Lerp(previous.value, current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Mathf.Lerp(previous.value, current.value, percentage);
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return 0;
    }
}

[Serializable]
public class Vector2Sequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerVector2Key[] array;
    public int index = 0;

    public Vector2 Start()
    {
        return array[0].value;
    }

    public Vector2 Update()
    {
        if (array.Length > 0)
        {
            SequencerVector2Key previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerVector2Key current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Vector2.Lerp(previous.value, current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Vector2.Lerp(previous.value, current.value, percentage);
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return Vector2.zero;
    }
}

[Serializable]
public class Vector3Sequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerVector3Key[] array;
    public int index = 0;

    public Vector3 Start()
    {
        return array[0].value;
    }

    public Vector3 Update()
    {
        if (array.Length > 0)
        {
            SequencerVector3Key previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerVector3Key current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Vector3.Lerp(previous.value, current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Vector3.Lerp(previous.value, current.value, percentage);
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return Vector3.zero;
    }
}

[Serializable]
public class QuaternionSequencer
{
    public enum Format { Loop, PingPong };
    public Format format;
    public int repetitions = 1;
    public enum Interpolation { None, Linear, Spherical };
    public Interpolation interpolation = Interpolation.None;
    public SequencerQuaternionKey[] array;
    public int index = 0;

    public Quaternion Start()
    {
        return array[0].value;
    }

    public Quaternion Update()
    {
        if (array.Length > 0)
        {
            SequencerQuaternionKey previous = array[((index - 1 >= 0) ? index - 1 : 0)];
            SequencerQuaternionKey current = array[index];
            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Quaternion.Lerp(previous.value, current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Quaternion.Lerp(previous.value, current.value, percentage);
            }

            switch (format)
            {
                case Format.Loop:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                case Format.PingPong:
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index > array.Length - 1)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }
        }

        return Quaternion.identity;
    }
}

[Serializable]
public class ColourSequencer : Sequencer
{
    public SequencerColourKey[] array;
    

    public Color Start()
    {
        base.BaseStart();
        return array[indexCurrent].value;
    }

    public Color Update()
    {
        if (array.Length > 0)
        {
            SequencerColourKey previous;
            SequencerColourKey current;

            switch (format)
            {
                case Format.PingPong:
                    int ind = (index >= array.Length) ? 2 - index : index;
                    previous = array[((ind - 1 >= 0) ? ((index > array.Length) ? ind+1 : ind - 1) : 0)];
                    current = array[ind];
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        ind = index;
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index >= array.Length * 2)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = 0;
                        }
                        if (ind != index)
                            array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
                default:
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    current.durationCurrent += Time.deltaTime;
                    if (current.durationCurrent > current.duration)
                    {
                        ind = index;
                        index++;// = (index+1 >= transforms.Length) ? transforms.Length - 1 : index++;// = (index + 1) % transforms.Length;
                        if (index >= array.Length)
                        {
                            if (repetitions != 0)
                            {
                                repetitions--;
                                index = 0;
                            }
                            else
                                index = array.Length - 1;
                        }
                        if (ind != index)
                            array[index].durationCurrent = current.durationCurrent - current.duration;
                    }
                    break;
            }

            switch (interpolation)
            {
                case Interpolation.None:
                    return current.value;
                case Interpolation.Linear:
                    float percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Color.Lerp(previous.value, current.value, percentage);
                case Interpolation.Spherical:
                    percentage = 0;
                    previous = array[((index - 1 >= 0) ? index - 1 : 0)];
                    current = array[index];
                    percentage = current.durationCurrent / current.duration;
                    return Color.Lerp(previous.value, current.value, percentage);
            }
        }

        return Color.white;
    }
}