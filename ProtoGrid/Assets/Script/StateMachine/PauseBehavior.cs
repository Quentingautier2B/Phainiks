using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : StateMachineBehaviour
{
    bool awake = true;
    GameObject inGameUI;
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
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            //PauseMenu = FindObjectOfType<PauseMenuUI>().gameObject;
            //inGameUI = FindObjectOfType<InGameUI>().gameObject;
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            awake = false;

        }
        x = (int)player.position.x;
        y = (int)player.position.z;
        Target = new Vector3(x, grid[x, y].transform.position.y + 20, y);
        LerpSpeed = slowLerpSpeed;
        gT = grid[x, y].GetComponent<GridTiling>();
        grid[x,y].transform.Find("Renderer").GetComponent<MeshRenderer>().material =gT.mat0D;
        
        foreach (GridTiles tile in grid)
        {            
            if((tile.transform.position.x != x || tile.transform.position.z != y) && !tile.invisible)
            {

                tile.target = (int)tile.transform.position.y - 20;
                animator.GetComponent<DoCoroutine>().startCoroutine(tile);
            }

        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gT.AllColonneActivate();
        gT.SetCubeSize();

        foreach (GridTiles tile in grid)
        {
            if (tile.pauseAnim)
                pauseTileMovement(tile);

            
        }

            /*grid[x, y].transform.position = Vector3.Lerp(grid[x, y].transform.position, Target, Time.deltaTime * LerpSpeed);
            LerpSpeed = Mathf.Lerp(LerpSpeed, fastLerpSpeed, Time.deltaTime * LerpSpeed);
            if (LerpSpeed > 0.8 * fastLerpSpeed)
            {
                fastLerpSpeed = slowLerpSpeed;
            }

            if ( Target.y - grid[x, y].transform.position.y < 0.4)
            {
                grid[x, y].transform.position = Target;
            }*/
    }

    void pauseTileMovement(GridTiles tile)
    {
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tile.lerpSpeed * Time.deltaTime), tile.transform.position.z);
        
        tile.lerpSpeed = Mathf.Lerp(tile.lerpSpeed, 3, Time.deltaTime * tile.lerpSpeed);
    

        //Called on last loop
        if (tile.transform.position.y <= tile.target + 0.3f)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
            
            tile.pauseAnim = false;
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
                tile.target = (int)tile.transform.position.y + (20 + (tile.target - (int)tile.transform.position.y));
            }
        }
    }
    
}
