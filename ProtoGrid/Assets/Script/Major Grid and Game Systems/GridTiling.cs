using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiling : MonoBehaviour
{
    [SerializeField] bool refreshRend = false;
    [SerializeField] bool refreshRendTempo = false;
    GridTiles[,] grid;
    GridGenerator gridG;
    TileVariables t;
    GridTiles tile;
    GameObject TempoTile;
    public MeshRenderer[] tempoTilesMats = new MeshRenderer[6];

    public Material mat4D;
    public Material mat3D;
    public Material mat2DO;
    public Material mat2DA;
    public Material mat1D;
    public Material mat0D;
    public Material TmatR;
    public Material TmatB1;
    public Material TmatB2;
    public Material TmatG1;
    public Material TmatG2;
    public Material TmatG3;

    public Mesh mesh1D;
    public Mesh mesh2DA;
    public Mesh normalMesh;
    MeshRenderer mesh;
    Mesh meshF;


    private void Awake()
    {
        TempoTile = transform.Find("TempoTile").gameObject;
        gridG = FindObjectOfType<GridGenerator>();
        t = FindObjectOfType<TileVariables>();
        grid = gridG.grid;
        tile = GetComponent<GridTiles>();
        mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
        //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
       // transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void Start()
    {
        if (tile.tempoTile != 0)
        {
            TempoTile.SetActive(true);
        }
        else
        {
            TempoTile.SetActive(false);
        }
        if (tile.walkable && !tile.crumble && tile.open)
            SetDirectionalMaterial();
        //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void OnDrawGizmos()
    {
        TempoTile = transform.Find("TempoTile").gameObject;
        tile = GetComponent<GridTiles>();
        t = FindObjectOfType<TileVariables>();


        if (tile.tempoTile != 0)
        {
            TempoTile.SetActive(true);
        }
        else 
        {
            TempoTile.SetActive(false);
        }

        if (tile.walkable && !tile.crumble && refreshRend)
        {
            gridG = FindObjectOfType<GridGenerator>();
            grid = gridG.grid;

            mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
            SetDirectionalMaterial();
            //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
        }
        else
            refreshRend = false;

        if (tile.walkable && tile.tempoTile != 0 && !tile.crumble && refreshRendTempo)
        {
            //mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            TempoTileMaterial();
        }
        else
            refreshRendTempo = false;
        //mesh.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        //TempoTileMaterial();
    }

    public void TempoTileMaterial()
    {
        if (tile.tempoTile != 0)
        {

            if (tile.tempoTile == 1)
            {
                foreach(MeshRenderer m in tempoTilesMats)
                {
                    m.material = TmatR;
                    refreshRendTempo = false;
                }
            }


            if (tile.tempoTile == 2)
            {
                if (t.blueTimer == 0 || t.blueTimer == 2)
                {
                    foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatB2;
                        refreshRendTempo = false;
                    }
                }
                else if (t.blueTimer == 1)
                {
                    foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatB1;
                        refreshRendTempo = false;
                    }
                }
                refreshRendTempo = false;
            }

            if (tile.tempoTile == 3)
            {
                if (t.greenTimer == 0 || t.greenTimer == 3)
                {
                    foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatG3;
                        refreshRendTempo = false;
                    }
                }
                else if ((t.greenTimer == 1 && t.greenFlag) || (t.greenTimer == 2 && !t.greenFlag))
                {
                    foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatG1;
                        refreshRendTempo = false;
                    }
                }
                else if ((t.greenTimer == 2 && t.greenFlag) || (t.greenTimer == 1 && !t.greenFlag))
                {
                    foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatG2;
                        refreshRendTempo = false;
                    }
                }
            }
        }
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
            //meshF = normalMesh;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90,0);
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
            mesh.transform.Rotate(-90, 180, 0);
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
            mesh.transform.Rotate(-90, 90, 0);
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
            mesh.transform.Rotate(-90, 180, 0);
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
            mesh.transform.Rotate(-90, -90, 0);
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
            mesh.transform.Rotate(-90, 0, 0);
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
