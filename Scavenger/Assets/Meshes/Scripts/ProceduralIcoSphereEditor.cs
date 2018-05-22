using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralIcoSphere))]
class ProceduralIcoSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralIcoSphere mesh = (ProceduralIcoSphere)target;
        if (GUILayout.Button("Generate"))
        {
            mesh.Generate();
        }

        DrawDefaultInspector();
    }
}