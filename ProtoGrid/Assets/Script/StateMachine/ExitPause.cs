using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPause : StateMachineBehaviour
{
    bool awake = true;
    GridTiles[,] grid;
    Vector3 Target;
    int x, y;
    public float slowLerpSpeed;
    float LerpSpeed;
    public float fastLerpSpeed;
    Transform player;
    bool lerping;
    bool endFlag;
    DoCoroutine doC;
    InGameUI inGameUI;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            doC = FindObjectOfType<DoCoroutine>();
            inGameUI = FindObjectOfType<InGameUI>();
            awake = false;

        }
        

        x = (int)player.position.x;
        y = (int)player.position.z;
        doC.revertLerper();
        foreach (GridTiles tile in grid)
        {
            if ((tile.transform.position.x != x || tile.transform.position.z != y) && !tile.invisible)
            {
                //tile.lerpSpeed = 0.1f;
                //tile.target = (int)tile.transform.position.y + 20;
                animator.GetComponent<DoCoroutine>().startCoroutine(tile);
                tile.currentPosY = tile.transform.position.y;
            }

        }
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GridTiles tile in grid)
        {
            if (tile.pauseAnim)
                pauseTileMovement(tile);
        }

        if (endFlag)
        {
            animator.SetBool("Paused", false);
            endFlag = false;
        }
    }


    void pauseTileMovement(GridTiles tile)
    {
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.currentPosY, tile.target, tile.lerpSpeed), tile.transform.position.z);
        
        //Debug.Log(tile.target);
        tile.lerpSpeed  +=  Time.deltaTime * Mathf.Lerp(1,.01f,tile.lerpSpeed);


        //Called on last loop
        if (tile.transform.position.y >= tile.target - 0.01f)
        {
            grid[x, y].GetComponent<GridTiling>().SetDirectionalMaterial();
            tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
            tile.pauseAnim = false;
            endFlag = true;
        }

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inGameUI.inGameUI.SetActive(true);
        foreach (GridTiles tile in grid)
        {
            var til = tile.transform.position;
            til.y = Mathf.RoundToInt(til.y);
            tile.transform.position = til;

            tile.lerpSpeed = .1f;

        }
       // grid[x, y].GetComponent<GridTiling>().SetDirectionalMaterial();
    }
}
