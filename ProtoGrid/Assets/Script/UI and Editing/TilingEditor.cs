using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;
#endif
public class TilingEditor : MonoBehaviour
{


    #region Variables
    GridTiles tile;
    bool flag = true;
    bool walkable;
    bool invisible;
    int door;
    public bool doorRotation;
    bool crumble;
    bool originalPosition;
    int key;
    //int timerChangeInputValue;
    float levelTransiIndex;
    int tempoValue;
    int tpValue;
    int tpIndex;
    int height;
    Renderer rend;
    Vector3 tpTarget;
    GridTiles[,] grid;
    
    [Header("Materials for CubeTypes")]
    [Space]
    [SerializeField] Material disabledMat;
    [SerializeField] Material invisibleMat;
    //[SerializeField] Material crumbleMat;
    [SerializeField] Mesh normalTile;
    [SerializeField] Mesh tempoTile;


    [Header("GameObjects to Instantiate on Tiles")]
    [Space]
    [SerializeField] GameObject DoorItem;
    [SerializeField] GameObject KeyItem;
    [SerializeField] GameObject originalPositionItem;
    [SerializeField] GameObject TimerItem;
    [SerializeField] GameObject LevelTransitionItem;
    [SerializeField] GameObject PSysTTU;
    [SerializeField] GameObject PSysTTD;
    [SerializeField] GameObject TpItem;
    [SerializeField] GameObject TeleLine;

    [SerializeField] Mesh arrowHead;

    [Header("TP Lines Properties")]
    [Space]
    GridTiles targetTile;
    Vector3 p1, p2, p3, p1p3, p3p2, p1p2;
    bool trailCreate = true;
    [HideInInspector] bool tpFeedback;
    float trailTimer;
    bool trailBool;
    Transform Trail;
    float interpolateAmount;

    [Header("Materials")]
    [Space]
    public Material redM;
    public Material blueM;
    public Material greenM;
    public Material tileRedM;
    public Material tileBlueM;
    public Material tileGreenM;
    [SerializeField] bool playOn = true;
    #endregion

    private void Awake()
    {
        playOn = true;
        rend = transform.Find("Renderer").GetComponent<Renderer>();
        if (Input.GetKeyDown(KeyCode.N) && !walkable)
            walkable = true;

        if (Input.GetKeyDown(KeyCode.B) && walkable)
            walkable = false;

        rend = transform.Find("Renderer").GetComponent<Renderer>();
        GetVariablesValue();
        EditorBlocRenderering();
        CreateDestroyMethodsHub();
        EditorBlocSnapping();
        ItemColoring();
    }

    private void Start()
    {
        grid = FindObjectOfType<GridGenerator>().grid;
        if (tpValue != 0)
        {
            foreach (GridTiles t in FindObjectOfType<GridGenerator>().grid)
            {
                if (t.teleporter == tpIndex)
                {
                    targetTile = t;
                    tpTarget = new Vector3(t.transform.position.x, t.transform.position.y + 0.6f, t.transform.position.z);
                    transform.Find("Teleporter").LookAt(tpTarget);
                    var rotatio =  transform.Find("Teleporter").rotation;
                    rotatio.eulerAngles = new Vector3(0, transform.Find("Teleporter").rotation.eulerAngles.y, 0);
                    transform.Find("Teleporter").rotation = rotatio;

                }
            }
        }
    }   

    private void Update()
    {

        trailSign();
    }
/*    private void OnMouseEnter()
    {
        trailBool = false;
        trailTimer = 2;
    }

    private void OnMouseOver()
    {
        trailSign();
    }

    private void OnMouseExit()
    {
        if(Trail != null)
        StartCoroutine(DestroyTrail(Trail.gameObject));
    }*/

