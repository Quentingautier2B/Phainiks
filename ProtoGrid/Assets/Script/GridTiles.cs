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
    public bool key; 
    public bool door;
    public bool crumble;
    public int levelTransiIndex;
    public int timerChangeInputValue;
    [HideInInspector] public int timerChangeValue;
    public int height;
    [SerializeField] bool errorPause;

    [Space]
    [Header("Components")]
    Renderer rend;
    public GameObject originalPositionItem;
    GameObject gameManager;
    PathHighlighter pathHighlighter;
    GridGenerator gridGenerator;
    PlayerMovement playerMovement;
    public Material crumbleMat;
    public Material normalMat;
    public Material disabledMat;
    #endregion

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (crumble)
            rend.material = crumbleMat;


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

        if (key)
        {
            if (!transform.Find("Key"))
            {
                print("Key Bloc with no Key" + this.name);  
                Time.timeScale = 0;

                //ErrorPause ne fait rien
                errorPause = true;
            }
        }
        if(door)
        {
            if (!transform.Find("Door"))
            {
                print("Door Bloc with no Door" + this.name);
                Time.timeScale = 0;

                //ErrorPause ne fait rien
                errorPause = true;
            }
        }

    }

    void Start()
    {
        if (!walkable && !door)
        {
            rend.enabled = false;
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

        if (walkable && !rend.enabled)
        {
            rend.enabled = true;
        }


        if (!walkable && rend.enabled && !door)
        {
            rend.enabled = false;
        }
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

    private void OnDrawGizmos()
    {
        if (!walkable && !door)
        {
            GetComponent<Renderer>().material = disabledMat;
        }

        if(walkable && !crumble)
        {
            GetComponent<Renderer>().material = normalMat;
        }
        

        if (crumble)
        {
            GetComponent<Renderer>().material = crumbleMat;
        }

   

        CreateDestroyOgPosGizmo();
        

        Vector3 snapToGrid = new Vector3(
            Mathf.Floor(transform.position.x),
            Mathf.Floor(transform.position.y),
            Mathf.Floor(transform.position.z)
            );
        transform.position = snapToGrid;
    }

    void CreateDestroyOgPosGizmo()
    {
        if (originalPosition && !transform.Find("OriginalPos"))
        {
            var inst = Instantiate(originalPositionItem, new Vector3(transform.position.x, transform.position.y + 0.53f, transform.position.z), Quaternion.identity);
            inst.transform.parent = this.transform;
            inst.name = "OriginalPos";
        }
        if (!originalPosition && transform.Find("OriginalPos"))
        {
            var inst = transform.Find("OriginalPos").gameObject;
            DestroyImmediate(inst);
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
