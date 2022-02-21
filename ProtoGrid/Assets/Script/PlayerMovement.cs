using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
public class PlayerMovement : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    #region variables

    [Header("Input Values")]
    [SerializeField] float moveSpeed;
    public Vector3 ogPos;

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
        player.gameObject.GetComponent<NavMeshAgent>().SetDestination(new Vector3(highlightedTiles[0].transform.position.x, player.position.y, highlightedTiles[0].transform.position.z));
        if (player.position.x == highlightedTiles[0].transform.position.x && player.position.z == highlightedTiles[0].transform.position.z)
        {
            highlightedTiles[0].highLight = false;
            highlightedTiles.RemoveAt(0);
            reset.resetTimer -= 1;
        }

        if (highlightedTiles.Count == 0)
        {
            moveState = false;
            stepAssignement.Initialisation();
        }
    }
}
