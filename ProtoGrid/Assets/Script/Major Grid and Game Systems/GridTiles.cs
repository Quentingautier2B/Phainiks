using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridTiles : MonoBehaviour
{
    


    #region variables
    [Header("TempoTilesEffect")]
    [HideInInspector] public int step;
    public bool walkable;
    public bool originalPosition;
    //[Range(0, 5)]public int key;
    //[Range(0, 5)] public int door;
    //public bool crumble;
    [Range(0, 8)] public float levelTransiIndex;
    //[Range(-100, 100)] public int timerChangeInputValue;
    [Range(0, 3)] public int tempoTile;

    [Header("Teleporter")]
    [Range(0, 20)] public int teleporter;
    [Range(0, 20)] public int tpTargetIndex;
    [HideInInspector] public GridTiles TpTarget;
    [HideInInspector] public int target;
    [HideInInspector] public bool tempoBool;
    [Space]
    [Header("Modifier")]
    public float tempoTileSpeed;
    //[HideInInspector] public int timerChangeValue;
    [HideInInspector] public int height;
    [HideInInspector] public bool hitByCam = false;
    [HideInInspector] public int numberFrameHit = 0;
    int earlynumberFrameHit = 0;

    Renderer rend;
    GameObject gameManager;      
    GridGenerator gridGenerator;



    
    #endregion

    #region CallMethods
    private void Awake()
    {
        

        //TimerValueSetUp();

        SetUpComponents();
    }

    private void Start()
    {
        tempoBool = true;
/*        if(door != 0)
        {
            walkable = false;
        }*/

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
        if (hitByCam)
        {
            var col = rend.material.color;
            col.a = 0.5f;
            rend.material.color = col;
            
        }

        HeightToInt();

        VisibleOrInvisibleTile();

        //tempoChange();
    }

    private void LateUpdate()
    {
        if(hitByCam && numberFrameHit > earlynumberFrameHit)
        {
            earlynumberFrameHit = numberFrameHit;
        }
        else if(numberFrameHit == earlynumberFrameHit && hitByCam)
        {
            hitByCam = false;
            numberFrameHit = 0;
            earlynumberFrameHit = 0;
            var col = rend.material.color;
            col.a = 1;
            rend.material.color = col;
        }
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
   
    /*void TimerValueSetUp()
    {
        timerChangeValue = timerChangeInputValue;
        if (timerChangeValue < 0)
        {
            transform.Find("Timer+").GetComponent<Renderer>().material.color = Color.red;

            transform.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystemRenderer>().material.color = Color.black;
        }
    }*/

    void VisibleOrInvisibleTile()
    {
        if (walkable && !rend.enabled)
        {
            rend.enabled = true;
        }


        if (!walkable && rend.enabled /*&& door == 0*/)
        {
            rend.enabled = false;
        }
    }
    #endregion
}
