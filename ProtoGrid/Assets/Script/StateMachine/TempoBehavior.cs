using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TempoBehavior : StateMachineBehaviour
{
    //public int redTimer = 0;
    int redOffValue = 1;
    //public bool redFlag;

   //public int blueTimer = 0;
   int blueOffValue = 2;
   //public bool blueFlag;

   //public int greenTimer = 0;
    int greenOffValue = 3;
   //public bool greenFlag;

    TileVariables t;


    [SerializeField] bool redFlager,blueFlager,greenFlager, crumbleFlager;
    float redTarget, blueTarget, greenTarget;
    public float tempoTileSpeed;    
    bool redTest, blueTest, greenTest;
    GridGenerator gridG;
    GridTiles[,] grid;
    bool awake = true;
    DebugTools debugTools;
    InGameUI UI;
    int x, y;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {

            debugTools = FindObjectOfType<DebugTools>();
            grid = FindObjectOfType<GridGenerator>().grid;
            t = FindObjectOfType<TileVariables>();
            UI = FindObjectOfType<InGameUI>();
            awake = false;
        }
 
        if (animator.GetBool("Rewind"))
        {
            x = (int)SwipeInput.rewindPos[UI.timerValue - 1].x;
            y = (int)SwipeInput.rewindPos[UI.timerValue - 1].x;
        }
        else 
        { 
            x = animator.GetInteger("PreviousX");
            y = animator.GetInteger("PreviousY");
        }

        if(grid[x, y].crumble)
        {
            crumbleFlager = true;
        }

        foreach (GridTiles tile in grid)
        {
            tile.tempoBool = true;
            
        }
        //redTempoTileFlag = true;
        //blueTempoTileFlag = true;
        //greenTempoTileFlag = true;
        redTest = false;
        blueTest = false;
        greenTest = false;

        foreach(GridTiles tile in grid)
        {
            if (tile.tempoTile == 1)            
                redTest = true;

            if (tile.tempoTile == 2)
                blueTest = true;

            if (tile.tempoTile == 3)
                greenTest = true;
        }
        if (animator.GetBool("Rewind"))
        {
            NewTempoTile(animator);
            TempoTileIncr(animator);
        }
        else
        {
            TempoTileIncr(animator);
            NewTempoTile(animator);
        }
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tempoChange();

        if(!redFlager && !blueFlager && !greenFlager && !crumbleFlager)
        {
            if (animator.GetBool("Rewind"))
            {
                animator.SetTrigger("tempoToMove");
            }
            else
            {
                if (stateInfo.IsName("Tempo"))
                {
                    animator.SetBool("OntoTempoTile", false);
                }
                else if (stateInfo.IsName("MoveOntoNormal"))
                {

                    animator.SetBool("OntonormalTileTempo", false);
                }
            }

            
        }
    }



    void NewTempoTile(Animator anim)
    {
        if (redTest)
        {
                
             if (t.redTimer <= 0)
             {
                t.redFlag = false;
                redFlager = true;
             }

             if (t.redTimer >= redOffValue)
             {
                t.redFlag = true;
                redFlager = true;
             }
            
        }


        if (blueTest)
        {
            if (t.blueTimer <= 0 )
            {
                t.blueFlag = false;
                blueFlager = true;
            }

            if (t.blueTimer >= blueOffValue)
            {
                t.blueFlag = true;
                blueFlager = true;
            }

        }

        

        if (greenTest)
        {
            if (anim.GetBool("Rewind"))
            {
                if (t.greenTimer <= 0)
                {
                    t.greenFlag = true;
                    
                }

                if (t.greenTimer >= greenOffValue)
                {
                    t.greenFlag = false;
                    
                }
                if (t.greenTimer == greenOffValue || t.greenTimer == 0)
                {
                    greenFlager = true;
                }
            }
            else
            {
                if (t.greenTimer <= 0)
                {
                    t.greenFlag = false;
                    greenFlager = true;
                }

                if (t.greenTimer >= greenOffValue)
                {
                    t.greenFlag = true;
                    greenFlager = true;
                }
            }
        }

    }

    void TempoTileIncr(Animator anim)
    {
        if (t.redFlag)
            t.redTimer--;

        if (!t.redFlag)
            t.redTimer++;




        if (t.blueFlag)
            t.blueTimer--;

        if (!t.blueFlag)
            t.blueTimer++;


        if (anim.GetBool("Rewind"))
        {
            if (t.greenFlag)
                t.greenTimer++;

            if (!t.greenFlag)
                t.greenTimer--;
        }
        else
        {
            if (t.greenFlag)
                t.greenTimer--;

            if (!t.greenFlag)
                t.greenTimer++;
        }
            
    }

    void tempoChange()
    {
        #region redTempoMove
        if (redFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 1 && t.redFlag)
                {
                    
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y + 2;
                        tile.tempoBool = false;
                        
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    
                    if (tile.transform.position.y >= tile.target - 0.01f)
                    {
                        debugTools.redMusic.setVolume(1);
                        //debugTools.redMusic.start();
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                        //redTempoTileFlag = true;
                        redFlager = false;
                    }
                }

                if (tile.tempoTile == 1 && !t.redFlag)
                {
                    
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y - 2;
                        tile.tempoBool = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= tile.target + 0.01f)
                    {
                        debugTools.redMusic.setVolume(0);
                        //debugTools.redMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                     
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                        //redTempoTileFlag = true;
                        redFlager = false;
                    }
                }
            }
           
        }
        #endregion

        #region blueTempoMove
        if (blueFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 2 && t.blueFlag)
                {
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y + 2;
                        tile.tempoBool = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y >= tile.target - 0.01f)
                    {
                        debugTools.blueMusic.setVolume(1);
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                        //blueTempoTileFlag = true;
                        blueFlager = false;
                    }
                }

                if (tile.tempoTile == 2 && !t.blueFlag)
                {
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y - 2;
                        tile.tempoBool = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= tile.target + 0.01f)
                    {
                        debugTools.blueMusic.setVolume(0);
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                        //blueTempoTileFlag = true;
                        blueFlager = false;
                    }
                }
            }
           
        }
        #endregion

        #region greenRegionMove
        if (greenFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 3 && t.greenFlag)
                {
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y + 2;
                        tile.tempoBool = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y >= tile.target - 0.01f)
                    {
                        debugTools.greenMusic.setVolume(1);
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                        //greenTempoTileFlag = true;
                        greenFlager = false;
                    }
                }

                if (tile.tempoTile == 3 && !t.greenFlag)
                {
                    if (tile.tempoBool)
                    {
                        tile.target = (int)tile.transform.position.y - 2;
                        tile.tempoBool = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= tile.target + 0.01f)
                    {
                        debugTools.greenMusic.setVolume(0);
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                       // greenTempoTileFlag = true;
                        greenFlager = false;
                    }
                }
            }
          
        }
        #endregion

        #region crumbleRegionMove
        if (grid[x,y].crumble)
        {
           
                if (grid[x, y].crumbleUp)
                {
                    if (grid[x, y].tempoBool)
                    {
                        grid[x, y].target = (int)grid[x, y].transform.position.y + 2;
                        grid[x, y].tempoBool = false;
                    }
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, Mathf.Lerp(grid[x, y].transform.position.y, grid[x, y].target, tempoTileSpeed * Time.deltaTime), grid[x, y].transform.position.z);
/*                      tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                        tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();*/
                    if (grid[x, y].transform.position.y >= grid[x, y].target - 0.01f)
                    {
                        //debugTools.greenMusic.setVolume(1);
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, grid[x, y].target, grid[x, y].transform.position.z);
                        grid[x, y].crumbleBool = false;
                        crumbleFlager = false;
                    }
                }

                if (!grid[x, y].crumbleUp)
                {
                    if (grid[x, y].tempoBool)
                    {
                        grid[x, y].target = (int)grid[x, y].transform.position.y - 2;
                        grid[x, y].tempoBool = false;
                    }
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, Mathf.Lerp(grid[x, y].transform.position.y, grid[x, y].target, tempoTileSpeed * Time.deltaTime), grid[x, y].transform.position.z);
                        /* tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                        tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();*/
                    if (grid[x, y].transform.position.y <= grid[x, y].target + 0.01f)
                    {
                        //debugTools.greenMusic.setVolume(0);
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, grid[x, y].target, grid[x, y].transform.position.z);
                        // greenTempoTileFlag = true;
                        grid[x, y].crumbleBool = false;
                        crumbleFlager = false;
                    }
                }
            

        }
        #endregion
    }
}
