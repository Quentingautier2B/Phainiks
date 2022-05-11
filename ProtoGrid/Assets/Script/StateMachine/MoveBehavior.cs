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
    DoCoroutine doC;

    int previousX;
    int previousY;
    bool flag;
    bool awake = true;
    public bool canMove;
    #endregion


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            doC = animator.GetComponent<DoCoroutine>();
            UI = FindObjectOfType<InGameUI>();
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            sChange = FindObjectOfType<SceneChange>();
        }
        canMove = true;
        if (animator.GetBool("Rewind"))
        {
            x = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].x;
            y = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].y;
        }
        else
        {
            x = animator.GetInteger("TargetInfoX");
            y = animator.GetInteger("TargetInfoY");
        }

        previousX = animator.GetInteger("PreviousX");
        previousY = animator.GetInteger("PreviousY");

        flag = true;
               
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(canMove)
        {
            Move(animator,stateInfo);
        }

    }


    void Move(Animator anim, AnimatorStateInfo stateInfo)
    {
        if (anim.GetBool("Rewind") && flag == true)
        {
            flag = false;
            TileEffectOnMove(x, y, anim);
        }

        float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(x, y));
        if (distance > 0f && grid[x, y].walkable)
        {
            Vector3 moveDir = (new Vector3(x, /*1.5f + grid[x, y].transform.position.y*/player.position.y, y) - player.position).normalized;
            
            player.position += moveDir * moveSpeed * Time.deltaTime;
            if(distance > 0.5f)
                player.LookAt(new Vector3(x, player.position.y/*1.5f + grid[x, y].transform.position.y*/, y));

            if (distance < 0.3f)
            {
                player.position = new Vector3(x, player.position.y/*1.5f + grid[x, y].transform.position.y*/, y);
                /*if (anim.GetBool("Rewind"))
                    Debug.Log(SwipeInput.rewindPos[SwipeInput.rewindPos.Count-1]);*/
            }
        }
        else
        {
            if (anim.GetBool("Rewind"))
            {
                UI.timerValue--;
            }
            else
            {
                UI.timerValue++;
            }

            if (!anim.GetBool("Rewind"))
            {
                TileEffectOnMove(x, y, anim);
            }

            if (anim.GetBool("Rewind"))
            {
                anim.SetBool("Rewind", false);
                SwipeInput.rewindPos.RemoveAt(SwipeInput.rewindPos.Count - 1);
            }

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

    void TileEffectOnMove(int x, int y, Animator anim)
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");
        //timerValue++;

        if (grid[x, y].levelTransiIndex != 0)
            doC.startClose(grid[x,y]);

        //sChange.startCoroutine(grid[x, y]);

        if (anim.GetBool("Rewind") && grid[previousX, previousY].teleporter != 0)
        {
            player.position = new Vector3(grid[previousX, previousY].TpTarget.transform.position.x, grid[previousX, previousY].TpTarget.transform.position.y + 1.5f, grid[previousX, previousY].TpTarget.transform.position.z);
        }
        else if (grid[x, y].teleporter != 0 && !anim.GetBool("Rewind"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/World/TP");
            player.position = new Vector3(grid[x, y].TpTarget.transform.position.x, grid[x, y].TpTarget.transform.position.y + 1.5f, grid[x, y].TpTarget.transform.position.z);
        }

        
        if (grid[x, y].key != 0)
            KeyBehavior(grid[x, y]);

        if (anim.GetBool("Rewind") && grid[previousX, previousY].crumble)
        {
            grid[previousX, previousY].crumbleBool = true;
            if (grid[previousX, previousY].crumbleUp)
                grid[previousX, previousY].crumbleUp = false;
            else if (!grid[previousX, previousY].crumbleUp)
                grid[previousX, previousY].crumbleUp = true;
        }
        else if (grid[x, y].crumble)
        {
            grid[x, y].crumbleBool = true;
            if (grid[x, y].crumbleUp)
                grid[x, y].crumbleUp = false;
            else if (!grid[x, y].crumbleUp)
                grid[x, y].crumbleUp = true;
        }

        if (anim.GetBool("Rewind"))
        { 
            if (grid[previousX, previousY].key !=0)
            {
                KeyBehavior(grid[previousX, previousY]);
            }
        }
    }

    void KeyBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/GetKey");
        //tile.transform.Find("Key").gameObject.SetActive(false);
        foreach (GridTiles t in grid)
        {
            if(t.door == tile.key && t.door > 0)
            {
               doC.startClose(t);
            }
        }
    }
}