    void trailSign()
    {
        grid = FindObjectOfType<GridGenerator>().grid;
        if (tpValue != 0)
        {
            foreach (GridTiles t in FindObjectOfType<GridGenerator>().grid)
            {
                if (t.teleporter == tpIndex)
                {
                    targetTile = t;
                    tpTarget = new Vector3(t.transform.position.x, t.transform.position.y + 0.6f, t.transform.position.z);
                }
            }
        }

        if (!trailBool)
        {
            trailTimer += Time.deltaTime;

            if (trailTimer >= 1.5f /*&& tpFeedback*/)
            {
                trailBool = true;
                trailTimer = 0;
            }
        }

        
        if (tpValue != 0 && trailBool && grid[(int)tpTarget.x,(int)tpTarget.z].open && grid[(int)transform.position.x, (int)transform.position.z].open)
        {
            if (interpolateAmount <= .05 && trailCreate)
            {
                trailCreate = false;
                var inst = Instantiate(TeleLine, transform.position, Quaternion.identity);
                inst.name = "Line";
                inst.transform.parent = transform;
            }
            Trail = transform.Find("Line");
            p1 = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            p2 = new Vector3(tpTarget.x,tpTarget.y +0.1f, tpTarget.z);
            p3 = (p1 + p2) / 2 + new Vector3(0, Mathf.Abs(p1.y-p2.y) + 1, 0);
            interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
            p1p3 = Vector3.Lerp(p1, p3, interpolateAmount);
            p3p2 = Vector3.Lerp(p3, p2, interpolateAmount);
            p1p2 = Vector3.Lerp(p1p3, p3p2, interpolateAmount);

            if (Trail != null)
                Trail.position = p1p2;

            if (interpolateAmount >= .95 && !trailCreate)
            {
                trailCreate = true;
                trailBool = false;
                StartCoroutine(DestroyTrail(Trail.gameObject));
            }
        }
    }


    IEnumerator DestroyTrail(GameObject g)
    {
        g.name = "Discarded";
        yield return new WaitForSeconds(1);
        Destroy(g);
    }

    private void OnDrawGizmos()
    {
        if (walkable)
        {
            invisible = false;
            tile.invisible = invisible;
        }

        if (!playOn)
        {
            if (Input.GetKeyDown(KeyCode.N) && !walkable)
                walkable = true;

            if (Input.GetKeyDown(KeyCode.B) && walkable)
                walkable = false;

            rend = transform.Find("Renderer").GetComponent<Renderer>();
            GetVariablesValue();
            EditorBlocRenderering();
            CreateDestroyMethodsHub();
            EditorBlocSnapping();
            ItemColoring();
        }

    }

    private void OnApplicationQuit()
    {
        playOn = false;
    }


    void GetVariablesValue()
    {   
        if (flag)
        {
            tile = GetComponent<GridTiles>();
            flag = false;
        }
        walkable = tile.walkable;
        invisible = tile.invisible;
        door = tile.door;
        key = tile.key;
        crumble = tile.crumble;
        originalPosition = tile.originalPosition;
        //timerChangeInputValue = tile.timerChangeInputValue;
        levelTransiIndex = tile.levelTransiIndex;   
        tempoValue = tile.tempoTile;
        tpValue = tile.teleporter;
        tpIndex = tile.tpTargetIndex;
        height = tile.height;
        // doorRotation = tile.doorRotation;
    }

    void EditorBlocRenderering()
    {
        if (!walkable && door == 0)
        {
            transform.Find("Renderer").GetComponent<MeshFilter>().mesh = tempoTile;
            transform.Find("Renderer").localScale = Vector3.one;
            transform.Find("Renderer").rotation = Quaternion.identity;
            if (invisible)
                rend.GetComponent<Renderer>().material = invisibleMat;
            
            else
                rend.GetComponent<Renderer>().material = disabledMat;
        }

        if (walkable /*&& !crumble*/)
        {

            transform.Find("Renderer").GetComponent<MeshFilter>().mesh = normalTile;
            transform.Find("Renderer").localScale = new Vector3(50,50,50);
            //transform.Find("Renderer").position = new Vector3(transform.Find("Renderer").position.x, -4.9f, transform.Find("Renderer").position.z);
            //rend.GetComponent<Renderer>().material = normalMat;
        }


        if (crumble)
        {
            //transform.Find("Renderer").rotation = Quaternion.identity;
            transform.Find("Renderer").GetComponent<MeshFilter>().mesh = normalTile;
            transform.Find("Renderer").localScale = Vector3.one * 50;
            //rend.GetComponent<Renderer>().material = crumbleMat;
        }

        if(tempoValue != 0)
        {
            //transform.Find("Renderer").rotation = Quaternion.identity;
            transform.Find("Renderer").GetComponent<MeshFilter>().mesh = normalTile;
            transform.Find("Renderer").localScale = Vector3.one * 50;
        }
    }

    #region CreateDestroyMethods
    void CreateDestroyMethodsHub()
    {
        CreateDestroyObjectBoolean(originalPosition, "OriginalPos", originalPositionItem, 0.53f);
        //CreateDestroyObjectIndex(timerChangeInputValue, "Timer+", TimerItem, 0.5f);
        CreateDestroyObjectIndex(key, "Key", KeyItem, 1f);
        //CreateDestroyObjectIndex(door, "Door", DoorItem, 1f);
        CreateDestroyObjectFloat(levelTransiIndex, "LevelTransi", LevelTransitionItem, 0.5f);
        CreateDestroyObjectIndex(tpValue, "Teleporter", TpItem, 0.52f);
        //CreateDestroyObjectIndex(tpValue, "Line", TeleLine, 0f);
    }

