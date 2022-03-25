using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

 
    GridTiles[,] grid;
    SwipeInput sInput;
    public float roundingThreshhold = 0.7f;
    

    public List<string> Inventory;

    private void Awake()
    {
        
        sInput = FindObjectOfType<Animator>().GetBehaviour<SwipeInput>();
        grid = FindObjectOfType<GridGenerator>().grid;       
    }
    private void Update()
    {
        var yPos = transform.position;

        if(sInput.roundingDirectionalYPosition.x == 0 && sInput.roundingDirectionalYPosition.y == 0)
            yPos.y = grid[RoundDownToInt(transform.position.x), RoundDownToInt(transform.position.z)].transform.position.y + 1.5f;

        if (sInput.roundingDirectionalYPosition.x == 0 && sInput.roundingDirectionalYPosition.y == 1)
            yPos.y = grid[RoundDownToInt(transform.position.x), RoundUpToInt(transform.position.z)].transform.position.y + 1.5f;

        if (sInput.roundingDirectionalYPosition.x == 1 && sInput.roundingDirectionalYPosition.y == 1)
            yPos.y = grid[RoundUpToInt(transform.position.x), RoundUpToInt(transform.position.z)].transform.position.y + 1.5f;

        if (sInput.roundingDirectionalYPosition.x == 1 && sInput.roundingDirectionalYPosition.y == 0)
            yPos.y = grid[RoundUpToInt(transform.position.x), RoundDownToInt(transform.position.z)].transform.position.y + 1.5f;
        transform.position = yPos;
    }

    int RoundDownToInt(float x)
    {
        int y = 0;
        float v = x - Mathf.FloorToInt(x);
        if (v <= roundingThreshhold)
        {
            y = Mathf.FloorToInt(v) + (int)(x - v);
        }
        else
        {
            y = Mathf.CeilToInt(v) + (int)(x - v);
        }
        return y;
    }

    int RoundUpToInt(float x)
    {
        int y = 0;
        float v = x - Mathf.FloorToInt(x);
        if (v >= 1-roundingThreshhold)
        {
            y = Mathf.CeilToInt(v) + (int)(x - v);
        }
        else
        {
            y = Mathf.FloorToInt(v) + (int)(x - v);
        }
        return y;
    }

}
