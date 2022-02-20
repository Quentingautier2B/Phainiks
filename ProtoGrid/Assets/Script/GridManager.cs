using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GridTiles[,] gridTiles;
    [SerializeField] int raws, columns;
    
    void Start()
    {

        GridTiles[] list = FindObjectsOfType<GridTiles>();
        gridTiles = new GridTiles[raws, columns];
        for (int i = 0; i < list.Length; i++)
        {
            int x = (int)list[i].transform.position.x / (int)list[i].transform.localScale.x;
            int y = (int)list[i].transform.position.z / (int)list[i].transform.localScale.y;
            gridTiles[x, y] = list[i];
            gridTiles[x, y].name = "tiles " + x + " "+ y;
        }
        
    }


    void Update()
    {

    }
}

