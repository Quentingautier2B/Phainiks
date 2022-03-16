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
    public float tempoTileSpeed;
    float target;
    bool tempoTileFlag = true;

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



    bool flager1 = true;   

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
        rend = transform.Find("Renderer").GetComponent<Renderer>();
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
    #endregion
}
