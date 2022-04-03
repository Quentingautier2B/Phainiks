using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiling : MonoBehaviour
{
    GridTiles[,] grid;
    GridGenerator gridG;
    GridTiles tile;
    public Material mat4D;
    public Material mat3D;
    public Material mat2DO;
    public Material mat2DA;
    public Material mat1D;
    public Material mat0D;
    MeshRenderer mesh;
    [SerializeField] bool refreshRend = true;


    private void Awake()
    {
        gridG = FindObjectOfType<GridGenerator>();
        grid = gridG.grid;
        tile = GetComponent<GridTiles>();
        mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if(tile.walkable && tile.tempoTile == 0 && !tile.crumble && tile.open)
            SetDirectionalMaterial();
    }

    private void OnDrawGizmos()
    {
        tile = GetComponent<GridTiles>();
        if (tile.walkable && tile.tempoTile == 0 && !tile.crumble && refreshRend && tile.open)
        {
            gridG = FindObjectOfType<GridGenerator>();
            grid = gridG.grid;
            
            mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            SetDirectionalMaterial();
        }
        //mesh.transform.rotation = Quaternion.identity;
    }

    public void SetDirectionalMaterial()
    {
        //4 directions
        if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat4D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0,0,0);
        }

        //3 directions
        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 180, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 0, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, -90, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 90, 0);
        }

        //2 directions opposées

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 0, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 90, 0);
        }

        //2 directions adjacentes

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 180, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, -90, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 0, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 90, 0);
        }

        //1 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 180, 0);
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, -90, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 0, 0);
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 90, 0);
        }

        //0 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat0D;
            //refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(0, 0, 0);
        }
    }
}
