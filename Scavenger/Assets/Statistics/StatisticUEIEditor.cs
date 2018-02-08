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
    public SerializedProperty vName, state, vI, vF, vS, vV2, vV3, vGO;
    private bool initialised = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 54;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        vName = prop.FindPropertyRelative("name");
        state = prop.FindPropertyRelative("type");
        vI = prop.FindPropertyRelative("valueInt");
        vF = prop.FindPropertyRelative("valueFloat");
        vS = prop.FindPropertyRelative("valueString");
        vV2 = prop.FindPropertyRelative("valueV2");
        vV3 = prop.FindPropertyRelative("valueV3");
        vGO = prop.FindPropertyRelative("valueGO");

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
                EditorGUI.PropertyField(rField, vI, GUIContent.none);
                break;
            case Statistic.ValueType.Float:
                EditorGUI.PropertyField(rField, vF, GUIContent.none);
                break;
            case Statistic.ValueType.String:
                EditorGUI.PropertyField(rField, vS, GUIContent.none);
                break;
            case Statistic.ValueType.Vector2:
                EditorGUI.PropertyField(rField, vV2, GUIContent.none);
                break;
            case Statistic.ValueType.Vector3:
                EditorGUI.PropertyField(rField, vV3, GUIContent.none);
                break;
            case Statistic.ValueType.GameObject:
                EditorGUI.PropertyField(rField, vGO, GUIContent.none);
                break;
        }

        EditorGUI.EndProperty();
    }
}
