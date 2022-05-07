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


    [SerializeField] public bool redFlager,blueFlager,greenFlager, crumbleFlager;
    float redTarget, blueTarget, greenTarget;
    public float tempoTileSpeed;    
    bool redTest, blueTest, greenTest;
    GridGenerator gridG;
    GridTiles[,] grid;
    bool awake = true;
    DebugTools debugTools;
    InGameUI UI;
    public int x, y;

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
            x = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].x;
            y = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].y;

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
        tempoChange(animator);

        if(!redFlager && !blueFlager && !greenFlager && !crumbleFlager)
        {
            if (animator.GetBool("Rewind"))
            {
                animator.SetTrigger("tempoToMove");
            }
            
            
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



    void NewTempoTile(Animator anim)
    {
        if (redTest)
        {

            if (anim.GetBool("Rewind"))
            {
                if (t.redTimer <= 0)
                {
                    t.redFlag = true;
                    redFlager = true;
                }

                if (t.redTimer >= redOffValue)
                {
                    t.redFlag = false;
                    redFlager = true;
                }

            }
            else
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

        }


        if (blueTest)
        {
            if (anim.GetBool("Rewind"))
            {
                if (t.blueTimer <= 0)
                {
                    t.blueFlag = true;
                }

                if (t.blueTimer >= blueOffValue)
                {
                    t.blueFlag = false;
                }
                if (t.blueTimer == blueOffValue || t.blueTimer == 0)
                {
                    blueFlager = true;
                }

            }
            else
            {
                if (t.blueTimer <= 0)
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
        if (anim.GetBool("Rewind"))
        {
            if (t.redFlag)
                t.redTimer++;

            if (!t.redFlag)
                t.redTimer--;
        }
        else
        {
            if (t.redFlag)
                t.redTimer--;

            if (!t.redFlag)
                t.redTimer++;
        }
        



        if (anim.GetBool("Rewind"))
        {
            if (t.blueFlag)
                t.blueTimer++;

            if (!t.blueFlag)
                t.blueTimer--;
        }
        else
        {
            if (t.blueFlag)
                t.blueTimer--;

            if (!t.blueFlag)
                t.blueTimer++;
        }
        


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

    void tempoChange(Animator anim)
    {
        #region TempoMove
        if (redFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 1)
                    redFlager = LerpPos(tile, debugTools.redMusic, redFlager, t.redFlag);                
            }
           
        }

        if (blueFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 2)
                    blueFlager = LerpPos(tile, debugTools.blueMusic, blueFlager, t.blueFlag);
            }
        }

        if (greenFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 3)
                    greenFlager = LerpPos(tile, debugTools.greenMusic, greenFlager, t.greenFlag);               
            }
          
        }
        #endregion

        #region crumbleRegionMove
        if (grid[x,y].crumble)
        {
                if (grid[x, y].crumbleUp && !anim.GetBool("Rewind") || !grid[x, y].crumbleUp && anim.GetBool("Rewind"))
                {
                    if (grid[x, y].tempoBool)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/World/Ascenseur");
                        grid[x, y].target = (int)grid[x, y].transform.position.y + 2;
                        grid[x, y].tempoBool = false;
                    }
                
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, Mathf.Lerp(grid[x, y].transform.position.y, grid[x, y].target, tempoTileSpeed * Time.deltaTime), grid[x, y].transform.position.z);

                    if (grid[x, y].transform.position.y >= grid[x, y].target - 0.01f)
                    {
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, grid[x, y].target, grid[x, y].transform.position.z);
                        grid[x, y].crumbleBool = false;
                        crumbleFlager = false;
                    }
                }

                if (!grid[x, y].crumbleUp && !anim.GetBool("Rewind") || grid[x, y].crumbleUp && anim.GetBool("Rewind"))
                {
                    if (grid[x, y].tempoBool)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/World/Ascenseur");
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

    bool LerpPos(GridTiles tile, FMOD.Studio.EventInstance music, bool Flager, bool colorFlag)
    {
            //Called on First Loop
            if (tile.tempoBool && colorFlag)
            {
                tile.target = (int)tile.transform.position.y + 2;
                tile.tempoBool = false;
            }
            else if(tile.tempoBool && !colorFlag)
            {
                tile.target = (int)tile.transform.position.y - 2;
                tile.tempoBool = false;
            }

            //Called Every loop
            tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
     

            //Called on last loop
            if ((tile.transform.position.y >= tile.target - 0.01f && colorFlag) || (tile.transform.position.y <= tile.target + 0.01f && !colorFlag))
            {

                if (colorFlag)
                {
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    music.setVolume(1);
                }
                else
                {
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
                    music.setVolume(0);
                }

                tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);               
                return false;
            }
            else
            {
            return true;
            }
        
       
    }
}
