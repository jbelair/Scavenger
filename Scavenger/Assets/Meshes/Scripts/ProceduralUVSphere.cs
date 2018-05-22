using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class ProceduralUVSphere : MonoBehaviour
{
    [Range(4,32),Tooltip("Defines the number of faces across the longitude, and along the latitude")]
    public int segments = 16;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();

    #if UNITY_EDITOR
    [Header("Diagnostics")]
    public int triangleCount;
    public int vertexCount;
    #endif

    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        vertices.Clear();
        uvs.Clear();

        // Starting at the north pole, generate a latitudal stripe of n segments, as wide as (2PIr)/n where n is number of segments.
        float tStep = (2f * Mathf.PI) / segments;
        float pStep = Mathf.PI / segments;
        for (int theta = 0; theta <= segments; theta++)
        {
            for (int phi = 0; phi <= segments; phi++)
            {
                vertices.Add(new Vector3(Mathf.Sin(phi * pStep) * Mathf.Sin(theta * tStep), Mathf.Cos(phi * pStep), Mathf.Sin(phi * pStep) * Mathf.Cos(theta * tStep)));
                uvs.Add(new Vector2(theta / (float)segments, 1 - (phi / (float)segments)));
            }
        }

        // Triangles are constructed in a second pass
        List<int> tris = new List<int>();

        List<Vector3Int> triangles = new List<Vector3Int>();

        for (int theta = 0; theta <= segments; theta++)
        {
            for (int phi = 0; phi < segments; phi++)
            {
                int a, b, c;
                a = phi + theta * segments;
                b = a + 1;
                c = a + segments + 2;

                if (a < vertices.Count && b < vertices.Count && c < vertices.Count)
                {
                    if (!Contains(triangles, new Vector3Int(a, b, c)))
                    {
                        tris.Add(a);
                        tris.Add(b);
                        tris.Add(c);
                        triangles.Add(new Vector3Int(a, b, c));
                    }
                    else
                    {
                        Debug.Log("ProceduralUVSphere: Trimming duplicate face @" + name);
                    }
                    
                    b = c;
                    c = b - 1;
                    if (!Contains(triangles, new Vector3Int(a, b, c)))
                    {
                        tris.Add(a);
                        tris.Add(b);
                        tris.Add(c);
                        triangles.Add(new Vector3Int(a, b, c));
                    }
                    else
                    {
                        Debug.Log("ProceduralUVSphere: Trimming duplicate face @" + name);
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();

        lastSegments = segments;

        #if UNITY_EDITOR
        vertexCount = vertices.Count;
        triangleCount = triangles.Count;
        #endif
    }

    bool Contains(List<Vector3Int> tris, Vector3Int v)
    {
        return tris.Contains(v) || tris.Contains(new Vector3Int(v.x, v.z, v.y)) || tris.Contains(new Vector3Int(v.y, v.x, v.z)) || tris.Contains(new Vector3Int(v.y, v.z, v.x)) || tris.Contains(new Vector3Int(v.z, v.y, v.x)) || tris.Contains(new Vector3Int(v.z, v.x, v.y));
    }

    void Awake()
    {
        Generate();
    }

    // Use this for initialization
    void Start()
    {

    }

    private int lastSegments = 0;
    // Update is called once per frame
    void Update()
    {
        if (lastSegments != segments)
            Generate();
    }

    //void OnDrawGizmos()
    //{
    //    if (vertices != null)
    //    {
    //        for (int i = 0; i < vertices.Count; i++)
    //        {
    //            Gizmos.color = new Color(uvs[i].x, uvs[i].y, ((1 - uvs[i].y) + (1 - uvs[i].x)) / 2);
    //            Gizmos.DrawSphere(vertices[i], 0.01f);
    //            Gizmos.DrawLine(vertices[i], vertices[i] + normals[i] / 10f);
    //        }
    //    }
    //}
}
