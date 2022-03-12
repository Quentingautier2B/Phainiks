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
       
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;    
    LoopCycle loopCycle;


    bool flager1 = true;   

    #endregion

    #region CallMethods
    private void Awake()
    {
        if (tempoTile == 1)
        {
            transform.Find("DirectionTempoD").GetComponent<ParticleSystemRenderer>().material.color = Color.blue;
            transform.Find("DirectionTempoU").GetComponent<ParticleSystemRenderer>().material.color = Color.blue;
        }        
        
        if (tempoTile == 2)
        {
            transform.Find("DirectionTempoD").GetComponent<ParticleSystemRenderer>().material.color = Color.green;
            transform.Find("DirectionTempoU").GetComponent<ParticleSystemRenderer>().material.color = Color.green;
        }        
        
        if (tempoTile == 3)
        {
            transform.Find("DirectionTempoD").GetComponent<ParticleSystemRenderer>().material.color = Color.red;
            transform.Find("DirectionTempoU").GetComponent<ParticleSystemRenderer>().material.color = Color.red;
        }


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

        VisibleOrInvisibleTile();

        tempoChange();
    }
    #endregion

    #region Methods
    void SetUpComponents()
    {
        rend = GetComponent<Renderer>();
        height = (int)transform.position.y;
        gameManager = FindObjectOfType<GridGenerator>().gameObject;
        gridGenerator = gameManager.GetComponent<GridGenerator>();       
        playerMovement = gameManager.GetComponent<PlayerMovement>();
        loopCycle = gameManager.GetComponent<LoopCycle>();
    }

    void HeightToInt()
    {   
        if (height != (int)transform.position.y)
            height = (int)transform.position.y;
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
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play ();
            flager1 = false;
        }


        if (redTile && !loopCycle.redFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
            //stepAssignement.Initialisation();
            flager1 = true;
        }


        if (blueTile && loopCycle.blueFlag && flager1)
        {
            var heights = transform.position;
            heights.y += 2;
            transform.position = heights;
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
            flager1 = false;
        }


        if (blueTile && !loopCycle.blueFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
            //stepAssignement.Initialisation();
            flager1 = true;
        }


                if (greenTile && loopCycle.greenFlag && flager1)
        {
            var heights = transform.position;
            heights.y += 2;
            transform.position = heights;
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Play();
            flager1 = false;
        }


        if (greenTile && !loopCycle.greenFlag && !flager1)
        {
            var heights = transform.position;
            heights.y -= 2;
            transform.position = heights;
            transform.Find("DirectionTempoD").GetComponent<ParticleSystem>().Stop();
            transform.Find("DirectionTempoU").GetComponent<ParticleSystem>().Play();
            //stepAssignement.Initialisation();
            flager1 = true;
        }


    }

    #endregion
}
