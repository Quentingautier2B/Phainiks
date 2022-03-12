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
    [Range(0, 5)]public int key;
    [Range(0, 5)] public int door;
    public bool crumble;
    [Range(0, 8)] public float levelTransiIndex;
    [Range(-100, 100)] public int timerChangeInputValue;
    [Header("Work in progress don't use")]
    public int tempoTile;

    bool redTile;
    bool blueTile;
    bool greenTile;
    
    [HideInInspector] public int timerChangeValue;
    [HideInInspector] public int height;

    [Space]
    [Header("Components")]
    Renderer rend;
    GameObject gameManager;
    DebugTools debugTools;
    PathHighlighter pathHighlighter;
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;
    StepAssignement stepAssignement;
    LoopCycle loopCycle;


    bool flager1 = true;   

    #endregion

    #region CallMethods
    private void Awake()
    {
        DebugHub();

        TimerValueSetUp();

        SetUpComponents();
    }

    private void Start()
    {
        if(door != 0)
        {
            walkable = false;
        }

    }

    void Update()
    {
        if (tempoTile == 1)
            redTile = true;

        if (tempoTile == 2)
            blueTile = true;

        if (tempoTile == 3)
            greenTile = true;

        SetUpDebugStepValue();

        HeightToInt();

        HighlightingHub();

        VisibleOrInvisibleTile();

        tempoChange();
    }

   /* private void OnMouseOver()
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
        if (walkable && step>-1 && playerMovement.highlightedTiles.Count == 0)
        {
            playerMovement.moveFlag = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Click");
        }
    }*/
    #endregion

    #region Methods
    void SetUpComponents()
    {
        height = (int)transform.position.y;
        gameManager = FindObjectOfType<GridGenerator>().gameObject;
        gridGenerator = gameManager.GetComponent<GridGenerator>();
        pathHighlighter = gameManager.GetComponent<PathHighlighter>();
        playerMovement = gameManager.GetComponent<PlayerMovement>();
        loopCycle = gameManager.GetComponent<LoopCycle>();
        stepAssignement = gameManager.GetComponent<StepAssignement>();
    }

    void HeightToInt()
    {
        if (height != (int)transform.position.y)
            height = (int)transform.position.y;
    }
   
    void DebugHub()
    {
        debugTools = FindObjectOfType<DebugTools>();

        rend = GetComponent<Renderer>();
        if (debugTools.debugModOn && !transform.Find("Step").gameObject.activeSelf)
        {
            transform.Find("Step").gameObject.SetActive(true);
        }

        if (!debugTools.debugModOn && transform.Find("Step").gameObject.activeSelf)
        {
            transform.Find("Step").gameObject.SetActive(false);
        }
    }

    void TimerValueSetUp()
    {
        timerChangeValue = timerChangeInputValue;
        if (timerChangeValue < 0)
        {
            transform.Find("Timer+").GetComponent<Renderer>().material.color = Color.red;

            transform.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystemRenderer>().material.color = Color.black;
        }
    }


    void SetUpDebugStepValue()
    {
        var text = step.ToString();
        transform.Find("Step").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = text;
    }

    void Error(string errorText)
    {
        print(errorText);
        Time.timeScale = 0;
    }

    void VisibleOrInvisibleTile()
    {
        if (walkable && !rend.enabled)
        {
            rend.enabled = true;
            transform.Find("Step").Find("Text (TMP)").gameObject.SetActive(true);
        }


        if (!walkable && rend.enabled && door == 0)
        {
            transform.Find("Step").Find("Text (TMP)").gameObject.SetActive(false);
            rend.enabled = false;
        }
    }
    void tempoChange()
    {
       
        if (redTile && loopCycle.redFlag && flager1)
        {
            var heights = transform.position;
            heights.y += 2;
            transform.position = heights;

            flager1 = false;
        }


        if (redTile && !loopCycle.redFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
           //stepAssignement.Initialisation();
            flager1 = true;
        }


        if (blueTile && loopCycle.blueFlag && flager1)
        {
            var heights = transform.position;
            heights.y += 2;
            transform.position = heights;

            flager1 = false;
        }


        if (blueTile && !loopCycle.blueFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
            //stepAssignement.Initialisation();
            flager1 = true;
        }


                if (greenTile && loopCycle.greenFlag && flager1)
        {
            var heights = transform.position;
            heights.y += 2;
            transform.position = heights;

            flager1 = false;
        }


        if (greenTile && !loopCycle.greenFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
           //stepAssignement.Initialisation();
            flager1 = true;
        }


    }

    #endregion

    #region HighlightMethods
    void HighlightingHub()
    {
        if (highLight)
            Highlight();

        if (!highLight)
            UnHighlight();
    }

    void Highlight()
    {
        transform.Find("highlight").gameObject.SetActive(true);
    }

    void UnHighlight()
    {
        transform.Find("highlight").gameObject.SetActive(false);
        
    }
    #endregion 

}