    void EditorBlocSnapping()
    {
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        Vector3 snapToGrid = new Vector3(
            Mathf.Floor(transform.position.x),
            Mathf.Floor(transform.position.y),
            Mathf.Floor(transform.position.z)
            );
        transform.position = snapToGrid;
    }

    void CreateDestroyObjectIndex(int localInt, string itemTypeName, GameObject itemType, float itemHeight)
    {
        if (localInt != 0 && !transform.Find(itemTypeName))
        {
            var inst = InstantiatePrefab(itemType, itemHeight);
            inst.transform.parent = this.transform;
            inst.name = itemTypeName;
        }
        if (localInt == 0 && transform.Find(itemTypeName))
        {
            var inst = transform.Find(itemTypeName).gameObject;
            DestroyImmediate(inst);
        }
    }

    void CreateDestroyObjectFloat(float localInt, string itemTypeName, GameObject itemType, float itemHeight)
    {
        if (localInt != 0 && !transform.Find(itemTypeName))
        {
            var inst = InstantiatePrefab(itemType, itemHeight);
            inst.transform.parent = this.transform;
            inst.name = itemTypeName;
        }
        if (localInt == 0 && transform.Find(itemTypeName))
        {
            var inst = transform.Find(itemTypeName).gameObject;
            DestroyImmediate(inst);
        }
    }

    void CreateDestroyObjectBoolean(bool localBool, string itemTypeName, GameObject itemType, float itemHeight)
    {
        if (localBool && !transform.Find(itemTypeName))
        {
            var inst = InstantiatePrefab(itemType, itemHeight);
            inst.transform.parent = this.transform;
            inst.name = itemTypeName;
        }
        if (!localBool && transform.Find(itemTypeName))
        {
            var inst = transform.Find(itemTypeName).gameObject;
            DestroyImmediate(inst);
        }
    }
    #endregion

    GameObject InstantiatePrefab(GameObject itemType, float itemHeight)
    {
        #if UNITY_EDITOR
            GameObject inst = PrefabUtility.InstantiatePrefab(itemType) as GameObject;
            inst.transform.position = new Vector3(transform.position.x, transform.position.y + itemHeight, transform.position.z);
        #endif

        #if !UNITY_EDITOR
            var inst = Instantiate(itemType, new Vector3(transform.position.x, transform.position.y + itemHeight, transform.position.z), Quaternion.identity);
        #endif

        return inst;
    } 

    #region ItemColoration
    void ItemColoring()
    {
        //DoorColoration();
        KeyColoration();
        TempoTileColoration();
    }

    /*void DoorColoration()
    {
        if (door == 1)
        {
            var mesh = Color.red;
            transform.Find("Door").GetComponent<MeshRenderer>().material = redM;
        }

        if (door == 2)
        {
            var mesh = Color.blue;
            transform.Find("Door").GetComponent<MeshRenderer>().material = blueM;
        }

        if (door == 3)
        {
            var mesh = Color.black;
            transform.Find("Door").GetComponent<MeshRenderer>().material = greenM;
        }

        if (doorRotation && door!=0)
        {
            transform.Find("Door").Rotate(0, 90, 0);
            doorRotation = false;           
        }
    }*/

    void KeyColoration()
    {
        if (key == 1)
        {
            var mesh = Color.red;
            transform.Find("Key").GetComponent<MeshRenderer>().material = redM;
        }

        if (key == 2)
        {
            var mesh = Color.blue;
            transform.Find("Key").GetComponent<MeshRenderer>().material = blueM;
        }

        if (key == 3)
        {
            var mesh = Color.black;
            transform.Find("Key").GetComponent<MeshRenderer>().material = greenM;
        }
    }

    void TempoTileColoration()
    {
/*        if (tempoValue == 1)
        {
            var mesh = Color.red;
            rend.GetComponent<MeshRenderer>().material = tileRedM;
        }

        if (tempoValue == 2)
        {
            var mesh = Color.blue;
            rend.GetComponent<MeshRenderer>().material = tileBlueM;
        }

        if (tempoValue== 3)
        {
            var mesh = Color.black;
            rend.GetComponent<MeshRenderer>().material = tileGreenM;
        }
*/
        CreateDestroyObjectIndex(tempoValue, "DirectionTempoU", PSysTTU, 0.505f);
        CreateDestroyObjectIndex(tempoValue, "DirectionTempoD", PSysTTD, -0.505f);

    }
    #endregion
}
