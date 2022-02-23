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
    [Header("Work in progress don't use")]
    public int tempoTile;
    
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
    LoopCycle loopCycle;


    #endregion

    #region CallMethods
    private void Awake()
    {
        DebugHub();
        TimerValueSetUp();
        SetUpComponents();
    }

    void Update()
    {
        SetUpDebugStepValue();

        HeightToInt();

        HighlightingHub();

        VisibleOrInvisibleTile();

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
            FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Click");
        }
    }
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
        if(loopCycle.tempoIndex == tempoTile && tempoTile!=0)
        {
            walkable = true;
        }      
        

        if (loopCycle.tempoIndex != tempoTile && tempoTile != 0)
            walkable = false;
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
