using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] StepAssignement stepAssignement;
    public bool moveFlag;
    bool moveState = false;
    public List<GridTiles> highlightedTiles;
    GridTiles[,] grid;
    Transform player;
    [SerializeField] float moveSpeed;
    private void Awake()
    {
        stepAssignement = GetComponent<StepAssignement>();
        grid = FindObjectOfType<GridGenerator>().grid;
        player = FindObjectOfType<Player>().transform;
    }
    private void Start()
    {
        stepAssignement.Initialisation();
    }

    private void Update()
    {
        if (moveFlag)
        {
            print(1);
            FindHighlighted();       
        }
        else if (moveState)
        {            
            player.position = Vector3.Lerp(player.position,new Vector3( highlightedTiles[0].transform.position.x,player.position.y, highlightedTiles[0].transform.position.z), Mathf.Clamp(moveSpeed/10,0,1));
            if(player.position.x == highlightedTiles[0].transform.position.x && player.position.z == highlightedTiles[0].transform.position.z)
            {
                
                highlightedTiles.RemoveAt(0);
            }
            
            if (highlightedTiles.Count == 0) 
            {
                moveState = false;
            }   
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
        highlightedTiles.RemoveAt(0);
        moveFlag = false;
        moveState = true;
    }
}
