using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatisticUEI))]
[CanEditMultipleObjects]
public class StatisticUEIEditor : PropertyDrawer
{
    public SerializedProperty vName, state, vI, vF, vS, vV2, vV3, vGO, vGOArray, vStat;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 54;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        vName = prop.FindPropertyRelative("name");
        state = prop.FindPropertyRelative("type");
        
        EditorGUI.BeginProperty(position, label, prop);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect rName = new Rect(position.x, position.y, position.width, 15);
        Rect rState = new Rect(position.x, rName.y + rName.height + 4, position.width, 15);
        Rect rField = new Rect(position.x, rState.y + rState.height + 4, position.width, 15);

        EditorGUI.PropertyField(rName, vName, GUIContent.none);

        EditorGUI.PropertyField(rState, state, GUIContent.none);

        switch ((Statistic.ValueType)state.intValue)
        {
            case Statistic.ValueType.Integer:
                vI = prop.FindPropertyRelative("valueInt");
                EditorGUI.PropertyField(rField, vI, GUIContent.none);
                break;
            case Statistic.ValueType.Float:
                vF = prop.FindPropertyRelative("valueFloat");
                EditorGUI.PropertyField(rField, vF, GUIContent.none);
                break;
            case Statistic.ValueType.String:
                vS = prop.FindPropertyRelative("valueString");
                EditorGUI.PropertyField(rField, vS, GUIContent.none);
                break;
            case Statistic.ValueType.Vector2:
                vV2 = prop.FindPropertyRelative("valueV2");
                EditorGUI.PropertyField(rField, vV2, GUIContent.none);
                break;
            case Statistic.ValueType.Vector3:
                vV3 = prop.FindPropertyRelative("valueV3");
                EditorGUI.PropertyField(rField, vV3, GUIContent.none);
                break;
            case Statistic.ValueType.GameObject:
                vGO = prop.FindPropertyRelative("valueGO");
                EditorGUI.PropertyField(rField, vGO, GUIContent.none);
                break;
            case Statistic.ValueType.GameObjectArray:
                vGOArray = prop.FindPropertyRelative("valueGOArray");
                EditorGUI.PropertyField(rField, vGOArray, GUIContent.none);
                break;
            case Statistic.ValueType.Statistic:
                vStat = prop.FindPropertyRelative("valueStatistic");
                EditorGUI.PropertyField(rField, vStat, GUIContent.none);
                break;
        }

        EditorGUI.EndProperty();
    }
}
