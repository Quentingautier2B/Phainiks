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
    Transform player;
    

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
        reset = GetComponent<Reset>();
        stepAssignement = GetComponent<StepAssignement>();
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
            Move();
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
    private void Move()
    {
        DestroyDoor();
        
        if (highlightedTiles.Count != 0)
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
                reset.resetTimer -= 1;
            }

        }
        if (highlightedTiles.Count <= 0)
        {
            moveState = false;
            stepAssignement.Initialisation();
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
        tile.timerChangeValue = 0;
    } 
    
    void TimerMinusBehavior(GridTiles tile)
    {
        
        FindObjectOfType<BoxCollider2D>().GetComponent<TextMeshProUGUI>().text = "" + tile.timerChangeValue;
        tile.transform.Find("Timer+").Find("TimerGoOver").GetComponent<ParticleSystem>().Play();
        reset.resetTimer += tile.timerChangeValue;
    }
    #endregion
}
