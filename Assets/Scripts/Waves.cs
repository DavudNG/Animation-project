using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public int Dimensions = 10;
    public Octave[] Octaves;
    public float UVScale;

    protected MeshFilter mMeshFilter;
    protected Mesh mMesh;
    

    // Start is called before the first frame update
    void Start()
    {
        mMesh = new Mesh();
        mMesh.name = gameObject.name;

        mMesh.vertices = GenerateVerts();
        mMesh.triangles = GenerateTris();
        mMesh.uv = GenerateUVs();
        mMesh.RecalculateBounds();
        mMesh.RecalculateNormals();

        mMeshFilter = gameObject.AddComponent<MeshFilter>();
        mMeshFilter.mesh = mMesh;
    }

    public float GetHeight(Vector3 position)
    {
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));


        p1.x = Mathf.Clamp(p1.x, 0, Dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, Dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, Dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, Dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, Dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, Dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, Dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, Dimensions);

        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector4.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
            + (max - Vector3.Distance(p2, localPos))
            + (max - Vector3.Distance(p3, localPos))
            + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        var height = mMesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
            + mMesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
            + mMesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
            + mMesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        return height * transform.lossyScale.y / dist;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[mMesh.vertices.Length];

        for (int x = 0; x <= Dimensions; x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimensions + 1) * (Dimensions + 1)];
        
        for(int x = 0; x <= Dimensions; x++)
            for(int z = 0; z <= Dimensions; z++)
            {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        return verts;
    }
    private int index(int x, int z)
    {
        return x * (Dimensions + 1) + z;
    }

    private int[] GenerateTris()
    {
        //var tris = new int[mMesh.vertices.Length * 6];
        //for (int x = 0; x < Dimensions; x++)
        //{
        //    for (int z = 0; z < Dimensions; z++)
        //    {
        //        //tris[index(x, z) * 6 + 0] = index(x, z);
        //        //tris[index(x, z) * 6 + 1] = index(x + 1, z + 1);
        //        //tris[index(x, z) * 6 + 2] = index(x + 1, z);
        //        //tris[index(x, z) * 6 + 3] = index(x, z);
        //        //tris[index(x, z) * 6 + 4] = index(x, z + 1);
        //        //tris[index(x, z) * 6 + 5] = index(x + 1, z + 1);
        //
        //        tris[index(x, z) * 6 + 0] = index(x + 1, z);
        //        tris[index(x, z) * 6 + 1] = index(x + 1, z + 1);
        //        tris[index(x, z) * 6 + 2] = index(x, z);
        //        tris[index(x, z) * 6 + 3] = index(x + 1, z + 1);
        //        tris[index(x, z) * 6 + 4] = index(x, z + 1);
        //        tris[index(x, z) * 6 + 5] = index(x, z);
        //    }
        //}
        //return tris;

        var tries = new int[mMesh.vertices.Length * 6];

        //two triangles are one tile
        for (int x = 0; x < Dimensions; x++)
        {
            for (int z = 0; z < Dimensions; z++)
            {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }
        return tries;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = mMesh.vertices;
        for(int x = 0; x <= Dimensions;x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                var y = 0f;
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x) / Dimensions, (z * Octaves[o].scale.y) / Dimensions) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x + Time.time * Octaves[o].speed.x) / Dimensions, 
                            (z * Octaves[o].scale.y + Time.time * Octaves[o].speed.y) / Dimensions) - 0.5f;
                        y += perl * Octaves[o].height;
                    }
                }

                verts[index(z,x)] = new Vector3(x, y, z);
            }
        }

        mMesh.vertices = verts;
        mMesh.RecalculateNormals();
    }

    [System.Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
