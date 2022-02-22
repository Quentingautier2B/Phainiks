using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridTiles : MonoBehaviour
{
    
    [TextArea(minLines: 0, maxLines: 20)]
    [SerializeField] string Notes = "Comment Here.";

    #region variables
    [Header("Accessible Values")]
    [HideInInspector]public int step;
    public bool walkable;
    [HideInInspector] public bool highLight;
    public bool originalPosition;
    public int key; 
    public int door;
    public bool crumble;
    public int levelTransiIndex;
    public int timerChangeInputValue;
    public int tempoTile;
    [HideInInspector] public int timerChangeValue;
    [HideInInspector] public int height;
    [SerializeField] bool errorPause;

    [Space]
    [Header("Components")]
    Renderer rend;
    GameObject gameManager;
    PathHighlighter pathHighlighter;
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;
    LoopCycle loopCycle;


    #endregion

    private void Awake()
    {
        rend = GetComponent<Renderer>();


        timerChangeValue = timerChangeInputValue;
        if(timerChangeValue < 0)
        {
            transform.Find("Timer+").GetComponent<Renderer>().material.color = Color.red;
          
            transform.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystemRenderer>().material.color = Color.black;
        }


        height = (int)transform.position.y;
        gameManager = FindObjectOfType<GridGenerator>().gameObject;
        gridGenerator = gameManager.GetComponent<GridGenerator>();
        pathHighlighter = gameManager.GetComponent<PathHighlighter>();
        playerMovement = gameManager.GetComponent<PlayerMovement>();
        loopCycle = gameManager.GetComponent<LoopCycle>();

        if (key!=0)
        {
            if (!transform.Find("Key"))
            {
                print("Key Bloc with no Key" + this.name);  
                Time.timeScale = 0;

                //ErrorPause ne fait rien
                errorPause = true;
            }
        }
        if(door!=0)
        {
            if (!transform.Find("Door"))
            {
                Error("Door Bloc with no Door" + this.name);
            }
        }

        if (!errorPause)
        {
            Time.timeScale = 1;
        }
    }

    void Error(string errorText)
    {
        print(errorText);
        Time.timeScale = 0;

        //ErrorPause ne fait rien
        errorPause = true;
    }

    void Start()
    {
/*        if (!walkable && door == 0)
        {
            transform.Find("Step").Find("Text(TMP)").gameObject.SetActive(false);
            rend.enabled = false;
        }*/

        if(transform.position.x < 0 || transform.position.z < 0)
        {
            Error("Cube in negative position" + this.name);
        }
    }

    void Update()
    {
        var text = step.ToString();
        transform.Find("Step").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = text;
        if (height != (int)transform.position.y)
            height = (int)transform.position.y;

        if (highLight)
            Highlight();
        if (!highLight)
            UnHighlight();

        if (walkable && !rend.enabled )
        {
            rend.enabled = true;
            transform.Find("Step").Find("Text (TMP)").gameObject.SetActive(true);
        }


        if (!walkable && rend.enabled && door == 0 )
        {
            transform.Find("Step").Find("Text (TMP)").gameObject.SetActive(false);  
            rend.enabled = false;
        }

        tempoChange();
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
        if (walkable && step>-1)
        {
            playerMovement.moveFlag = true;
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

    void tempoChange()
    {
        if(loopCycle.tempoIndex == tempoTile && tempoTile!=0)
        {
            walkable = true;
        }      
        

        if (loopCycle.tempoIndex != tempoTile && tempoTile != 0)
            walkable = false;
    }
}
