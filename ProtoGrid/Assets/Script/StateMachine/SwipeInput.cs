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

    DoCoroutine doC;
    Transform player;
    GridGenerator gridG;
    GridTiles[,] grid;
    TileVariables temp;
    CameraBehavior cam;
    bool awake = true;
    [HideInInspector]public Vector2 roundingDirectionalYPosition;
    static public List<Vector2> rewindPos = new List<Vector2>();
    
 

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (awake)
        {        
            doC = animator.GetComponent<DoCoroutine>();
            grid = FindObjectOfType<GridGenerator>().grid;
            foreach (GridTiles g in grid)
            {
                if (!g.originalPosition)
                {                  
                    doC.startClose(g, g.levelTransiIndex);
                }

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
     


        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            clickBool = true;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && Vector2.Distance(currentTouchPos, startTouchPos) >= deadZoneDiameter /*&& !clickBool*/)
        {
            endTouchPos = Input.mousePosition;
            directionSwipe = -(startTouchPos - endTouchPos).normalized;
            if (cam.rotateMode == 0)
            {
                //directionSwipe = directionSwipe;
            }
            else if (cam.rotateMode == 1)
            {
                directionSwipe = Quaternion.AngleAxis(90, -Vector3.forward) * directionSwipe;
            }
            else if (cam.rotateMode == 2)
            {
                directionSwipe = Quaternion.AngleAxis(180, -Vector3.forward) * directionSwipe;
            }
            else if (cam.rotateMode == 3)
            {
                directionSwipe = Quaternion.AngleAxis(270, -Vector3.forward) * directionSwipe;
            }
            else
            {
                Debug.LogError("Prob avce l'angle de swipe, modulo pas correct");
            }
            pPosAssignement();
            TestFourDirections(animator);
        }

        if (Input.GetMouseButtonUp(0) && clickBool)
        {
           
            if(directionIndex > 0)
            {
                //pPosAssignement();
                //TestFourDirections(animator);
            }
            else { }
        }

        if (clickBool)
        {
            clickTimer -= Time.deltaTime;
            if(clickTimer <=0)
            {
                clickBool = false;
                clickTimer = clickTimerValue;
            }
        }

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
        }

    }
}
