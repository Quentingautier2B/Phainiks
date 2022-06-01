using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

 
    GridTiles[,] grid;
    SwipeInput sInput;
    public float roundingThreshhold = 0.7f;
    SceneChange sChange;

    public List<string> Inventory;
    float previousYpos;
    float lerp;
    bool flag;
    private void Awake()
    {
        sChange = FindObjectOfType<SceneChange>();
        sInput = FindObjectOfType<Animator>().GetBehaviour<SwipeInput>();
        grid = FindObjectOfType<GridGenerator>().grid;       
    }
    public void PlayerStickTile()
    {
        var yPos = transform.position;
        previousYpos = transform.position.y;
        if (grid[RoundDownToInt(transform.position.x), RoundDownToInt(transform.position.z)].walkable && !sChange.Hub && !flag)
        {
            if (sInput.roundingDirectionalYPosition.x == 0 && sInput.roundingDirectionalYPosition.y == 0)
                yPos.y = grid[RoundDownToInt(transform.position.x), RoundDownToInt(transform.position.z)].transform.position.y + 1.5f;

            if (sInput.roundingDirectionalYPosition.x == 0 && sInput.roundingDirectionalYPosition.y == 1)
                yPos.y = grid[RoundDownToInt(transform.position.x), RoundUpToInt(transform.position.z)].transform.position.y + 1.5f;

            if (sInput.roundingDirectionalYPosition.x == 1 && sInput.roundingDirectionalYPosition.y == 1)
                yPos.y = grid[RoundUpToInt(transform.position.x), RoundUpToInt(transform.position.z)].transform.position.y + 1.5f;

            if (sInput.roundingDirectionalYPosition.x == 1 && sInput.roundingDirectionalYPosition.y == 0)
                yPos.y = grid[RoundUpToInt(transform.position.x), RoundDownToInt(transform.position.z)].transform.position.y + 1.5f;

            transform.position = yPos;
        }
    }

    public IEnumerator Lerper(float prevPosY, float yPos)
    {
        flag = true;
        lerp += Time.deltaTime * 3;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(prevPosY, yPos, lerp), transform.position.z);
        if (lerp >= 1)
        {
            lerp = 0;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            flag = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper(prevPosY, yPos));
        }
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
