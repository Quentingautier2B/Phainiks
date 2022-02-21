using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    #region variables

    [Header("Input Values")]
    [SerializeField] float moveSpeed;
    public Vector3 ogPos;
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

    [Space]
    [Header("Lists")]
    public List<GridTiles> highlightedTiles;
    GridTiles[,] grid;

    


    #endregion

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
        highlightedTiles[0].highLight = false;
        highlightedTiles.RemoveAt(0);
        moveFlag = false;
        moveState = true;
    }

    private void Move()
    {


        if (highlightedTiles[currentPathIndex].transform.Find("Door"))
        {
            Destroy(highlightedTiles[currentPathIndex].transform.Find("Door").gameObject);
        }
        if (highlightedTiles != null)
        {
            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(highlightedTiles[currentPathIndex].transform.position.x, highlightedTiles[currentPathIndex].transform.position.z));
            if (distance > 0f)
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
       // highlightedTiles[currentPathIndex].highLight = false;
        if (highlightedTiles[currentPathIndex].key)       
            KeyBehavior(highlightedTiles[currentPathIndex]);
      
        if (highlightedTiles[currentPathIndex].levelEnd)        
            EndBehavior(highlightedTiles[currentPathIndex]);

        if (highlightedTiles[currentPathIndex].timerPlusValue > 0)
            TimerPlusBehavior(highlightedTiles[currentPathIndex]);

        if (highlightedTiles[currentPathIndex].timerPlusValue < 0)
            TimerMinusBehavior(highlightedTiles[currentPathIndex]);

        // reset.resetTimer -= 1;
    }

    void KeyBehavior(GridTiles tile)
    {
        tile.key = false;
        tile.transform.Find("Key").gameObject.SetActive(false);
        player.GetComponent<Player>().Inventory.Add("key" + tile.transform.Find("Key").GetComponent<KeyScript>().keyIndex);
    }

    void EndBehavior(GridTiles tile)
    {
        print("end level");
    }

    void TimerPlusBehavior(GridTiles tile)
    {
        tile.transform.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystem>().Stop();
        FindObjectOfType<BoxCollider2D>().GetComponent<TextMeshProUGUI>().text = "+" + tile.timerPlusValue;
        tile.transform.Find("Timer+").Find("TimerGoOver").GetComponent<ParticleSystem>().Play();
        reset.resetTimer += tile.timerPlusValue;
        tile.timerPlusValue = 0;
    } 
    
    void TimerMinusBehavior(GridTiles tile)
    {
        
        FindObjectOfType<BoxCollider2D>().GetComponent<TextMeshProUGUI>().text = "" + tile.timerPlusValue;
        tile.transform.Find("Timer+").Find("TimerGoOver").GetComponent<ParticleSystem>().Play();
        reset.resetTimer += tile.timerPlusValue;
    }
}
