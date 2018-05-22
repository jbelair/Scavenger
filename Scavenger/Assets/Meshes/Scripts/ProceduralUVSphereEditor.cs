using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralUVSphere))]
class ProceduralUVSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralUVSphere mesh = (ProceduralUVSphere)target;
        if (GUILayout.Button("Generate"))
        {
            mesh.Generate();
        }

        DrawDefaultInspector();
    }
}