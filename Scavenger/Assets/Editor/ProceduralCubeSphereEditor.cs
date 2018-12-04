using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralCubeSphere))]
class ProceduralCubeSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralCubeSphere mesh = (ProceduralCubeSphere)target;
        if (GUILayout.Button("Generate"))
        {
            mesh.Generate();
        }

        DrawDefaultInspector();
    }
}