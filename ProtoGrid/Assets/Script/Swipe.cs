using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    Vector2 startTouchPos, currentTouchPos, endTouchPos, directionSwipe;
    bool goodDirection = false;
    int pPosX, pPosY;
    int targetPosx, targetPosy;
    public float deadZoneDiameter;
    public bool movestate = false;
    
    StepAssignement stepA;
    PlayerMovement pMov;
    Transform player;
    GridGenerator gridG;
    GridTiles[,] grid;
    private void Awake()
    {
        stepA = GetComponent<StepAssignement>();
        player = FindObjectOfType<Player>().transform;
        gridG = GetComponent<GridGenerator>();
        pMov = GetComponent<PlayerMovement>();
        
    }

    private void Start()
    {
        grid = gridG.grid;
        pPosAssignement();    
    }

    void pPosAssignement()
    {
        pPosX = (int)player.position.x;
        pPosY = (int)player.position.z;
    }



    void Update()
    {
        if (movestate)
        {
            pMov.Move(targetPosx, targetPosy);
        }


        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPos = Input.mousePosition;
        }


        //Ajouter un timer en condition
        if (Input.GetMouseButtonUp(0) && Vector2.Distance(currentTouchPos, startTouchPos)>=deadZoneDiameter)
        {

            
            endTouchPos = Input.mousePosition;
            directionSwipe = -(startTouchPos - endTouchPos).normalized;
            pPosAssignement();
            int x = pPosX;
            int y = pPosY;
            
            if(directionSwipe.x > 0 && directionSwipe.y > 0)
            {
                if (x + 1 < gridG.raws && grid[x + 1, y] && grid[x + 1, y].step > -1 && (grid[x + 1, y].height - grid[x, y].height == 0 || grid[x + 1, y].height - grid[x, y].height == -1) && grid[x + 1, y].walkable)
                {
                    goodDirection = true;
                    if (goodDirection)
                    {
                        print("en haut a droite");
                        targetPosx = x + 1;
                        targetPosy = y;
                        movestate = true;
                        goodDirection = false;
                    }
                }
                else
                {
                    print("NAN en haut a droite");
                    goodDirection = false;
                }
            }            
            
            if(directionSwipe.x > 0 && directionSwipe.y < 0)
            {
                if (y - 1 > -1 && grid[x, y - 1] && grid[x, y-1].step > -1 && (grid[x, y-1].height - grid[x, y].height == 0 || grid[x, y-1].height - grid[x, y].height == -1) && grid[x, y-1].walkable)
                {
                    goodDirection = true;
                    if (goodDirection)
                    {
                        print("en bas a droite");
                        targetPosx = x;
                        targetPosy = y-1;
                        movestate = true;
                        goodDirection = false;
                    }
                }
                else
                {
                    print("NAN en bas a droite");
                    goodDirection = false;
                }
            }            
            
            if(directionSwipe.x < 0 && directionSwipe.y > 0)
            {
                if (y + 1 < gridG.columns && grid[x, y+1] && grid[x, y+1].step > -1 && (grid[x, y+1].height - grid[x, y].height == 0 || grid[x, y+1].height - grid[x, y].height == -1) && grid[x, y+1].walkable)
                {
                    goodDirection = true;
                    if (goodDirection)
                    {
                        print("en haut a gauche");
                        targetPosx = x;
                        targetPosy = y + 1;
                        movestate = true;
                        goodDirection = false;
                    }
                }
                else
                {
                    print("NAN en haut a gauche");
                    goodDirection = false;
                }
            }            
            
            if(directionSwipe.x < 0 && directionSwipe.y < 0)
            {
                if (x - 1 > -1 && grid[x - 1, y] && grid[x - 1, y].step > -1 && (grid[x - 1, y].height - grid[x, y].height == 0 || grid[x - 1, y].height - grid[x, y].height == -1) && grid[x - 1, y].walkable)
                {
                    goodDirection = true;
                    if (goodDirection)
                    {
                        print("en bas a gauche");
                        targetPosx = x - 1;
                        targetPosy = y;
                        movestate = true;
                        goodDirection = false;
                    }
                }
                else
                {
                    print("en bas a gauche");
                    goodDirection = false;
                }
            }

        }
    }
}
