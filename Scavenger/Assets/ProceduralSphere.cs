﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on http://catlikecoding.com/unity/tutorials/rounded-cube/
// and http://catlikecoding.com/unity/tutorials/cube-sphere/

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class ProceduralSphere : MonoBehaviour
{
    [Range(2,20)]
    public int gridSize;
    public float radius = 1f;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    public Vector2[] uvs;

    private Color32[] cubeUV;

    void CreateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (gridSize + gridSize + gridSize - 3) * 4;
        int faceVertices = (
            (gridSize - 1) * (gridSize - 1) +
            (gridSize - 1) * (gridSize - 1) +
            (gridSize - 1) * (gridSize - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];
        uvs = new Vector2[vertices.Length];
        cubeUV = new Color32[vertices.Length];

        int v = 0;
        for (int y = 0; y <= gridSize; y++)
        {
            for (int x = 0; x <= gridSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }
            for (int z = 1; z <= gridSize; z++)
            {
                SetVertex(v++, gridSize, y, z);
            }
            for (int x = gridSize - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, gridSize);
            }
            for (int z = gridSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
            {
                SetVertex(v++, x, gridSize, z);
            }
        }
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
    }

    

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 s;
        s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
        normals[i] = s;
        vertices[i] = normals[i] * radius;

        uvs[i] = new Vector2(Mathf.Atan2(normals[i].z, normals[i].x) / (Mathf.PI * 2f) + 0.5f, normals[i].y / 2f + 0.5f);

        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    void CreateTriangles()
    {
        int quads = (gridSize * gridSize + gridSize * gridSize + gridSize * gridSize) * 2;
        int[] triangles = new int[quads * 6];

        int ring = (gridSize + gridSize) * 2;
        int t = 0, v = 0;

        for (int y = 0; y < gridSize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }

        t = CreateTopFace(triangles, t, ring);
        t = CreateBottomFace(triangles, t, ring);

        mesh.triangles = triangles;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * gridSize;
        for (int x = 0; x < gridSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

        int vMin = ring * (gridSize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;

        for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + gridSize - 1);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid, vMid + 1, vMid + gridSize - 1, vMid + gridSize);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + gridSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMin - 2);
        for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

        return t;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (gridSize - 1) * (gridSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < gridSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= gridSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + gridSize - 1, vMin + 1, vMid);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + gridSize - 1, vMid + gridSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + gridSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void CreateColliders()
    {
        if (!gameObject.GetComponent<SphereCollider>())
            gameObject.AddComponent<SphereCollider>();

        gameObject.GetComponent<SphereCollider>().radius = radius;
    }

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        CreateVertices();
        CreateTriangles();
        CreateColliders();

        lastGrid = gridSize;
        lastRadius = radius;
    }

    // Use this for initialization
    void Start()
    {

    }

    private int lastGrid = 0;
    private float lastRadius = 0;
    // Update is called once per frame
    void Update()
    {
        if (lastGrid != gridSize || lastRadius != radius)
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            CreateVertices();
            CreateTriangles();
            CreateColliders();
        }
    }
}
