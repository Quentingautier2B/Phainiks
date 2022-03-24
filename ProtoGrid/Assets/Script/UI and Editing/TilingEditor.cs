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
    int door;
    public bool doorRotation;
    //bool crumble;
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
    [SerializeField] Material normalMat;
    [SerializeField] Material crumbleMat;

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
    public float LineLength;
    public float LineCurving;
    public float arrowHeadGirth;
    public float lineThickness;
    [Range(0,4)] public int colorIndex;
    public Color[] LineColorList = new Color[5];
    Color lineColor;
    int lineHeight;

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
            lineColor = LineColorList[colorIndex];



            foreach (GridTiles t in FindObjectOfType<GridGenerator>().grid)
            {
                if (t.teleporter == tpIndex)
                {

                    tpTarget = new Vector3(t.transform.position.x, t.transform.position.y + 0.6f, t.transform.position.z);
                    if (height > t.height)
                        lineHeight = height;
                    else
                    {
                        lineHeight = t.height;
                    }

                }
            }

            if (tpTarget != null)
            {
                var LineRendered = transform.Find("Line").GetComponent<LineRenderer>();
                var p1 = tpTarget;
                var p2 = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
                var diffP = new Vector3(p1.x - p2.x, 0, p1.z - p2.z);
                var diffPo = new Vector3(p2.x - p1.x, 0, p2.z - p1.z);
                var distanceP = Vector3.Distance(new Vector3(p2.x, 0, p2.z), new Vector3(p1.x, 0, p1.z));
                LineRendered.SetPosition(0, new Vector3(0, 0.5f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
                LineRendered.SetPosition(1, new Vector3(0, lineHeight + 1f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
                LineRendered.SetPosition(2, diffP + new Vector3(0, lineHeight + 1f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
                LineRendered.SetPosition(3, p1 - p2 + new Vector3(0, 0.5f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
                LineRendered.endWidth = 10;
                //LineRendered.startColor = lineColor;
                //LineRendered.endColor = lineColor;
                LineRendered.startWidth = lineThickness;
                
                //Handles.DrawBezier(p2 - (p2 - p1) * LineLength, p1 + (p2 - p1) * LineLength, new Vector3(p2.x, p2.y + LineCurving, p2.z), new Vector3(p1.x, p1.y + LineCurving, p1.z), lineColor, null, lineThickness);
               // Gizmos.DrawCube(p1 + (diffPo) * (LineLength * (distanceP * 100)), Vector3.one * arrowHeadGirth);
                //Gizmos.DrawMesh(arrowHead,0, p1 + (p2 - p1) *LineLength, Quaternion.LookRotation(Vector3.forward, p2-p1), new Vector3(arrowHeadGirth, arrowHeadGirth, arrowHeadGirth));
                //Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y +0.53f,transform.position.z), tpTarget);
            }

        }
    }

    private void Update()
    {
        if (tpTarget != null && tpValue != 0)
        {
            var LineRendered = transform.Find("Line").GetComponent<LineRenderer>();
            var p1 = tpTarget;
            var p2 = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            var diffP = new Vector3(p1.x - p2.x, 0, p1.z - p2.z);
            var diffPo = new Vector3(p2.x - p1.x, 0, p2.z - p1.z);
            var distanceP = Vector3.Distance(new Vector3(p2.x, 0, p2.z), new Vector3(p1.x, 0, p1.z));
            LineRendered.SetPosition(0, new Vector3(0, 0.5f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
            LineRendered.SetPosition(1, new Vector3(0, lineHeight + 1f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
            LineRendered.SetPosition(2, diffP + new Vector3(0, lineHeight + 1f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
            LineRendered.SetPosition(3, p1 - p2 + new Vector3(0, 0.5f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
            LineRendered.startColor = lineColor;
            //LineRendered.endColor = lineColor;
            //LineRendered.startWidth = lineThickness;
            LineRendered.endWidth = lineThickness;
        }
    }
    private void OnDrawGizmos()
    {








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

            if (tpValue != 0)
            {
                lineColor = LineColorList[colorIndex];



                foreach (GridTiles t in FindObjectOfType<GridGenerator>().grid)
                {
                    if (t.teleporter == tpIndex)
                    {

                        tpTarget = new Vector3(t.transform.position.x, t.transform.position.y + 0.6f, t.transform.position.z);
                        if (height > t.height)
                            lineHeight = height;
                        else
                        {
                            lineHeight = t.height;
                        }

                    }
                }

                if (tpTarget != null)
                {
                    var LineRendered = transform.Find("Line").GetComponent<LineRenderer>();
                    var p1 = tpTarget;
                    var p2 = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
                    var diffP = new Vector3(p1.x - p2.x, 0, p1.z - p2.z);
                    var diffPo = new Vector3(p2.x - p1.x, 0, p2.z - p1.z);
                    var distanceP = Vector3.Distance(new Vector3(p2.x, 0, p2.z), new Vector3(p1.x, 0, p1.z));
                    LineRendered.SetPosition(0, new Vector3(0, 0.5f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
                    LineRendered.SetPosition(1, new Vector3(0, lineHeight + 1f, 0) - (diffPo) * (LineLength * (distanceP * 100)));
                    LineRendered.SetPosition(2, diffP + new Vector3(0, lineHeight + 1f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
                    LineRendered.SetPosition(3, p1 - p2 + new Vector3(0, 0.5f, 0) + (diffPo) * (LineLength * (distanceP * 100)));
                    //LineRendered.startColor = lineColor;
                    //LineRendered.endColor = Color.red;
                    LineRendered.startWidth = lineThickness;
                    LineRendered.endWidth = lineThickness;
                    //Matrix4x4 matrix = Matrix4x4.TRS(p1, Quaternion.identity, Vector3.one * 0.1f);
                    
                    //Graphics.DrawMeshNow(arrowHead, matrix);
                }
            }
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
        door = tile.door;
        key = tile.key;
        //crumble = tile.crumble;
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
            rend.GetComponent<Renderer>().material = disabledMat;
        }

        if (walkable /*&& !crumble*/)
        {
            rend.GetComponent<Renderer>().material = normalMat;
        }


   /*     if (crumble)
        {
            rend.GetComponent<Renderer>().material = crumbleMat;
        }*/
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
        CreateDestroyObjectIndex(tpValue, "Line", TeleLine, 0f);
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
        if (tempoValue == 1)
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

        CreateDestroyObjectIndex(tempoValue, "DirectionTempoU", PSysTTU, 0.505f);
        CreateDestroyObjectIndex(tempoValue, "DirectionTempoD", PSysTTD, -0.505f);

    }
    #endregion
}
