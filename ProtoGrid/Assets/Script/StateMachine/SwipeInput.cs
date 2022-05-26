using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwipeInput : StateMachineBehaviour
{
    Vector2 startTouchPos, currentTouchPos, endTouchPos;
    public Vector2 directionSwipe;
    int pPosX, pPosY;
    int targetPosx, targetPosy;
    public float deadZoneDiameter;
    int directionIndex = 0;
    public float clickTimerValue;
    float clickTimer;
    bool clickBool;
    SkinnedMeshRenderer pSRend;
    DoCoroutine doC;
    Transform player;
    GridGenerator gridG;
    GridTiles[,] grid;
    TileVariables temp;
    CameraBehavior cam;
    bool awake = true;
    [HideInInspector]public Vector2 roundingDirectionalYPosition;
    static public List<Vector2> rewindPos = new List<Vector2>();
    InputSaver inputBuffer;
    float animIndexValue;
    bool monte; 

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        


        if (awake)
        {
            pSRend = FindObjectOfType<SkinnedMeshRenderer>();
            inputBuffer = FindObjectOfType<InputSaver>();
            doC = animator.GetComponent<DoCoroutine>();
            grid = FindObjectOfType<GridGenerator>().grid;
            GridTiling gTil = null;
            foreach(GridTiles g in grid)
            {
                if (g.originalPosition)
                {
                    gTil = g.GetComponent<GridTiling>();
                }
            }
            foreach (GridTiles g in grid)
            {
                if ((!g.originalPosition && g.open && g.door != 0) || (!g.originalPosition && g.door == 0))
                {                  
                    doC.startClose(g, g.levelTransiIndex, gTil);
                }
                /*if (g.door != 0 && g.open)
                    g.open = false;*/
                /*if (g.door != 0 && !g.open)
                    doC.startClose(g, g.levelTransiIndex);
                else
                    g.open = false;*/
            }
            player = FindObjectOfType<Player>().transform;
            gridG = FindObjectOfType<GridGenerator>();
            
            temp = FindObjectOfType<TileVariables>();
            clickTimer = clickTimerValue;
            awake = false;
            cam = FindObjectOfType<CameraBehavior>();
            rewindPos.Clear();
        }

        grid = gridG.grid;
        pPosAssignement();
        
        foreach(GridTiles g in grid)
        {
            
            if (g.walkable && g.open && g.tempoTile == 0)
            {
                g.GetComponent<GridTiling>().SetDirectionalMaterial();

            }
            if (g.walkable && (g.tempoTile != 0 || g.crumble) && g.open)
            {
                g.GetComponent<GridTiling>().TempoTileMaterial();
            }
           
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
       // Debug.Log(animIndexValue);

        //animIndexValue += Time.deltaTime * 150;
        if (monte)
        {
            animIndexValue += Time.deltaTime * 50;
        }
        else
        {
            animIndexValue -= Time.deltaTime * 50;
        }

        if(animIndexValue < 0 && !monte)
        {
            animIndexValue = 1;
            monte = true;
        }
        else if(animIndexValue > 50 && monte)
        {
            animIndexValue = 49;
            monte = false;
        }
        //pSRend.SetBlendShapeWeight(1, animIndexValue);
        if (doC.right)
        {
            pPosAssignement();
            HubTestRightDirections(animator);
            doC.right = false;
        }

        if (doC.left)
        {
            pPosAssignement();
            HubTestLeftDirections(animator);
            doC.left = false;
        }

        if (inputBuffer.SavedInput.Count > 0)
        {
            directionSwipe = inputBuffer.SavedInput[0];
            pPosAssignement();
            TestFourDirections(animator);
        }




    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animIndexValue = 0;
        monte = true;
        if (inputBuffer.SavedInput.Count > 0 && inputBuffer.SavedInput[0] != null)
            inputBuffer.SavedInput.RemoveAt(0);

        directionSwipe = Vector2.zero;
        if (animator.GetBool("Rewind"))
        {
            animator.SetInteger("PreviousX", pPosX);
            animator.SetInteger("PreviousY", pPosY);
        }
 
        animator.SetInteger("CurrentX", (int)player.position.x);
        animator.SetInteger("CurrentY", (int)player.position.z);
        
    }



    void pPosAssignement()
    {
        pPosX = (int)player.position.x;
        pPosY = (int)player.position.z;
        
    }

    void TestFourDirections(Animator anim)
    {
        if (directionSwipe.x > 0 && directionSwipe.y > 0)
        {
            
            if (gridG.TestDirection(pPosX, pPosY, 1))
            {
                roundingDirectionalYPosition = new Vector2(0, 0);
                anim.SetInteger("TargetInfoX", pPosX + 1);
                anim.SetInteger("TargetInfoY", pPosY);
                anim.SetInteger("PreviousX", pPosX);
                anim.SetInteger("PreviousY", pPosY);
                directionIndex = 1;
                
                if ((grid[pPosX + 1, pPosY].tempoTile == 1) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].crumble))
                {
                    anim.SetBool("OntoTempoTile", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
            }
            else
            {
                if (inputBuffer.SavedInput.Count > 0 && inputBuffer.SavedInput[0] != null)
                    inputBuffer.SavedInput.RemoveAt(0);
            }
        }

        if (directionSwipe.x > 0 && directionSwipe.y < 0)
        {
      
            if (gridG.TestDirection(pPosX, pPosY, 2))
            {
                roundingDirectionalYPosition = new Vector2(0, 1);
                anim.SetInteger("TargetInfoX", pPosX);
                anim.SetInteger("TargetInfoY", pPosY - 1);
                anim.SetInteger("PreviousX", pPosX);
                anim.SetInteger("PreviousY", pPosY);
                
                directionIndex = 1;
                if ((grid[pPosX, pPosY - 1].tempoTile == 1) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].crumble))
                {
                    anim.SetBool("OntoTempoTile", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
                else
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
            }
            else
            {
                if (inputBuffer.SavedInput.Count > 0 && inputBuffer.SavedInput[0] != null)
                    inputBuffer.SavedInput.RemoveAt(0);
            }
        }

        if (directionSwipe.x < 0 && directionSwipe.y > 0)
        {

            if (gridG.TestDirection(pPosX, pPosY, 3))
            {
                roundingDirectionalYPosition = new Vector2(1, 0);
                anim.SetInteger("TargetInfoX", pPosX);
                anim.SetInteger("TargetInfoY", pPosY + 1);
                anim.SetInteger("PreviousX", pPosX);
                anim.SetInteger("PreviousY", pPosY);
                
                directionIndex = 1;
                if ((grid[pPosX, pPosY + 1].tempoTile == 1) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].crumble))
                {
                    anim.SetBool("OntoTempoTile", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
            }
            else
            {
                if (inputBuffer.SavedInput.Count > 0 && inputBuffer.SavedInput[0] != null)
                    inputBuffer.SavedInput.RemoveAt(0);
            }
        }

        if (directionSwipe.x < 0 && directionSwipe.y < 0)
        {

            if (gridG.TestDirection(pPosX, pPosY, 4))
            {
                roundingDirectionalYPosition = new Vector2(1, 1);
                anim.SetInteger("TargetInfoX", pPosX - 1);
                anim.SetInteger("TargetInfoY", pPosY);
                anim.SetInteger("PreviousX", pPosX);
                anim.SetInteger("PreviousY", pPosY);
                
                directionIndex = 1;
                if ((grid[pPosX - 1, pPosY].tempoTile == 1) || 
                   (grid[pPosX - 1, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX - 1, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX - 1, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) || 
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].crumble))
                {
                    anim.SetBool("OntoTempoTile", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                    rewindPos.Add(new Vector2(pPosX, pPosY));
                }
            }
            else
            {
                if (inputBuffer.SavedInput.Count > 0 && inputBuffer.SavedInput[0] != null)
                    inputBuffer.SavedInput.RemoveAt(0);
            }
        }


    }

    void HubTestRightDirections(Animator anim)
    {
        roundingDirectionalYPosition = new Vector2(0, 0);
        anim.SetInteger("TargetInfoX", pPosX + 3);
        anim.SetInteger("TargetInfoY", pPosY);
        anim.SetInteger("PreviousX", pPosX);
        anim.SetInteger("PreviousY", pPosY);
        directionIndex = 1;
        anim.SetBool("OntonormalTileMove", true);
        anim.SetBool("OntonormalTileTempo", true);
        rewindPos.Add(new Vector2(pPosX, pPosY));
    }
    void HubTestLeftDirections(Animator anim)
    {
        roundingDirectionalYPosition = new Vector2(1, 1);
        anim.SetInteger("TargetInfoX", pPosX - 3);
        anim.SetInteger("TargetInfoY", pPosY);
        anim.SetInteger("PreviousX", pPosX);
        anim.SetInteger("PreviousY", pPosY);
        directionIndex = 1;
        anim.SetBool("OntonormalTileMove", true);
        anim.SetBool("OntonormalTileTempo", true);
        rewindPos.Add(new Vector2(pPosX, pPosY));
    }
}
