using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : StateMachineBehaviour
{
    bool awake = true;
    InGameUI inGameUI;
    GameObject PauseMenu;
    GridTiles[,] grid;
    GridTiling gT;
    Vector3 Target;
    int x, y;
    public float slowLerpSpeed;
    float LerpSpeed;
    public float fastLerpSpeed;
    Transform player;
    bool lerping;
    DoCoroutine doC;
    RectTransform pauseUI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            doC = animator.GetComponent<DoCoroutine>();
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            inGameUI = FindObjectOfType<InGameUI>();
            awake = false;
        }
        x = (int)player.position.x;
        y = (int)player.position.z;
        Target = new Vector3(x, grid[x, y].transform.position.y + 20, y);
        LerpSpeed = slowLerpSpeed;
        gT = grid[x, y].GetComponent<GridTiling>();
        grid[x,y].transform.Find("Renderer").GetComponent<MeshRenderer>().material =gT.mat0D;
        doC.startLerper();
        foreach (GridTiles tile in grid)
        {            
            if((tile.transform.position.x != x || tile.transform.position.z != y) && !tile.invisible)
            {

                tile.target = (int)tile.transform.position.y - 20;
                doC.startCoroutine(tile);
                
            }

        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //gT.AllColonneActivate();
        //gT.SetCubeSize();
        inGameUI.inGameUI.SetActive(false);
        foreach (GridTiles tile in grid)
        {
            if (tile.pauseAnim)
                doC.pauseTileMovement(tile, gT);         
        }
    }


    



    public void OnStateExit(Animator animator, int stateMachinePathHash)
    {
        
        foreach (GridTiles tile in grid)
        {
            if ((tile.transform.position.x != x || tile.transform.position.z != y) && !tile.invisible)
            {
                tile.pauseAnim = false;
                tile.lerpSpeed = 0;
                tile.pauseLerpSpeed = 0.1f;
                tile.target = (int)tile.transform.position.y + (20 + (tile.target - (int)tile.transform.position.y));
            }
        }
    }
    
}
