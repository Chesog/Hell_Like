using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class Mesh_Generator : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private int X_GridSize = 20;
    [SerializeField] private int Z_GridSize = 20;
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    MeshCollider meshCollider;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider =  GetComponent<MeshCollider>();
        CreateMeshShape();
        UpdateMesh();

        meshCollider.sharedMesh = mesh;
    }

    private void CreateMeshShape() 
    {
        vertices = new Vector3[(X_GridSize + 1) * (Z_GridSize + 1)];

        for (int i = 0, z = 0; z <= Z_GridSize; z++)
        {
            for (int x = 0; x <= X_GridSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f,z * 0.3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[X_GridSize * Z_GridSize * 6];
        int vert = 0;
        int triang = 0;

        for (int z = 0; z < Z_GridSize; z++)
        {
            for (int x = 0; x < X_GridSize; x++)
            {
                triangles[triang + 0] = vert + 0;
                triangles[triang + 1] = vert + X_GridSize + 1;
                triangles[triang + 2] = vert + 1;
                triangles[triang + 3] = vert + 1;
                triangles[triang + 4] = vert + X_GridSize + 1;
                triangles[triang + 5] = vert + X_GridSize + 2;

                vert++;
                triang += 6;
            }
            vert++;
        }
    }
    private void UpdateMesh() 
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        int vertLength = vertices.Length;
        float rad = 0.1f;
        Gizmos.color = Color.red;

        for (int i = 0; i < vertLength; i++)
        {
            Gizmos.DrawSphere(vertices[i], rad);
        }
    }
}
