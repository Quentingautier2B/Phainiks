using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridTiles : MonoBehaviour
{
    
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";

    #region variables
    [Header("Accessible Values")]
    public int step;
    public bool walkable;
    public bool highLight;
    public bool originalPosition;
    public int height;

    [Space]
    [Header("Accessible Values")]
    GameObject gameManager;
    PathHighlighter pathHighlighter;
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;
    #endregion

    private void Awake()
    {
        height = (int)transform.position.y;
        gameManager = FindObjectOfType<GridGenerator>().gameObject;
        gridGenerator = gameManager.GetComponent<GridGenerator>();
        pathHighlighter = gameManager.GetComponent<PathHighlighter>();
        playerMovement = gameManager.GetComponent<PlayerMovement>();       
    }

    void Start()
    {
        if (!walkable)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Update()
    {
        if (height != (int)transform.position.y)
            height = (int)transform.position.y;

        if (highLight)
            Highlight();
        if (!highLight)
            UnHighlight();
    }

    private void OnMouseOver()
    {
     
        if(step>-1 && !playerMovement.moveState && step != 0) 
            pathHighlighter.PathAssignment((int)transform.position.x, (int)transform.position.z, (int)height, step);

    }

    private void OnMouseExit()
    {
        if (!playerMovement.moveState)
        {
            foreach (GridTiles obj in gridGenerator.grid)
            {
                obj.highLight = false;
            }
        }
    }

    private void OnMouseDown()
    {
        playerMovement.moveFlag = true;
    }

    private void OnDrawGizmos()
    {
        if (!walkable)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (walkable)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void Highlight()
    {
        transform.Find("highlight").gameObject.SetActive(true);
    }

    void UnHighlight()
    {
        transform.Find("highlight").gameObject.SetActive(false);
        
    }


}
