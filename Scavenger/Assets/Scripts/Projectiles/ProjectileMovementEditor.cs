//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(ProjectileMovementUEI))]
//[CanEditMultipleObjects]
//public class ProjectileMovementEditor : Editor
//{
//    public SerializedProperty state;
//    public Dictionary<string, SerializedProperty> props = new Dictionary<string, SerializedProperty>();

//    private void OnEnable()
//    {
//        props.Add("owner", serializedObject.FindProperty("owner"));
//        props.Add("rigidbody", serializedObject.FindProperty("rigidbody"));
//        props.Add("skill", serializedObject.FindProperty("skill"));
//        state = serializedObject.FindProperty("format");
//        props.Add("velocity", serializedObject.FindProperty("velocity"));
//        props.Add("velocityMax", serializedObject.FindProperty("velocityMax"));
//        props.Add("velocityRelative", serializedObject.FindProperty("velocityRelative"));
//        props.Add("acceleration", serializedObject.FindProperty("acceleration"));
//        props.Add("debug", serializedObject.FindProperty("debug"));
//        props.Add("turnRate", serializedObject.FindProperty("turnRate"));
//        props.Add("target", serializedObject.FindProperty("target"));
//        props.Add("lateralWander", serializedObject.FindProperty("lateralWander"));
//        props.Add("lateralWanderTime", serializedObject.FindProperty("lateralWanderTime"));
//        props.Add("lateralWanderRandomness", serializedObject.FindProperty("lateralWanderRandomness"));
//        props.Add("lateralWanderOffset", serializedObject.FindProperty("lateralWanderOffset"));
//        props.Add("lateralWanderCurrent", serializedObject.FindProperty("lateralWanderCurrent"));
//        props.Add("radialWander", serializedObject.FindProperty("radialWander"));
//        props.Add("radialWanderTime", serializedObject.FindProperty("radialWanderTime"));
//        props.Add("radialWanderRandomness", serializedObject.FindProperty("radialWanderRandomness"));
//        props.Add("radialWanderOffset", serializedObject.FindProperty("radialWanderOffset"));
//        props.Add("radialWanderCurrent", serializedObject.FindProperty("radialWanderCurrent")); 
//    }

//    private void GUIWander()
//    {
//        EditorGUILayout.PropertyField(props["lateralWander"]);
//        if (props["lateralWander"].floatValue > 0)
//        {
//            EditorGUILayout.PropertyField(props["lateralWanderTime"]);
//            EditorGUILayout.PropertyField(props["lateralWanderRandomness"]);

//            if (props["debug"].boolValue)
//            {
//                EditorGUILayout.PropertyField(props["lateralWanderOffset"]);
//                EditorGUILayout.PropertyField(props["lateralWanderCurrent"]);
//            }
//        }

//        ProjectileMovement.MovementFormat format = (ProjectileMovement.MovementFormat)state.intValue;
//        if (format != ProjectileMovement.MovementFormat.Home && format != ProjectileMovement.MovementFormat.Return && format != ProjectileMovement.MovementFormat.Orbit)
//        {
//            EditorGUILayout.PropertyField(props["radialWander"]);
//            if (props["radialWander"].floatValue > 0)
//            {
//                EditorGUILayout.PropertyField(props["radialWanderTime"]);
//                EditorGUILayout.PropertyField(props["radialWanderRandomness"]);

//                if (props["debug"].boolValue)
//                {
//                    EditorGUILayout.PropertyField(props["radialWanderOffset"]);
//                    EditorGUILayout.PropertyField(props["radialWanderCurrent"]);
//                }
//            }
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(props["owner"]);
//        EditorGUILayout.PropertyField(props["rigidbody"]);
//        EditorGUILayout.PropertyField(props["skill"]);
//        EditorGUILayout.PropertyField(state);
//        EditorGUILayout.PropertyField(props["velocity"]);
//        EditorGUILayout.PropertyField(props["velocityMax"]);
//        EditorGUILayout.PropertyField(props["velocityRelative"]);
//        EditorGUILayout.PropertyField(props["acceleration"]);
//        EditorGUILayout.PropertyField(props["debug"]);

//        switch ((ProjectileMovement.MovementFormat)state.intValue)
//        {
//            case ProjectileMovement.MovementFormat.Home:
//                EditorGUILayout.PropertyField(props["turnRate"]);
//                EditorGUILayout.PropertyField(props["target"]);
//                break;
//            case ProjectileMovement.MovementFormat.Steer:
//                EditorGUILayout.PropertyField(props["turnRate"]);
//                break;
//            case ProjectileMovement.MovementFormat.Return:
//                EditorGUILayout.PropertyField(props["turnRate"]);
//                break;
//            case ProjectileMovement.MovementFormat.Orbit:
//                EditorGUILayout.PropertyField(props["turnRate"]);
//                EditorGUILayout.PropertyField(props["target"]);
//                break;
//            default:
//                // TODO support default displaying all properties
//                //EditorGUILayout.PropertyField();
//                break;
//        }

//        GUIWander();

//        serializedObject.ApplyModifiedProperties();
//    }
//}
