using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    #region variables
    Vector2 startTouchPos, currentTouchPos, endTouchPos, directionSwipe;
    bool goodDirection = false;
    int pPosX, pPosY;
    int targetPosx, targetPosy;
    public float deadZoneDiameter;
    public bool movestate = false;
    

    PlayerMovement pMov;
    Transform player;
    GridGenerator gridG;
    GridTiles[,] grid;
    public Animator StateMachine;
    LoopCycle loopC;
    #endregion

    #region callMethods
    private void Awake()
    {
        loopC = GetComponent<LoopCycle>();
        StateMachine = FindObjectOfType<Animator>();
        player = FindObjectOfType<Player>().transform;
        gridG = GetComponent<GridGenerator>();
        pMov = GetComponent<PlayerMovement>();
        
    }

    private void Start()
    {
        grid = gridG.grid;
        pPosAssignement();    
    }

    void Update()
    {
        StateMachineSet();
        SwipeInput();
    }
    #endregion

    #region otherMethods
    void pPosAssignement()
    {
        pPosX = (int)player.position.x;
        pPosY = (int)player.position.z;
    }

    void SwipeInput()
    {
        if (StateMachine.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                currentTouchPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) && Vector2.Distance(currentTouchPos, startTouchPos) >= deadZoneDiameter)
            {
                endTouchPos = Input.mousePosition;
                directionSwipe = -(startTouchPos - endTouchPos).normalized;
                pPosAssignement();
                TestFourDirections();
            }
        }
    }

    void TestFourDirections()
    {
        if(!TestDirection(pPosX, pPosY, 1) && !TestDirection(pPosX, pPosY, 2) && !TestDirection(pPosX, pPosY, 3) && !TestDirection(pPosX, pPosY, 4))
            LaunchMovement(pPosX, pPosY);

        if (directionSwipe.x > 0 && directionSwipe.y > 0)
        {
            if (TestDirection(pPosX, pPosY, 1))
                LaunchMovement(pPosX + 1, pPosY);
        }

        if (directionSwipe.x > 0 && directionSwipe.y < 0)
        {
            if (TestDirection(pPosX, pPosY, 2))
                LaunchMovement(pPosX, pPosY - 1);
        }

        if (directionSwipe.x < 0 && directionSwipe.y > 0)
        {
            if (TestDirection(pPosX, pPosY, 3))
                LaunchMovement(pPosX, pPosY + 1);
        }

        if (directionSwipe.x < 0 && directionSwipe.y < 0)
        {
            if (TestDirection(pPosX, pPosY, 4))
                LaunchMovement(pPosX - 1, pPosY);
        }
    }

    bool TestDirection(int x, int y, int direction)
    {
        switch(direction)
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

            default :
                return false;
        }
    }

    void LaunchMovement(int x, int y)
    {
        goodDirection = true;
        if (goodDirection)
        {
            targetPosx = x;
            targetPosy = y;
            // une fois qu'on a le travelling remplacer ligne suivante par texte en commente tout en bas.
            StateMachine.SetBool("OntoTempoTile", true);
            goodDirection = false;
        }
        else
        {
            goodDirection = false;
        }
    }

    void StateMachineSet()
    {
        if (StateMachine.GetCurrentAnimatorStateInfo(0).IsName("MoveOntoNormal"))
        {
            pMov.Move(targetPosx, targetPosy);
            loopC.tempoTileIncr();

        }

        if (StateMachine.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            pMov.Move(targetPosx, targetPosy);

        }

        if (StateMachine.GetCurrentAnimatorStateInfo(0).IsName("Tempo"))
        {
            loopC.tempoTileIncr();
        }
    }
    #endregion

    //commentaire en dessous important ref methode de setup de mouvement au dessus
    /* if (grid[x -1, y].tempoTile > 0)
                        {
                            StateMachine.SetBool("OntoTempoTile", true);
                        }
                        else if (grid[x - 1, y].tempoTile == 0)
                        {
                            StateMachine.SetBool("OntonormalTile", true);
                        }*/
}
