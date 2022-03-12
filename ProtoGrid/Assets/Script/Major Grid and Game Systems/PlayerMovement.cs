using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";

    #region variables

    [Header("Input Values")]
    [SerializeField] float moveSpeed;
    [HideInInspector] public Vector3 ogPos;
    public int currentPathIndex = 0;

    [Space]
    [Header("Components")]
    [SerializeField] StepAssignement stepAssignement;
    Reset reset;
    Swipe swipe;
    Transform player;
    LoopCycle lC;
    GridGenerator gG;
    

    [Space]
    [Header("Booleans")]
    public bool moveFlag;
    public bool moveState = false;
    bool stopFlag = true;

    [Space]
    [Header("Lists")]
    public List<GridTiles> highlightedTiles;
    GridTiles[,] grid;

    #endregion

    #region callMethods
    private void Awake()
    {
        gG = GetComponent<GridGenerator>();
        reset = GetComponent<Reset>();
        swipe = GetComponent<Swipe>();
        stepAssignement = GetComponent<StepAssignement>();
        lC = GetComponent<LoopCycle>();
        grid = FindObjectOfType<GridGenerator>().grid;
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        
        foreach(GridTiles obj in grid)
        {
            if (obj.originalPosition)
            {
                ogPos = new Vector3(obj.transform.position.x,player.position.y,obj.transform.position.z);
                player.position = ogPos;
            }
        }
        stepAssignement.Initialisation();
    }

    private void Update()
    {
        if (moveFlag)
        {
            FindHighlighted();       
        }
        else if (moveState)
        {
            //Move();
        }
    }
    #endregion

    #region moveMethods
    void FindHighlighted()
    {
        //highlightedTiles = new List<GridTiles>();
        foreach(GridTiles obj in grid)
        {

            if (obj.highLight)
            {
                highlightedTiles.Add(obj);              
            }
            
        }
        highlightedTiles = highlightedTiles.OrderBy(x => x.step).ToList();
        if (highlightedTiles.Count != 0)
        {
            highlightedTiles[0].highLight = false;
            highlightedTiles.RemoveAt(0);
        }
        moveFlag = false;
        moveState = true;
    }



    public void Move(int x, int y)
    {
        //DestroyDoor();

        /*if (highlightedTiles.Count != 0)
        {
            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(highlightedTiles[currentPathIndex].transform.position.x, highlightedTiles[currentPathIndex].transform.position.z));
            if (distance > 0f && highlightedTiles[currentPathIndex].walkable)
            {
                Vector3 moveDir = (new Vector3(highlightedTiles[currentPathIndex].transform.position.x, 1.5f + highlightedTiles[currentPathIndex].transform.position.y, highlightedTiles[currentPathIndex].transform.position.z) - player.position).normalized;
                player.position += moveDir * moveSpeed * Time.deltaTime;
                
                if (distance < 0.1f)
                {
                    player.position = new Vector3(highlightedTiles[currentPathIndex].transform.position.x, 1.5f + highlightedTiles[currentPathIndex].transform.position.y, highlightedTiles[currentPathIndex].transform.position.z);
                }
            }
            else
            {
                highlightedTiles[currentPathIndex].highLight = false;
                TileEffectOnMove();
                FirstCrumble();
                currentPathIndex++;
                if (currentPathIndex >= highlightedTiles.Count)
                {
                    highlightedTiles.Clear();
                    currentPathIndex = 0;
                }
                reset.resetTimer += 1;
            }

        }
        if (highlightedTiles.Count <= 0)
        {
            moveState = false;
            stepAssignement.Initialisation();
        }*/

        print(grid[x, y].name);
        float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(x, y));
        if (distance > 0f && grid[x, y].walkable)
        {
            Vector3 moveDir = (new Vector3(x, 1.5f + grid[x, y].transform.position.y, y) - player.position).normalized;
            player.position += moveDir * moveSpeed * Time.deltaTime;

            if (distance < 0.1f)
            {
                player.position = new Vector3(x, 1.5f + grid[x, y].transform.position.y, y);
            }
        }
        else
        {
            //TileEffectOnMove();
            swipe.movestate = false;
        }
    }
    void TileEffectOnMove()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");

        if (highlightedTiles[currentPathIndex].key != 0)       
            KeyBehavior(highlightedTiles[currentPathIndex]);
      
        if (highlightedTiles[currentPathIndex].levelTransiIndex != 0)        
            StartCoroutine(EndBehavior(highlightedTiles[currentPathIndex]));



        if (currentPathIndex < highlightedTiles.Count -1)
        {
            if (highlightedTiles[currentPathIndex].crumble && highlightedTiles[currentPathIndex].walkable)        
                CrumbleBehavior(highlightedTiles[currentPathIndex]);
        }

        if (highlightedTiles[currentPathIndex].timerChangeValue > 0)
            TimerPlusBehavior(highlightedTiles[currentPathIndex]);

        if (highlightedTiles[currentPathIndex].timerChangeValue < 0)
            TimerMinusBehavior(highlightedTiles[currentPathIndex]);
    }
    #endregion

    #region TileBehavior
    void KeyBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/PickupItem");
        player.GetComponent<Player>().Inventory.Add("key" + tile.key);
        tile.key = 0;
        tile.transform.Find("Key").gameObject.SetActive(false);
    }
        
    void DestroyDoor()
    {
        if(highlightedTiles.Count != 0)
        {
            if (highlightedTiles[currentPathIndex].transform.Find("Door"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/World/DoorOpen");
                Destroy(highlightedTiles[currentPathIndex].transform.Find("Door").gameObject);
            }
        }
    }
   
    IEnumerator EndBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Lvl_" + tile.levelTransiIndex, LoadSceneMode.Single);
    } 
    


    void FirstCrumble()
    {
        var firstCrumble = grid[stepAssignement.startPosX, stepAssignement.startPosY];
        if (firstCrumble.walkable && firstCrumble.crumble)
            firstCrumble.walkable = false;
    }

    void CrumbleBehavior(GridTiles tile)
    {
        tile.walkable = false;
    }

    void TimerPlusBehavior(GridTiles tile)
    {
        tile.transform.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystem>().Stop();
        FindObjectOfType<BoxCollider2D>().GetComponent<TextMeshProUGUI>().text = "+" + tile.timerChangeValue;
        tile.transform.Find("Timer+").Find("TimerGoOver").GetComponent<ParticleSystem>().Play();
        reset.resetTimer += tile.timerChangeValue;
        //tile.timerChangeValue = 0;
        

        foreach(GridTiles tiled in gG.grid)
        {
            if (tiled.tempoTile == 1)
            {

                if (!lC.redFlag)
                {
                    lC.redTimer++;
                    if(lC.redTimer > lC.redOffValue)
                    {
                        lC.redTimer = lC.redOffValue - (lC.redTimer - lC.redOffValue);
                    }
                    /*lC.redTimer--;
                                        if (((((tile.timerChangeValue + lC.redTimer) % lC.redOffValue) - tile.timerChangeValue - lC.redTimer) / lC.redOffValue) % 2 == 0)
                                        {

                                            lC.redTimer = (tile.timerChangeValue + lC.redTimer) % lC.redOffValue;
                                            lC.redFlag = true;
                                        }
                                        else
                                        {

                                            lC.redTimer = lC.redOffValue - (tile.timerChangeValue + lC.redTimer) % lC.redOffValue;
                                            lC.redFlag = false;
                                        }*/
                }

                if (lC.redFlag)
                {
                    lC.redTimer--;
                    if (lC.redTimer < 0)
                    {
                        lC.redTimer *= -1;
                    }
                    /*                    if (((((tile.timerChangeValue + lC.redTimer) % lC.redOffValue) - tile.timerChangeValue - lC.redTimer) / lC.redOffValue) % 2 == 0)
                                        {

                                            lC.redTimer = lC.redOffValue-(tile.timerChangeValue + lC.redTimer) % lC.redOffValue;
                                            lC.redFlag = false;
                                        }
                                        else
                                        {

                                            lC.redTimer = (tile.timerChangeValue + lC.redTimer) % lC.redOffValue;
                                            lC.redFlag = true;
                                        }*/
                }
            }


            if (tiled.tempoTile == 2)
            {

                if (!lC.blueFlag)
                {
                    lC.blueTimer++;
                    if (lC.blueTimer > lC.blueOffValue)
                    {
                        lC.blueTimer = lC.blueOffValue - (lC.blueTimer - lC.blueOffValue);
                    }
                    /* if (((((tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue) - tile.timerChangeValue - lC.blueTimer) / lC.blueOffValue) % 2 == 0)
                     {

                         lC.blueTimer = (tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue;
                         lC.blueFlag = true;
                     }
                     else
                     {

                         lC.blueTimer = (tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue;
                         lC.blueFlag = false;
                     }*/
                }

                if (lC.blueFlag)
                {
                    lC.blueTimer--;
                    if (lC.blueTimer < 0)
                    {
                        lC.blueTimer *=-1;
                    }
                    /*if (((((tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue) - tile.timerChangeValue - lC.blueTimer) / lC.blueOffValue) % 2 == 0)
                    {

                        lC.blueTimer = (tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue;
                        lC.blueFlag = false;
                    }
                    else
                    {

                        lC.blueTimer = (tile.timerChangeValue + lC.blueTimer) % lC.blueOffValue;
                        lC.blueFlag = true;
                    }*/
                }
            }

            if (tiled.tempoTile == 3)
            {

                if (lC.greenFlag)
                {

                    if (((((tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue) - tile.timerChangeValue - lC.greenTimer) / lC.greenOffValue) % 2 == 0)
                    {

                        lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                        lC.greenFlag = true;
                    }
                    else
                    {

                        lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                        lC.greenFlag = false;
                    }
                }

                if (!lC.greenFlag)
                {

                    if (((((tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue) - tile.timerChangeValue - lC.greenTimer) / lC.greenOffValue) % 2 == 0)
                    {

                        lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                        lC.greenFlag = false;
                    }
                    else
                    {

                        lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                        lC.greenFlag = true;
                    }
                }
            }
        }
    }


    
    
    void TimerMinusBehavior(GridTiles tile)
    {
        
        FindObjectOfType<BoxCollider2D>().GetComponent<TextMeshProUGUI>().text = "" + tile.timerChangeValue;
        tile.transform.Find("Timer+").Find("TimerGoOver").GetComponent<ParticleSystem>().Play();
        reset.resetTimer += tile.timerChangeValue;
        print("a");
        if(tile.tempoTile == 3)
        {
            print("b");
            if (!lC.greenFlag)
            {
                lC.greenTimer++;
                
                if (lC.greenTimer > lC.greenOffValue)
                {
                    lC.greenTimer = lC.greenOffValue - (lC.greenTimer - lC.greenOffValue);
                }
                /*print("c");
                if (((((tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue) - tile.timerChangeValue - lC.greenTimer) / lC.greenOffValue) % 2 == 0)
                {
                    lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                    lC.greenFlag = true;
                }
                else
                {
                    lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                    lC.greenFlag = false;
                }*/
            }

            if (lC.greenFlag)
            {
                lC.greenTimer--;
                
                if (lC.greenTimer <0)
                {
                    lC.greenTimer *=-1;
                }
                /*              print("d");
                              if (((((tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue) - tile.timerChangeValue - lC.greenTimer) / lC.greenOffValue) % 2 == 0)
                              {
                                  lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                                  lC.greenFlag = false;
                              }
                              else
                              {
                                  lC.greenTimer = (tile.timerChangeValue + lC.greenTimer) % lC.greenOffValue;
                                  lC.greenFlag = true;
                              }*/
            }
        }
    }
    #endregion
}
