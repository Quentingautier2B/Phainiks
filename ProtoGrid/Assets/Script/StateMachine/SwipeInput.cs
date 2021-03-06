using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : StateMachineBehaviour
{
    Vector2 startTouchPos, currentTouchPos, endTouchPos, directionSwipe;
    
    int pPosX, pPosY;
    int targetPosx, targetPosy;
    public float deadZoneDiameter;
    int directionIndex = 0;
    public float clickTimerValue;
    float clickTimer;
    bool clickBool;
    
    Transform player;
    GridGenerator gridG;
    GridTiles[,] grid;
    TileVariables temp;
    bool awake = true;
    [HideInInspector]public Vector2 roundingDirectionalYPosition;
  
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            player = FindObjectOfType<Player>().transform;
            gridG = FindObjectOfType<GridGenerator>();
            grid = gridG.grid;
            temp = FindObjectOfType<TileVariables>();
            clickTimer = clickTimerValue;
            awake = false;
        }
     
        pPosAssignement();
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
            pPosAssignement();
            TestFourDirections(animator);
        }

        if(Input.GetMouseButtonUp(0) && clickBool)
        {
            Debug.Log(directionSwipe);
            if(directionIndex > 0)
            {
                
                pPosAssignement();
                TestFourDirections(animator);
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



    void pPosAssignement()
    {
        pPosX = (int)player.position.x;
        pPosY = (int)player.position.z;
    }

    void TestFourDirections(Animator anim)
    {
        if (!TestDirection(pPosX, pPosY, 1) && !TestDirection(pPosX, pPosY, 2) && !TestDirection(pPosX, pPosY, 3) && !TestDirection(pPosX, pPosY, 4))
        {           
            //anim.SetBool("OntoTempoTile", true);
        }
            

        if (directionSwipe.x > 0 && directionSwipe.y > 0)
        {
            if (TestDirection(pPosX, pPosY, 1))
                {
                roundingDirectionalYPosition = new Vector2(0, 0);
                anim.SetInteger("TargetInfoX", pPosX + 1);
                anim.SetInteger("TargetInfoY", pPosY);
                directionIndex = 1;
                //anim.SetBool("OntoTempoTile", true);
                if ((grid[pPosX + 1, pPosY].tempoTile == 1) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX + 1, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag))
                {
                    anim.SetBool("OntoTempoTile", true);
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                }
            }
                
        }

        if (directionSwipe.x > 0 && directionSwipe.y < 0)
        {
            if (TestDirection(pPosX, pPosY, 2))
            {
                roundingDirectionalYPosition = new Vector2(0, 1);
                anim.SetInteger("TargetInfoX", pPosX);
                anim.SetInteger("TargetInfoY", pPosY - 1);
                //anim.SetBool("OntoTempoTile", true);
                directionIndex = 1;
                if ((grid[pPosX, pPosY - 1].tempoTile == 1) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY - 1].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag))
                {
                    anim.SetBool("OntoTempoTile", true);
                }
                else
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                }
            }
        }

        if (directionSwipe.x < 0 && directionSwipe.y > 0)
        {
            if (TestDirection(pPosX, pPosY, 3))
            {
                roundingDirectionalYPosition = new Vector2(1, 0);
                anim.SetInteger("TargetInfoX", pPosX);
                anim.SetInteger("TargetInfoY", pPosY + 1);
                //anim.SetBool("OntoTempoTile", true);
                directionIndex = 1;
                if ((grid[pPosX, pPosY + 1].tempoTile == 1) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY + 1].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag))
                {
                    anim.SetBool("OntoTempoTile", true);
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                }


            }
        }

        if (directionSwipe.x < 0 && directionSwipe.y < 0)
        {
            if (TestDirection(pPosX, pPosY, 4))
            {
                roundingDirectionalYPosition = new Vector2(1, 1);
                anim.SetInteger("TargetInfoX", pPosX - 1);
                anim.SetInteger("TargetInfoY", pPosY);
                //anim.SetBool("OntoTempoTile", true);
                directionIndex = 1;
                if ((grid[pPosX - 1, pPosY].tempoTile == 1) || 
                   (grid[pPosX - 1, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX - 1, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX - 1, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag) || 
                   (grid[pPosX, pPosY].tempoTile == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 2 && temp.blueTimer == 1) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 1 && temp.greenFlag) ||
                   (grid[pPosX, pPosY].tempoTile == 3 && temp.greenTimer == 2 && !temp.greenFlag))
                {
                    anim.SetBool("OntoTempoTile", true);
                }
                else 
                {
                    anim.SetBool("OntonormalTileMove", true);
                    anim.SetBool("OntonormalTileTempo", true);
                }
            }
        }
    }

    bool TestDirection(int x, int y, int direction)
    {
        switch (direction)
        {
            case 1:

                if (x + 1 < gridG.raws && grid[x + 1, y] && grid[x + 1, y].step > -1 && (grid[x + 1, y].height - grid[x, y].height == 0 || grid[x + 1, y].height - grid[x, y].height == -1) && grid[x + 1, y].walkable)
                    return true;
                else
                    return false;


            case 2:
                if (y - 1 > -1 && grid[x, y - 1] && grid[x, y - 1].step > -1 && (grid[x, y - 1].height - grid[x, y].height == 0 || grid[x, y - 1].height - grid[x, y].height == -1) && grid[x, y - 1].walkable)
                    return true;
                else
                    return false;

            case 3:
                if (y + 1 < gridG.columns && grid[x, y + 1] && grid[x, y + 1].step > -1 && (grid[x, y + 1].height - grid[x, y].height == 0 || grid[x, y + 1].height - grid[x, y].height == -1) && grid[x, y + 1].walkable)
                    return true;
                else
                    return false;


            case 4:
                if (x - 1 > -1 && grid[x - 1, y] && grid[x - 1, y].step > -1 && (grid[x - 1, y].height - grid[x, y].height == 0 || grid[x - 1, y].height - grid[x, y].height == -1) && grid[x - 1, y].walkable)
                    return true;
                else
                    return false;

            default:
                return false;
        }
    }
}
