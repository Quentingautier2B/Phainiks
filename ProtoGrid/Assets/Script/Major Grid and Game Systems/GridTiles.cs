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
    public int tempoTile;
    public float tempoTileSpeed;

    [Header("Work in progress don't use")]
    [Range(0, 5)] public int teleporter;
    public int tpTargetIndex;
    [HideInInspector] public GridTiles TpTarget;



    [HideInInspector] public int timerChangeValue;
    [HideInInspector] public int height;

    [Space]
    [Header("Components")]
    Renderer rend;
    GameObject gameManager;
       
    GridGenerator gridGenerator;



    
    #endregion

    #region CallMethods
    private void Awake()
    {
        

        TimerValueSetUp();

        SetUpComponents();
    }

    private void Start()
    {
        if(door != 0)
        {
            walkable = false;
        }

        if (teleporter != 0)
        {
            foreach (GridTiles obj in gridGenerator.grid)
            {
                if (obj.teleporter == tpTargetIndex)
                {
                    if (TpTarget != null)
                        Debug.LogError("2 same targetTP");
                    if (obj == this)
                    {
                        Debug.LogError("Tp Can't have himself as target");
                        return;
                    }
                        
                    TpTarget = obj;
                }
            }
        }

    }

    void Update()
    {


        HeightToInt();

        VisibleOrInvisibleTile();

        //tempoChange();
    }
    #endregion

    #region Methods
    void SetUpComponents()
    {
        rend = GetComponent<Renderer>();
        height = (int)transform.position.y;
        gameManager = FindObjectOfType<GridGenerator>().gameObject;
        gridGenerator = gameManager.GetComponent<GridGenerator>();             
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

    void VisibleOrInvisibleTile()
    {
        if (walkable && !rend.enabled)
        {
            rend.enabled = true;
        }


        if (!walkable && rend.enabled && door == 0)
        {
            rend.enabled = false;
        }
    }

    void RecupTargetTeleporter()
    {

    }
    #endregion
}
