using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridTiles : MonoBehaviour
{



    #region variables
    [SerializeField, HideInInspector]public bool pauseAnim = false;
    [SerializeField, HideInInspector]public float lerpSpeed = .1f;
    [SerializeField, HideInInspector]public float pauseLerpSpeed = .1f;
    [SerializeField, HideInInspector]public float currentPosY ;
    [SerializeField, HideInInspector]public float HeightDiffR, HeightDiffL,HeightDiffU, HeightDiffD;
    [Header("TempoTilesEffect")]
    [HideInInspector] public int step;
    [HideInInspector] public bool opening;

    [HideInInspector] public bool walkableC;
    [HideInInspector] public bool invisibleC;
    [HideInInspector] public bool ogPosC;
    [HideInInspector] public int KeyC;
    [HideInInspector] public int DoorC;
    [HideInInspector] public bool OpenC;
    [HideInInspector] public bool CrumbleC;
    [HideInInspector] public int LevelTransiIndexC;
    [HideInInspector] public int TempoTileC;
    [HideInInspector] public int TeleporterC;
    [HideInInspector] public int TeleporterTargetC;

    public bool walkable;
    public bool invisible;
    public bool originalPosition;
    [Range(0, 5)] public int key;
    [Range(0, 5)] public int door;    
    public bool open = false;

    
    public bool crumble;
    [HideInInspector] public bool crumbleUp;
    [HideInInspector] public bool crumbleBool;

    [Range(0, 8)] public float levelTransiIndex;
    //[Range(-100, 100)] public int timerChangeInputValue;
    [Range(0, 3)] public int tempoTile;

    [Header("Teleporter")]
    [Range(0, 20)] public int teleporter;
    [Range(0, 20)] public int tpTargetIndex;
    [HideInInspector] public GridTiles TpTarget;

    [HideInInspector] public int target;
    [HideInInspector] public int targetOpen;
    [HideInInspector] public int currentOpen = 0;
    [HideInInspector] public bool tempoBool;
    [Space]
    [Header("Modifier")]
    public float tempoTileSpeed;
    //[HideInInspector] public int timerChangeValue;
    [HideInInspector] public int height;
    [HideInInspector] public bool hitByCam = false;
    [HideInInspector] public int numberFrameHit = 0;
    int earlynumberFrameHit = 0;
    float fadeInSpeed =3;
    Renderer rend;
    GameObject gameManager;      
    GridGenerator gridGenerator;
    Transform tpTarget;
    GridTiles[,] grid;

    
    #endregion

    #region CallMethods
    private void Awake()
    {
        if (!originalPosition)
        {
            transform.position += new Vector3(0,-20,0);
        }
     

        //TimerValueSetUp();

        SetUpComponents();
    }



    private void Start()
    {
        currentOpen = 0;
        grid = FindObjectOfType<GridGenerator>().grid;
        tempoBool = true;
        if (walkable)
        {
            var col = rend.material.color;
            col.a = 1;
            rend.material.color = col;
        }
        if (!walkable)
        {
            var col = rend.material.color;
            col.a = 0;
            rend.material.color = col;
        }

/*        if(door != 0)
        {
            walkable = false;
        }*/
        
        if (teleporter != 0)
        {
            foreach (GridTiles obj in grid)
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
        if (walkable)
            invisible = false;

        if (hitByCam)
        {
            foreach (MeshRenderer m in transform.GetComponentsInChildren<MeshRenderer>())
            {
                var col = m.material.color;
                col.a = 0.5f;
                m.material.color = col;
            }
        }

        if (!walkable && door == 0)
        {
            var yo = transform.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer y in yo)
            {
                if (y.name != "Renderer" && y.name != "Door")
                    y.enabled = false;
            }
        }

        if (walkable)
        {
            var yo = transform.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer y in yo)
            {
                if (y.name != "Renderer" && y.name != "Door" )
                    y.enabled = true;
            }
        }


        if(door != 0 && open)
        {
            walkable = false;
           
        }

        if (door != 0 && !open) 
        {
            walkable = true;
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

    void VisibleOrInvisibleTile()
    {
        if (walkable)
        {
            if (tempoTile != 0)
            {
                transform.Find("DirectionTempoU").gameObject.SetActive(true);
                transform.Find("DirectionTempoD").gameObject.SetActive(true);

            }
            var col = rend.material.color;
            col.a = Mathf.Lerp(col.a,1,Time.deltaTime*fadeInSpeed);
            rend.material.color = col;
        }


        if (!walkable)
        {
            if(tempoTile != 0)
            {
                transform.Find("DirectionTempoU").gameObject.SetActive(false) ;
                transform.Find("DirectionTempoD").gameObject.SetActive(false) ;
                
            }
            var col = rend.material.color;
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime*fadeInSpeed);
            rend.material.color = col;
        }
    }
    #endregion
}
