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


    [SerializeField] bool redFlager,blueFlager,greenFlager;
    float redTarget, blueTarget, greenTarget;
    public float tempoTileSpeed;    
    bool redTest, blueTest, greenTest;
    GridGenerator gridG;
    GridTiles[,] grid;
    bool awake = true;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            grid = FindObjectOfType<GridGenerator>().grid;
            t = FindObjectOfType<TileVariables>();
            awake = false;
            
        }

        foreach(GridTiles tile in grid)
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

    void TempoTileIncr()
    {
        if (t.redFlag)
            t.redTimer--;

        if (!t.redFlag)
            t.redTimer++;




        if (t.blueFlag)
            t.blueTimer--;

        if (!t.blueFlag)
            t.blueTimer++;




        if (t.greenFlag)
            t.greenTimer--;

        if (!t.greenFlag)
            t.greenTimer++;
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
                        tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);
                       // greenTempoTileFlag = true;
                        greenFlager = false;
                    }
                }
            }
          
        }
        #endregion
    }
}
