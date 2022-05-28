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
    DoCoroutine doC;
    GridTiling gT;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            gridG = FindObjectOfType<GridGenerator>();
            doC = FindObjectOfType<DoCoroutine>();
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
                    redFlager = LerpPos(tile, redFlager, t.redFlag, tile.GetComponent<GridTiling>(), "Tile Rouge");                
            }
           
        }

        if (blueFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 2)
                    blueFlager = LerpPos(tile, blueFlager, t.blueFlag, tile.GetComponent<GridTiling>(), "Tile Bleu");
            }
        }

        if (greenFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 3)
                    greenFlager = LerpPos(tile, greenFlager, t.greenFlag, tile.GetComponent<GridTiling>(), "Tile Vert");               
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
                        gT = grid[x, y].GetComponent<GridTiling>();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/World/Ascenseur");
                        grid[x, y].target = (int)grid[x, y].transform.position.y + 2;
                        grid[x, y].tempoBool = false;
                        grid[x,y].opening = true;
                    }
                    gT.SetDirectionalMaterial();
                    UpdateAdjacentTileColonnes(grid[x, y], (int)grid[x, y].transform.position.x, (int)grid[x, y].transform.position.z, gT);
                    grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, Mathf.Lerp(grid[x, y].transform.position.y, grid[x, y].target, tempoTileSpeed * Time.deltaTime), grid[x, y].transform.position.z);

                    if (grid[x, y].transform.position.y >= grid[x, y].target - 0.01f)
                    {
                        grid[x, y].opening = false;
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, grid[x, y].target, grid[x, y].transform.position.z);
                        gT.SetDirectionalMaterial();
                        grid[x, y].crumbleBool = false;
                        crumbleFlager = false;
                    }
                }

                if (!grid[x, y].crumbleUp && !anim.GetBool("Rewind") || grid[x, y].crumbleUp && anim.GetBool("Rewind"))
                {
                    if (grid[x, y].tempoBool)
                    {
                        gT = grid[x, y].GetComponent<GridTiling>();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/World/Ascenseur");
                        grid[x, y].target = (int)grid[x, y].transform.position.y - 2;
                        grid[x, y].tempoBool = false;
                        grid[x, y].opening = true;
                    }
                    gT.SetDirectionalMaterial();
                    UpdateAdjacentTileColonnes(grid[x, y], (int)grid[x, y].transform.position.x, (int)grid[x, y].transform.position.z, gT);
                    grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, Mathf.Lerp(grid[x, y].transform.position.y, grid[x, y].target, tempoTileSpeed * Time.deltaTime), grid[x, y].transform.position.z);
                        /* tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
                        tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();*/
                    if (grid[x, y].transform.position.y <= grid[x, y].target + 0.01f)
                    {
                        grid[x, y].opening = false;
                        //debugTools.greenMusic.setVolume(0);
                        grid[x, y].transform.position = new Vector3(grid[x, y].transform.position.x, grid[x, y].target, grid[x, y].transform.position.z);
                        gT.SetDirectionalMaterial();
                        // greenTempoTileFlag = true;
                        grid[x, y].crumbleBool = false;
                        crumbleFlager = false;
                    }
                }
        }
        #endregion
    }

    bool LerpPos(GridTiles tile, bool Flager, bool colorFlag, GridTiling g, string musicName)
    {
            //Called on First Loop
            if (tile.tempoBool && colorFlag)
            {
                doC.UpdateAdjTiles(tile, (int)tile.transform.position.x, (int)tile.transform.position.z);
                tile.target = (int)tile.transform.position.y + 2;
                tile.opening = true;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(musicName, 1);
                tile.tempoBool = false;
            }
            else if(tile.tempoBool && !colorFlag)
            {
                doC.UpdateAdjTiles(tile, (int)tile.transform.position.x, (int)tile.transform.position.z);
                tile.target = (int)tile.transform.position.y - 2;
                tile.opening = true;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(musicName, 0);
                tile.tempoBool = false;
            }

        //Called Every loop
            g.TempoTileMaterial();
            tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
            UpdateAdjacentTileColonnes(tile, (int)tile.transform.position.x, (int)tile.transform.position.z, gT);

            //Called on last loop
            if ((tile.transform.position.y >= tile.target - 0.01f && colorFlag) || (tile.transform.position.y <= tile.target + 0.01f && !colorFlag))
                {
                    tile.opening = false;

                    tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);               
                    return false;
                }
            else
            {
                return true;
            }
        
       
    }

    void UpdateAdjacentTileColonnes(GridTiles g, int x, int y, GridTiling gT)
    {
        if(x + 1 < gridG.raws && grid[x + 1, y] != null && grid[x + 1, y].walkable && grid[x + 1, y].tempoTile == 0)
        {
            /* gridG.TestDirection(x + 1, y, 4);
             grid[x + 1, y].GetComponent<GridTiling>().SetCubeSize();*/
            grid[x + 1, y].tiling.SetDirectionalMaterial();
        }

        if (y - 1 > -1 && grid[x, y - 1] != null && grid[x, y - 1].walkable && grid[x, y - 1].tempoTile == 0)
        {
            /*            gridG.TestDirection(x, y - 1, 3);
                        grid[x, y - 1].GetComponent<GridTiling>().SetCubeSize();*/
            grid[x, y - 1].tiling.SetDirectionalMaterial();

        }

        if (y + 1 < gridG.columns && grid[x, y + 1] != null && grid[x, y + 1].walkable && grid[x, y + 1].tempoTile == 0)
        {
            /*            gridG.TestDirection(x, y + 1, 2);
                        grid[x, y + 1].GetComponent<GridTiling>().SetCubeSize();*/
            grid[x, y + 1].tiling.SetDirectionalMaterial();

        }

        if (x - 1 > -1 && grid[x - 1, y] != null && grid[x - 1, y].walkable && grid[x - 1, y].tempoTile == 0)
        {
            /*            gridG.TestDirection(x - 1, y, 1);
                        grid[x - 1, y].GetComponent<GridTiling>().SetCubeSize();*/
            grid[x - 1, y].tiling.SetDirectionalMaterial();

        }
    }
}
