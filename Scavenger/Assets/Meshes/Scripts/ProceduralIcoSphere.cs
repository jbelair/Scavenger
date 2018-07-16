using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct IcoFace
{
    public int a, b, c;

    public IcoFace(int v1, int v2, int v3)
    {
        a = v1;
        b = v2;
        c = v3;
    }
}

[Serializable]
public struct IcoLodSettings
{
    [Range(0, 5), Tooltip("Defines how many times the icosahedron will have it's faces divided and pushed to unit sphere distance.")]
    public int subdivisions;
    [Tooltip("Defines how far away the camera must be for this lod setting to set subdivisions to the desired value")]
    public float cameraDistance;
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class ProceduralIcoSphere : MonoBehaviour
{
    public static Dictionary<int, Mesh> vbos = new Dictionary<int, Mesh>();

    [Range(0,5),Tooltip("Defines how many times the icosahedron will have it's faces divided and pushed to unit sphere distance.")]
    public int subdivisions = 0;

    [Header("LOD")]
    private Transform editorView;
    public bool hasLod = false;
    public IcoLodSettings low;
    public IcoLodSettings high;

    #if UNITY_EDITOR
    [Header("Diagnostics")]
    public int triangleCount;
    public int vertexCount;
    #endif

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();

    public void Export()
    {

    }

    public void Generate()
    {
        if (vbos.ContainsKey(subdivisions) && vbos[subdivisions] != null)
        {
            GetComponent<MeshFilter>().mesh = mesh = vbos[subdivisions];
            Debug.Log(gameObject.name + " " + vbos[subdivisions]);
        }
        else
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();

            vertices.Clear();
            uvs.Clear();

            // Thank you: http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html
            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            vertices.Add(new Vector3(-1, t, 0));
            vertices.Add(new Vector3(1, t, 0));
            vertices.Add(new Vector3(-1, -t, 0));
            vertices.Add(new Vector3(1, -t, 0));

            vertices.Add(new Vector3(0, -1, t));
            vertices.Add(new Vector3(0, 1, t));
            vertices.Add(new Vector3(0, -1, -t));
            vertices.Add(new Vector3(0, 1, -t));

            vertices.Add(new Vector3(t, 0, -1));
            vertices.Add(new Vector3(t, 0, 1));
            vertices.Add(new Vector3(-t, 0, -1));
            vertices.Add(new Vector3(-t, 0, 1));

            // I need to make all the above vertices normalized, that puts them on the unit sphere
            FixVertices(vertices);

            List<IcoFace> faces = new List<IcoFace>()
        {
            // 0 vertex faces
            new IcoFace( 0, 11, 5),
            new IcoFace( 0,  5,  1),
            new IcoFace( 0,  1,  7),
            new IcoFace( 0,  7,  10),
            new IcoFace( 0, 10, 11),
            // One down, three more to go
            new IcoFace( 1,  5,  9),
            new IcoFace( 5, 11,  4),
            new IcoFace(11, 10,  2),
            new IcoFace(10,  7,  6),
            new IcoFace( 7,  1,  8),
            // Two left
            new IcoFace( 3,  9,  4),
            new IcoFace( 3,  4,  2),
            new IcoFace( 3,  2,  6),
            new IcoFace( 3,  6,  8),
            new IcoFace( 3,  8,  9),
            // Last one
            new IcoFace( 4,  9,  5),
            new IcoFace( 2,  4, 11),
            new IcoFace( 6,  2, 10),
            new IcoFace( 8,  6,  7),
            new IcoFace( 9,  8,  1)
        };

            // The actual subdivision logic
            for (int i = 0; i < subdivisions; i++)
            {
                List<IcoFace> newFaces = new List<IcoFace>();
                foreach (IcoFace face in faces)
                {
                    vertices.Add(((vertices[face.a] + vertices[face.b]) / 2f).normalized);
                    int a = vertices.Count - 1;
                    vertices.Add(((vertices[face.b] + vertices[face.c]) / 2f).normalized);
                    int b = vertices.Count - 1;
                    vertices.Add(((vertices[face.c] + vertices[face.a]) / 2f).normalized);
                    int c = vertices.Count - 1;

                    newFaces.Add(new IcoFace(face.a, a, c));
                    newFaces.Add(new IcoFace(face.b, b, a));
                    newFaces.Add(new IcoFace(face.c, c, b));
                    newFaces.Add(new IcoFace(a, b, c));
                }
                faces = newFaces;
            }

            // Final passes, first use cartesian to polar conversion
            for (int i = 0; i < vertices.Count; i++)
            {
                uvs.Add(new Vector2(Mathf.Atan2(vertices[i].z, vertices[i].x) / (Mathf.PI * 2f), Mathf.Acos(vertices[i].y * -1) / (Mathf.PI)));
            }

            // Second look for the so called 'zipper' and fix it
            // http://mft-dev.dk/uv-mapping-sphere/
            for (int i = 0; i < faces.Count; i++)
            {
                Vector3 a = new Vector3(uvs[faces[i].a].x, uvs[faces[i].a].y, 0);
                Vector3 b = new Vector3(uvs[faces[i].b].x, uvs[faces[i].b].y, 0);
                Vector3 c = new Vector3(uvs[faces[i].c].x, uvs[faces[i].c].y, 0);
                Vector3 cross = Vector3.Cross(b - a, c - a);
                if (cross.z > 0)
                {
                    // One or more of these uvs is wrong
                    if (a.x < 0.25f)
                    {
                        vertices.Add(vertices[faces[i].a]);
                        uvs.Add(uvs[faces[i].a] + new Vector2(1, 0));
                        faces[i] = new IcoFace(vertices.Count - 1, faces[i].b, faces[i].c);
                    }
                    if (b.x < 0.25f)
                    {
                        vertices.Add(vertices[faces[i].b]);
                        uvs.Add(uvs[faces[i].b] + new Vector2(1, 0));
                        faces[i] = new IcoFace(faces[i].a, vertices.Count - 1, faces[i].c);
                    }
                    if (c.x < 0.25f)
                    {
                        vertices.Add(vertices[faces[i].c]);
                        uvs.Add(uvs[faces[i].c] + new Vector2(1, 0));
                        faces[i] = new IcoFace(faces[i].a, faces[i].b, vertices.Count - 1);
                    }
                }
            }

            // Strip the faces out of the IcoFace structure and into a normal int list
            List<int> triangles = new List<int>();
            foreach (IcoFace face in faces)
            {
                triangles.Add(face.a);
                triangles.Add(face.b);
                triangles.Add(face.c);
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            lastSubdivisions = subdivisions;

#if UNITY_EDITOR
            vertexCount = vertices.Count;
            triangleCount = triangles.Count;
#endif

            vbos[subdivisions] = mesh;
        }
    }

    void FixVertices(List<Vector3> vertices)
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i].normalized;
        }
    }

    void Awake()
    {
        Generate();
    }

    // Use this for initialization
    void Start()
    {

    }

    private int lastSubdivisions = 0;
    // Update is called once per frame
    void Update()
    {
        if (hasLod)
        {
            float distance = high.cameraDistance;
            if (Application.isPlaying)
                distance = Vector3.Distance(Camera.main.transform.position, transform.position);

            if (distance > low.cameraDistance)
                subdivisions = low.subdivisions;
            else
            {
                subdivisions = Mathf.FloorToInt(Mathf.Lerp(high.subdivisions, low.subdivisions, (distance - high.cameraDistance) / (low.cameraDistance - high.cameraDistance)));
            }
        }

        if (subdivisions != lastSubdivisions)
            Generate();
    }
}
