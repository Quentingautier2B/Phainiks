using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiling : MonoBehaviour
{
    [SerializeField] public bool refreshRend = false;
    [SerializeField] public bool refreshRendTempo = false;
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
    public Material Cmat;
    public GameObject cube1, cube2, cube3, cube4;

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
        if (tile.tempoTile != 0 || tile.crumble)
        {
            TempoTile.SetActive(true);
        }
        else
        {
            TempoTile.SetActive(false);
        }
        if (tile.walkable  && tile.open)
            SetDirectionalMaterial();
        //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void OnDrawGizmos()
    {
        TempoTile = transform.Find("TempoTile").gameObject;
        tile = GetComponent<GridTiles>();
        t = FindObjectOfType<TileVariables>();


        if (tile.tempoTile != 0 || tile.crumble)
        {
            TempoTile.SetActive(true);
        }
        else 
        {
            TempoTile.SetActive(false);
        }

        if (tile.walkable && refreshRend)
        {
            gridG = FindObjectOfType<GridGenerator>();
            gridG.generateGrid();
            grid = gridG.grid;

            mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
            SetDirectionalMaterial();
            //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
        }
        else
            refreshRend = false;

        if (tile.walkable && (tile.tempoTile != 0 || tile.crumble)  && refreshRendTempo)
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
        if (tile.tempoTile != 0 || tile.crumble)
        {
            if (tile.crumble)
            {
                foreach (MeshRenderer m in tempoTilesMats)
                {
                    m.material = Cmat;
                    refreshRendTempo = false;
                }
            }

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
            CubeActive(cube1, false);

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
            mesh.transform.Rotate(-90, 0, 0);
            CubeActive(cube1,false);
            /*print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffR);
            print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffL);
            print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffU);
            print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffD);*/
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
            mesh.transform.Rotate(-90, 180, 0);
            CubeActive(cube1,false);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 90, 0);
            CubeActive(cube1,false);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, -90, 0);
            CubeActive(cube1,false);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 180, 0);
            CubeActive(cube1, false);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, -90, 0);
            CubeActive(cube1, false);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 0, 0);
            CubeActive(cube1, true);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 90, 0);
            CubeActive(cube1, true);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 180, 0);
            CubeActive(cube1, true);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, -90, 0);
            CubeActive(cube1, true);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 0, 0);
            CubeActiveTwo(cube1, cube3, true);
            SetCubeSize();
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
            mesh.transform.Rotate(-90, 90, 0);
            CubeActiveTwo(cube1, cube3, true);
            SetCubeSize();
     
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
            mesh.transform.Rotate(-90, 180, 0);
            CubeActiveTwo(cube1, cube3, true);
            SetCubeSize();

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
            mesh.transform.Rotate(-90, -90, 0);
            CubeActiveTwo(cube1, cube3, true);
            SetCubeSize();


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
            cube1.SetActive(true);
            cube2.SetActive(true);
            cube3.SetActive(true);
            cube4.SetActive(true);
            SetCubeSize();
            // mesh.transform.rotation = Quaternion.identity;
            // mesh.transform.Rotate(-90, mesh.transform.rotation.y, 0);
        }
    }

    void CubeActive(GameObject cube, bool activate)
    {
        /*cube1.SetActive(false);
        cube2.SetActive(false);
        cube3.SetActive(false);
        cube4.SetActive(false);

        if(activate)
            cube.SetActive(true);*/
    }
    
    void CubeActiveTwo(GameObject cube, GameObject cubeT, bool activate)
    {
       /* cube1.SetActive(false);
        cube2.SetActive(false);
        cube3.SetActive(false);
        cube4.SetActive(false);

        if (activate)
        {
            cube.SetActive(true);
            cubeT.SetActive(true);
        }*/
    }


    void SetCubeSize()
    {

        cube1.transform.localPosition = new Vector3(cube1.transform.localPosition.x, cube1.transform.localPosition.y,  0.01f);
        cube2.transform.localPosition = new Vector3(cube2.transform.localPosition.x, cube2.transform.localPosition.y,  0.01f);
        cube3.transform.localPosition = new Vector3(cube3.transform.localPosition.x, cube3.transform.localPosition.y,  0.01f);
        cube4.transform.localPosition = new Vector3(cube4.transform.localPosition.x, cube4.transform.localPosition.y,  0.01f);

        if (tile.HeightDiffU > tile.HeightDiffL)
        {
            cube1.transform.localScale = new Vector3(cube1.transform.localScale.x, tile.HeightDiffL * 0.02f, cube1.transform.localScale.z);
            cube1.transform.localPosition = new Vector3(cube1.transform.localPosition.x, cube1.transform.localPosition.y, cube1.transform.localPosition.z - (tile.HeightDiffL * 0.01f));
        }
        else
        {
            cube1.transform.localScale = new Vector3(cube1.transform.localScale.x, tile.HeightDiffU * 0.02f, cube1.transform.localScale.z);
            cube1.transform.localPosition = new Vector3(cube1.transform.localPosition.x, cube1.transform.localPosition.y, cube1.transform.localPosition.z - (tile.HeightDiffU * 0.01f));
        }


        if (tile.HeightDiffU > tile.HeightDiffR )
        {
            cube2.transform.localScale = new Vector3(cube2.transform.localScale.x, tile.HeightDiffR * 0.02f, cube2.transform.localScale.z);
            cube2.transform.localPosition = new Vector3(cube2.transform.localPosition.x, cube2.transform.localPosition.y, cube2.transform.localPosition.z - (tile.HeightDiffR * 0.01f));
        }
        else
        {
            cube2.transform.localScale = new Vector3(cube2.transform.localScale.x, tile.HeightDiffU * 0.02f, cube2.transform.localScale.z);
            cube2.transform.localPosition = new Vector3(cube2.transform.localPosition.x, cube2.transform.localPosition.y, cube2.transform.localPosition.z - (tile.HeightDiffU * 0.01f));
        }


        if (tile.HeightDiffD > tile.HeightDiffL )
        {
            cube3.transform.localScale = new Vector3(cube3.transform.localScale.x, tile.HeightDiffL * 0.02f, cube3.transform.localScale.z);
            cube3.transform.localPosition = new Vector3(cube3.transform.localPosition.x, cube3.transform.localPosition.y, cube3.transform.localPosition.z - (tile.HeightDiffL * 0.01f));
        }
        else
        {
            cube3.transform.localScale = new Vector3(cube3.transform.localScale.x, tile.HeightDiffD * 0.02f, cube3.transform.localScale.z);
            cube3.transform.localPosition = new Vector3(cube3.transform.localPosition.x, cube3.transform.localPosition.y, cube3.transform.localPosition.z - (tile.HeightDiffD * 0.01f));
        }

        if (tile.HeightDiffD > tile.HeightDiffR)
        {
            cube4.transform.localScale = new Vector3(cube4.transform.localScale.x, tile.HeightDiffR * 0.02f, cube4.transform.localScale.z);
            cube4.transform.localPosition = new Vector3(cube4.transform.localPosition.x, cube4.transform.localPosition.y, cube4.transform.localPosition.z - (tile.HeightDiffR * 0.01f / 2));
        }
        else
        {
            cube4.transform.localScale = new Vector3(cube4.transform.localScale.x, tile.HeightDiffD * 0.02f, cube4.transform.localScale.z);
            cube4.transform.localPosition = new Vector3(cube4.transform.localPosition.x, cube4.transform.localPosition.y, cube4.transform.localPosition.z - (tile.HeightDiffD * 0.01f / 2));
        }

    }
}
