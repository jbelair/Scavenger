using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveModifier : MonoBehaviour
{
    public string dataName = "";
    public enum Modifier { Set, Add, Mult };
    public Modifier modifierType;
    public float modifier;
    public MinMaxStatistic value;
    private PlayerSave.Data data;

    public void Do()
    {
        if (data == null)
            data = PlayerSave.Active.Get(dataName);

        switch (modifierType)
        {
            case Modifier.Set:
                switch (value.Value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        value.Value.Set((int)modifier);
                        data.value = value.Value.Get<int>().ToString(); 
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        value.Value.Set(modifier);
                        data.value = value.Value.Get<float>().ToString();
                        break;
                    case Statistic.ValueType.String:
                        data.type = "String";
                        data.value = value.Value.Get<string>();
                        break;
                    case Statistic.ValueType.Colour:
                        data.type = "Colour";
                        data.value = JsonUtility.ToJson(value.Value.Get<Color>());
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = JsonUtility.ToJson(value.Value.Get<Vector2>());
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = JsonUtility.ToJson(value.Value.Get<Vector3>());
                        break;
                    case Statistic.ValueType.GameObject:
                        data.type = "Game Object";
                        data.value = JsonUtility.ToJson(value.Value.Get<GameObject>());
                        break;
                    case Statistic.ValueType.GameObjectArray:
                        data.type = "Game Object Array";
                        data.value = JsonUtility.ToJson(value.Value.Get<object>() as List<GameObject>);
                        break;
                    case Statistic.ValueType.Object:
                        data.type = "Object";
                        data.value = JsonUtility.ToJson(value.Value.Get<object>());
                        break;
                }
                break;
            case Modifier.Add:
                switch (value.Value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        value.Value.Set(int.Parse(data.value) + (int)modifier);
                        data.value = value.Value.Get<int>().ToString();
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        value.Value.Set(float.Parse(data.value) + modifier);
                        data.value = value.Value.Get<float>().ToString();
                        break;
                    case Statistic.ValueType.String:
                        data.type = "String";
                        data.value += value.Value.Get<string>();
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = (JsonUtility.FromJson<Vector2>(data.value) + value.Value.Get<Vector2>()).ToString();
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = (JsonUtility.FromJson<Vector3>(data.value) + value.Value.Get<Vector3>()).ToString();
                        break;
                }
                break;
            case Modifier.Mult:
                switch (value.value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        value.Value.Set(value.Value.Get<int>() * (int)modifier);
                        data.value = value.Value.Get<int>().ToString();
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        value.Value.Set(value.Value.Get<float>() * modifier);
                        data.value = value.Value.Get<float>().ToString();
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = (JsonUtility.FromJson<Vector2>(data.value) * value.Value.Get<Vector2>()).ToString();
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = (JsonUtility.FromJson<Vector3>(data.value).Multiply(value.Value.Get<Vector3>())).ToString();
                        break;
                }
                break;
        }
    }
}
