using UnityEngine;

public class Procedural_Mesh : MonoBehaviour
{
    [SerializeField] Vector3 GridSize = new Vector3(10, 5, 10);
    [SerializeField] Material material = null;
    [SerializeField] Mesh mesh = null;
    [SerializeField] MeshCollider meshCollider = null;
    [SerializeField] private float zoomMinValue = 2f;
    [SerializeField] private float zoomMaxValue = 10f;
    [SerializeField] private float zoom = 0f;
    [SerializeField] private float noiseLimit = 0.5f;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        zoom = Random.Range(zoomMinValue, zoomMaxValue);

        MakeGrid();
        //Noise2d();
        Noise3d();
        March();

        meshCollider.sharedMesh = mesh;
    }

    private void MakeGrid()
    {
        // Allocate The Array
        Marching_Cubes.grd = new GridPoint[(int)GridSize.x, (int)GridSize.y, (int)GridSize.z];

        // Define The Ponints

        for (int z = 0; z < GridSize.z; z++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    Marching_Cubes.grd[x, y, z] = new GridPoint();
                    Marching_Cubes.grd[x, y, z].Position = new Vector3(x, y, z);
                    Marching_Cubes.grd[x, y, z].On = false;
                }
            }
        }
    }

    private void Noise2d()
    {
        float nx = 0f;
        float nz = 0f;
        float height = 0f;

        for (int z = 0; z < GridSize.z; z++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                nx = (x / GridSize.x) * zoom;
                nz = (z / GridSize.z) * zoom;
                height = Mathf.PerlinNoise(nx, nz) * GridSize.y;

                for (int y = 0; y < GridSize.y; y++)
                {
                    if (y < height)
                    {
                        Marching_Cubes.grd[x, y, z].On = true;
                    }
                    else
                    {
                        Marching_Cubes.grd[x, y, z].On = false;
                    }
                }
            }
        }
    }

    private void Noise3d() 
    {
        float nx = 0f;
        float ny = 0f;
        float nz = 0f;
        float noise = 0f;

        for (int z = 0; z < GridSize.z; z++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    nx = (x / GridSize.x) * zoom;
                    ny = (y / GridSize.y) * zoom;
                    nz = (z / GridSize.z) * zoom;
                    noise = Marching_Cubes.PerlinNoise3D(nx, ny, nz);  //0..1

                    if (noise > noiseLimit)
                    {
                        Marching_Cubes.grd[x, y, z].On = true;
                    }
                    else
                    {
                        Marching_Cubes.grd[x, y, z].On = false;
                    }
                }
            }
        }
    }

    private void March()
    {
        GameObject go = this.gameObject;
        mesh = Marching_Cubes.GetMesh(ref go,ref material);

        // Clear the Vertices , Triangles && UV
        Marching_Cubes.Clear();

        Marching_Cubes.MarchCubes();


        Marching_Cubes.SetMesh(ref mesh);
    }
}
