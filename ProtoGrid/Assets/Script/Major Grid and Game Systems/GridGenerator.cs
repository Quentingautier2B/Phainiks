using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif
public class GridGenerator : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    #region variables
    public GridTiles[,] grid;
    [SerializeField] bool instantiateGrid = false;
    public GameObject Tile;
    Transform player;
    [Header("Input Values")]
    
    public int raws;
    public int columns;
    [HideInInspector] public Vector3 ogPos;


    #endregion
    void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        GridTiles[] list = FindObjectsOfType<GridTiles>();
        grid = new GridTiles[raws, columns];
        for (int i = 0; i < list.Length; i++)
        {
            int x = (int)list[i].transform.position.x / (int)list[i].transform.localScale.x;
            int y = (int)list[i].transform.position.z / (int)list[i].transform.localScale.y;
            grid[x, y] = list[i];
            grid[x, y].name = "tiles " + x + " "+ y;
        }
        
    }
    private void Start()
    {
        foreach (GridTiles obj in grid)
        {
            if (obj.originalPosition)
            {
                ogPos = new Vector3(obj.transform.position.x, player.position.y, obj.transform.position.z);
                player.position = ogPos;
            }
        }
    }

    private void OnDrawGizmos()
    {


        if (instantiateGrid)
        {

            if (grid != null)
            {
                foreach (GridTiles obj in grid)
                {

                    //DestroyImmediate(obj.gameObject);
                }

            }

            GridTiles[] list = FindObjectsOfType<GridTiles>();
            grid = new GridTiles[raws, columns];
            for (int i = 0; i < list.Length; i++)
            {
                int x = (int)list[i].transform.position.x / (int)list[i].transform.localScale.x;
                int y = (int)list[i].transform.position.z / (int)list[i].transform.localScale.y;
                grid[x, y] = list[i];
                grid[x, y].name = "tiles " + x + " " + y;
            }

            for (int x = 0; x < raws; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (!grid[x, y])
                    {
                        var inst = Instantiate(Tile);
                        inst.transform.position = new Vector3(x, 0, y);
                        inst.transform.Find("Renderer").Rotate(90 * Random.Range(0, 5), 90 * Random.Range(0, 5), 90 * Random.Range(0, 5));
                        grid[x, y] = inst.GetComponent<GridTiles>();
                        grid[x, y].transform.parent = GameObject.FindGameObjectWithTag("Terrain").transform;
                        grid[x, y].name = "tiles " + x + " " + y;
                    }
                }
            }

            instantiateGrid = false;

        }

    }

    

}

