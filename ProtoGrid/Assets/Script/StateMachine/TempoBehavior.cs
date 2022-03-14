using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoBehavior : StateMachineBehaviour
{
    public int redTimer = 0;
    int redOffValue = 1;
    public bool redFlag;

   public int blueTimer = 0;
   int blueOffValue = 2;
   public bool blueFlag;

   public int greenTimer = 0;
    int greenOffValue = 3;
   public bool greenFlag;


    [SerializeField] bool redFlager,blueFlager,greenFlager;
    float redTarget, blueTarget, greenTarget;
    public float tempoTileSpeed;
    bool redTempoTileFlag, blueTempoTileFlag, greenTempoTileFlag;
    bool redTest, blueTest, greenTest;
    GridGenerator gridG;
    GridTiles[,] grid;
    bool awake = true;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            grid = FindObjectOfType<GridGenerator>().grid;
            awake = false;
            
        }

        
        redTempoTileFlag = true;
        blueTempoTileFlag = true;
        greenTempoTileFlag = true;
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

        TempoTileIncr();
        NewTempoTile();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tempoChange();

        if(!redFlager && !blueFlager && !greenFlager)
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



    void NewTempoTile()
    {
        if (redTest)
        {
            if (redTimer <= 0)
            {
                redFlag = false;
                redFlager = true;
            }

            if (redTimer >= redOffValue)
            {
                redFlag = true;
                redFlager = true;
            }
        }


        if (blueTest)
        {
            if (blueTimer <= 0 )
            {
                blueFlag = false;
                blueFlager = true;
            }

            if (blueTimer >= blueOffValue)
            {
                blueFlag = true;
                blueFlager = true;
            }

        }



        if (greenTest)
        {
            if (greenTimer <= 0)
            {
                greenFlag = false;
                greenFlager = true;
            }

            if (greenTimer >= greenOffValue)
            {
                greenFlag = true;
                greenFlager = true;
            }
        }

    }

    void TempoTileIncr()
    {
        if (redFlag)
            redTimer--;

        if (!redFlag)
            redTimer++;




        if (blueFlag)
            blueTimer--;

        if (!blueFlag)
            blueTimer++;




        if (greenFlag)
            greenTimer--;

        if (!greenFlag)
            greenTimer++;
    }

    void tempoChange()
    {
        #region redTempoMove
        if (redFlager)
        {
            foreach (GridTiles tile in grid)
            {
                if (tile.tempoTile == 1 && redFlag)
                {
                    
                    if (redTempoTileFlag)
                    {
                        redTarget = tile.transform.position.y + 2;
                        redTempoTileFlag = false;
                        
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, redTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    
                    if (tile.transform.position.y >= redTarget - 0.01f)
                    {
                        
                        tile.transform.position = new Vector3(tile.transform.position.x, redTarget, tile.transform.position.z);
                        //redTempoTileFlag = true;
                        redFlager = false;
                    }
                }

                if (tile.tempoTile == 1 && !redFlag)
                {
                    
                    if (redTempoTileFlag)
                    {
                        redTarget = tile.transform.position.y - 2;
                        redTempoTileFlag = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, redTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= redTarget + 0.01f)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, redTarget, tile.transform.position.z);
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
                if (tile.tempoTile == 2 && blueFlag)
                {
                    if (blueTempoTileFlag)
                    {
                        blueTarget = tile.transform.position.y + 2;
                        blueTempoTileFlag = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, blueTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y >= blueTarget - 0.01f)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, blueTarget, tile.transform.position.z);
                        //blueTempoTileFlag = true;
                        blueFlager = false;
                    }
                }

                if (tile.tempoTile == 2 && !blueFlag)
                {
                    if (blueTempoTileFlag)
                    {
                        blueTarget = tile.transform.position.y - 2;
                        blueTempoTileFlag = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, blueTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= blueTarget + 0.01f)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, blueTarget, tile.transform.position.z);
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
                if (tile.tempoTile == 3 && greenFlag)
                {
                    if (greenTempoTileFlag)
                    {
                        greenTarget = tile.transform.position.y + 2;
                        greenTempoTileFlag = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, greenTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y >= greenTarget - 0.01f)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, greenTarget, tile.transform.position.z);
                        greenTempoTileFlag = true;
                        greenFlager = false;
                    }
                }

                if (tile.tempoTile == 3 && !greenFlag)
                {
                    if (greenTempoTileFlag)
                    {
                        greenTarget = tile.transform.position.y - 2;
                        greenTempoTileFlag = false;
                    }
                    tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, greenTarget, tempoTileSpeed * Time.deltaTime), tile.transform.position.z);
                    tile.transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
                    tile.transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
                    if (tile.transform.position.y <= greenTarget + 0.01f)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, greenTarget, tile.transform.position.z);
                        greenTempoTileFlag = true;
                        greenFlager = false;
                    }
                }
            }
          
        }
        #endregion
    }
}
