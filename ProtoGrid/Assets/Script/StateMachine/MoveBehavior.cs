using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveBehavior : StateMachineBehaviour
{
    #region variables
    [SerializeField] float moveSpeed;    
    [HideInInspector] public int timerValue;
    int x,y;
    public AnimationCurve moveAnimation, landAnimation;
    GridGenerator gridG;
    Transform player;
    SceneChange sChange;
    GridTiles[,] grid;
    InGameUI UI;
    DoCoroutine doC;
    float lerper;
    int previousX;
    int previousY;
    bool flag;
    bool awake = true;
    public bool canMove;
    Vector3 startPos;
    SceneChange sceneChange;
    SkinnedMeshRenderer pSRend;
    Quaternion startRot, endRot;
    bool yFlag;
    bool endFlag;
    #endregion


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lerper = 0;
        if (awake)
        {
            endFlag = false;
            gridG = GridGenerator.Instance;
            pSRend = gridG.skinnedMeshRenderer;
            sceneChange = gridG.sceneChange;
            doC = gridG.doCoroutine;
            UI = gridG.inGameUI;
            grid = gridG.grid;
            player = gridG.player.transform;
            sChange = gridG.sceneChange;
        }
        startPos = player.position;
        startRot = player.rotation;
        yFlag = true;
        endRot = Quaternion.AngleAxis(90, player.forward);
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
        if (lerper < 1 && grid[x, y].walkable)
        {
            lerper += Time.deltaTime * moveSpeed;
            pSRend.transform.localPosition = new Vector3(0,moveAnimation.Evaluate(lerper) * 1, 0);
            pSRend.transform.rotation = Quaternion.Lerp(startRot, endRot, lerper);
            if (yFlag && ((startPos.y - 1.5f) - grid[x, y].transform.position.y == 1 || (startPos.y - 1.5f) - grid[x, y].transform.position.y == -1))
            {
                doC.StartCoroutine(player.GetComponent<Player>().Lerper(startPos.y + 1.5f, grid[x, y].transform.position.y + 1.5f)) ;
                yFlag = false;
            }
            if(lerper <= .5f)
            {
                pSRend.SetBlendShapeWeight(0,Mathf.Lerp(0,100,lerper));

            }
            else
            {
                pSRend.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, lerper));
            }

            player.position = Vector3.Lerp(startPos, new Vector3(grid[x, y].transform.position.x, player.position.y, grid[x, y].transform.position.z), lerper);

            if(lerper <.2f)
                player.LookAt(new Vector3(x, player.position.y, y));

        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");
            pSRend.transform.rotation = Quaternion.identity;
            pSRend.transform.Rotate(90, 0, 0);
            doC.StartCoroutine(doC.lerping());
            player.position = new Vector3(x, player.position.y , y);
            if (anim.GetBool("Rewind"))
            {
                //UI.timerValue++;
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
        if (sceneChange.Hub)
        {
            if (grid[anim.GetInteger("PreviousX"), anim.GetInteger("PreviousY")].World > 0)
            {
                grid[anim.GetInteger("PreviousX"), anim.GetInteger("PreviousY")].gameObject.transform.Find("World/CanvasCam").gameObject.SetActive(false);
            }
            if (grid[x, y].World > 0 && grid[x, y].gameObject.transform.Find("World/CanvasCam"))
            {
                grid[x, y].gameObject.transform.Find("World/CanvasCam").gameObject.SetActive(true);
            }
        }
        else if (grid[x, y].levelTransiIndex != 0)
        {
            if(grid[x, y].levelTransiIndex >= 1)
                sChange.StartCoroutine(sChange.Lerper(UI.startPosX, UI.endPosX));

            foreach (GridTiles tile in grid)
            {
                if (!endFlag)
                {
                    endFlag = true;
                    doC.StartCoroutine(doC.QueueForOg(grid[x, y].transform.position.y, 0, grid[x, y].transform, grid[x,y]));
                }

                if (tile.levelTransiIndex != grid[x, y].levelTransiIndex)
                {
                    tile.levelTransiIndex = 100;
                    doC.startClose(tile, tile.tiling, grid[x,y].levelTransiIndex, grid[x,y].GetComponent<GridTiling>());
                }
            }
        }
            

        //sChange.startCoroutine(grid[x, y]);

        if (anim.GetBool("Rewind") && grid[previousX, previousY].teleporter != 0)
        {
            player.position = new Vector3(grid[previousX, previousY].TpTarget.transform.position.x, grid[previousX, previousY].TpTarget.transform.position.y + 1.5f, grid[previousX, previousY].TpTarget.transform.position.z);
        }
        else if (grid[x, y].teleporter != 0 && !anim.GetBool("Rewind"))
        {
            Transform body = player.Find("Body");
            Transform tpRend = grid[x, y].TpTarget.transform.Find("Teleporter");
            FMODUnity.RuntimeManager.PlayOneShot("event:/World/TP");
            doC.StartCoroutine(doC.tpScaling(tpRend, 0.7f, 0, 0.8f, 4));
            doC.StartCoroutine(doC.PlayerCoroutine(player, body,grid[x,y].transform, grid[x, y].TpTarget.transform, player.position.y + 4.5f, player.position.y - 5.5f,1,1, pSRend));
            //player.position = new Vector3(grid[x, y].TpTarget.transform.position.x, grid[x, y].TpTarget.transform.position.y + 1.5f, grid[x, y].TpTarget.transform.position.z);
        }

        if(grid[previousX, previousY].teleporter != 0)
        {
            //Transform tpRend = grid[previousX, previousY].TpTarget.transform.Find("Teleporter");
            doC.StartCoroutine(doC.tpScaling(doC.previousTP, 0, 0.7f, 0, 2));
        }

        if (anim.GetBool("Rewind") && grid[previousX, previousY].key != 0)
        {
            KeyBehavior(grid[previousX, previousY], anim);
        }  
        else if (grid[x, y].key != 0)
            KeyBehavior(grid[x, y], anim);

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

        /*if (anim.GetBool("Rewind"))
        { 
            if (grid[previousX, previousY].key !=0)
            {
                KeyBehavior(grid[previousX, previousY]);
            }
        }*/

        if (grid[previousX,previousY].key != 0)
        {   
            grid[previousX, previousY].transform.Find("Key").position += new Vector3(0, 0.05f, 0);
        }
    }

    void KeyBehavior(GridTiles tile, Animator anim)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/GetKey");
       if ((anim.GetBool("Rewind") && grid[x,y].key !=0) || !anim.GetBool("Rewind"))
            tile.transform.Find("Key").position -= new Vector3(0, 0.05f, 0);

        if (!anim.GetBool("Rewind") || !(grid[x, y].key != 0))
        {
            //tile.transform.Find("Key").gameObject.SetActive(false);
            foreach (GridTiles t in grid)
            {
                if(t.door == tile.key && t.door > 0)
                {
                   doC.startClose(t, t.tiling, t.levelTransiIndex, t.GetComponent<GridTiling>());
                }
            }
        }
    }


}
