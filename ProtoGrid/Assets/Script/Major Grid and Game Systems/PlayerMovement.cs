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
    [HideInInspector] public int timerValue;


    [Space]
    [Header("Components")]        
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
        swipe = GetComponent<Swipe>();     
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
    }
    #endregion

    #region moveMethods
    public void Move(int x, int y)
    {        
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
            TileEffectOnMove(x,y);
            swipe.StateMachine.SetBool("OntoTempoTile", false);
            swipe.StateMachine.SetBool("OntonormalTile", false);
            
        }
    }

    void TileEffectOnMove(int x, int y)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");
        timerValue++;
        /*if (highlightedTiles[currentPathIndex].key != 0)       
            KeyBehavior(highlightedTiles[currentPathIndex]);*/
      
        
        
        if (grid[x,y].levelTransiIndex != 0)        
            StartCoroutine(EndBehavior(grid[x,y]));



        /*if (currentPathIndex < highlightedTiles.Count -1)
        {
            if (highlightedTiles[currentPathIndex].crumble && highlightedTiles[currentPathIndex].walkable)        
                CrumbleBehavior(highlightedTiles[currentPathIndex]);
        }   */    
    }
    #endregion

    #region TileBehavior
    IEnumerator EndBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Lvl_" + tile.levelTransiIndex, LoadSceneMode.Single);
    } 
   
    
/*    void KeyBehavior(GridTiles tile)
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
      */
    void FirstCrumble()
    {
        //Values enleve quand on a enleve le step assignement
        var firstCrumble = grid[1, 1];
        if (firstCrumble.walkable && firstCrumble.crumble)
            firstCrumble.walkable = false;
    }

    void CrumbleBehavior(GridTiles tile)
    {
        tile.walkable = false;
    }
    #endregion
}
