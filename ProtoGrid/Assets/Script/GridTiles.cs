using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiles : MonoBehaviour
{
    public int step;
    public bool walkable;
    public bool highLight;
    GameObject gameManager;
    PathHighlighter pathHighlighter;
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;
    
    

    private void Awake()
    {
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
        if (highLight)
            Highlight();
        if (!highLight)
            UnHighlight();
    }

    private void OnMouseOver()
    {
     
        
        pathHighlighter.PathAssignment((int)transform.position.x, (int)transform.position.z, step);

    }

    private void OnMouseExit()
    {
        foreach (GridTiles obj in gridGenerator.grid)
        {
            obj.highLight = false;
        }
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

    private void OnMouseDown()
    {
        playerMovement.moveFlag = true;
    }

    void UnHighlight()
    {
        transform.Find("highlight").gameObject.SetActive(false);
        
    }


}
