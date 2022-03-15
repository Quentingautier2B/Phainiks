using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveBehavior : StateMachineBehaviour
{
    #region variables
    [SerializeField] float moveSpeed;    
    [HideInInspector] public int timerValue;
    int x,y;
      
    Transform player;
    SceneChange sChange;
    GridTiles[,] grid;
    InGameUI UI;

    bool awake = true;
    public bool canMove;
    #endregion


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            UI = FindObjectOfType<InGameUI>();
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            sChange = FindObjectOfType<SceneChange>();
        }
        canMove = true;
        x = animator.GetInteger("TargetInfoX");
        y = animator.GetInteger("TargetInfoY");        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
            

        if(canMove)
            Move(animator,stateInfo);
    }

    void Move(Animator anim, AnimatorStateInfo stateInfo)
    {
        float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(x, y));
        if (distance > 0f && grid[x, y].walkable)
        {
            Vector3 moveDir = (new Vector3(x, /*1.5f + grid[x, y].transform.position.y*/player.position.y, y) - player.position).normalized;
            player.position += moveDir * moveSpeed * Time.deltaTime;

            if (distance < 0.1f)
            {
                player.position = new Vector3(x, player.position.y/*1.5f + grid[x, y].transform.position.y*/, y);
            }
        }
        else
        {
            UI.timerValue++;
            TileEffectOnMove(x, y);
            if (stateInfo.IsName("Move"))
            {
                anim.SetTrigger("moveToTempo");
            }            
            else if (stateInfo.IsName("MoveOntoNormal"))
            {
                canMove = false;
                anim.SetBool("OntonormalTileMove", false);
            }
        }
    }

    void TileEffectOnMove(int x, int y)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");
        timerValue++;


        if (grid[x, y].levelTransiIndex != 0)
            sChange.startCoroutine(grid[x, y]);

        /*if (grid[x, y].key != 0)       
            KeyBehavior(grid[x, y]);*/

        /*if (currentPathIndex < highlightedTiles.Count -1)
        {
            if (highlightedTiles[currentPathIndex].crumble && highlightedTiles[currentPathIndex].walkable)        
                CrumbleBehavior(highlightedTiles[currentPathIndex]);
        }   */
    }


}
