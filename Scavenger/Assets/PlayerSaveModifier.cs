using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveModifier : MonoBehaviour
{
    public string dataName = "";
    public enum Modifier { Set, Add, Mult };
    public Modifier modifierType;
    public StatisticUEI valueUEI;
    public Statistic value;
    private PlayerSave.Data data;

    public void Do()
    {
        value = valueUEI.Initialise();

        if (data == null)
            data = PlayerSave.Active.Get(dataName);

        switch (modifierType)
        {
            case Modifier.Set:
                switch (value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        data.value = value.Get<int>().ToString(); 
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        data.value = value.Get<float>().ToString();
                        break;
                    case Statistic.ValueType.String:
                        data.type = "String";
                        data.value = value.Get<string>();
                        break;
                    case Statistic.ValueType.Colour:
                        data.type = "Colour";
                        data.value = JsonUtility.ToJson(value.Get<Color>());
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = JsonUtility.ToJson(value.Get<Vector2>());
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = JsonUtility.ToJson(value.Get<Vector3>());
                        break;
                    case Statistic.ValueType.GameObject:
                        data.type = "Game Object";
                        data.value = JsonUtility.ToJson(value.Get<GameObject>());
                        break;
                    case Statistic.ValueType.GameObjectArray:
                        data.type = "Game Object Array";
                        data.value = JsonUtility.ToJson(value.Get<object>() as List<GameObject>);
                        break;
                    case Statistic.ValueType.Object:
                        data.type = "Object";
                        data.value = JsonUtility.ToJson(value.Get<object>());
                        break;
                }
                break;
            case Modifier.Add:
                switch (value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        data.value = (int.Parse(data.value) + value.Get<int>()).ToString();
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        data.value = (float.Parse(data.value) + value.Get<float>()).ToString();
                        break;
                    case Statistic.ValueType.String:
                        data.type = "String";
                        data.value += value.Get<string>();
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = (JsonUtility.FromJson<Vector2>(data.value) + value.Get<Vector2>()).ToString();
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = (JsonUtility.FromJson<Vector3>(data.value) + value.Get<Vector3>()).ToString();
                        break;
                }
                break;
            case Modifier.Mult:
                switch (value.type)
                {
                    case Statistic.ValueType.Integer:
                        data.type = "Integer";
                        data.value = (int.Parse(data.value) * value.Get<int>()).ToString();
                        break;
                    case Statistic.ValueType.Float:
                        data.type = "Float";
                        data.value = (float.Parse(data.value) * value.Get<float>()).ToString();
                        break;
                    case Statistic.ValueType.Vector2:
                        data.type = "Vector 2";
                        data.value = (JsonUtility.FromJson<Vector2>(data.value) * value.Get<Vector2>()).ToString();
                        break;
                    case Statistic.ValueType.Vector3:
                        data.type = "Vector 3";
                        data.value = (JsonUtility.FromJson<Vector3>(data.value).Multiply(value.Get<Vector3>())).ToString();
                        break;
                }
                break;
        }
    }
}
