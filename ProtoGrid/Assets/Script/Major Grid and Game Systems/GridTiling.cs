using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiling : MonoBehaviour
{
    [SerializeField] bool refreshRend = false;
    GridTiles[,] grid;
    GridGenerator gridG;
    GridTiles tile;
    public Material mat4D;
    public Material mat3D;
    public Material mat2DO;
    public Material mat2DA;
    public Material mat1D;
    public Material mat0D;
    public Mesh mesh1D;
    public Mesh mesh2DA;
    public Mesh normalMesh;
    MeshRenderer mesh;
    Mesh meshF;


    private void Awake()
    {
        gridG = FindObjectOfType<GridGenerator>();
        grid = gridG.grid;
        tile = GetComponent<GridTiles>();
        mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
        //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
       // transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void Start()
    {
        if(tile.walkable && tile.tempoTile == 0 && !tile.crumble && tile.open)
            SetDirectionalMaterial();
        //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void OnDrawGizmos()
    {
        tile = GetComponent<GridTiles>();
        if (tile.walkable && tile.tempoTile == 0 && !tile.crumble && refreshRend)
        {
            gridG = FindObjectOfType<GridGenerator>();
            grid = gridG.grid;
            
            mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
            SetDirectionalMaterial();
            //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
        }
        //mesh.transform.rotation = Quaternion.identity;
    }

    public void SetDirectionalMaterial()
    {
        print("tiling");
        //4 directions
        if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat4D;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0,0);
        }

        //3 directions
        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
        }

        //2 directions opposées

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
        }

        //2 directions adjacentes

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            //meshF = mesh2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            //meshF = mesh2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
           // meshF = mesh2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            //meshF = mesh2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
        }

        //1 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            //meshF = mesh1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            meshF = mesh1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            //meshF = mesh1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            //meshF = mesh1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
        }

        //0 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat0D;
            //meshF = normalMesh;
            refreshRend = false;
           // mesh.transform.rotation = Quaternion.identity;
           // mesh.transform.Rotate(-90, mesh.transform.rotation.y, 0);
        }
    }
}
