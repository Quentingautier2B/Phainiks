using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TilingEditor : MonoBehaviour
{


    #region Variables
    GridTiles tile;
    bool flag = true;
    bool walkable;
    int door;
    public bool doorRotation;
    bool crumble;
    bool originalPosition;
    int key;
    int timerChangeInputValue;
    float levelTransiIndex;
    int tempoValue;
    int tpValue;
    Renderer rend;
    

    
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
        playOn = false;
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
    
    private void OnDrawGizmos()
    {
        if(playOn)
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
        playOn = true;
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
        
       // doorRotation = tile.doorRotation;
    }

    void EditorBlocRenderering()
    {
        if (!walkable && door == 0)
        {
            rend.GetComponent<Renderer>().material = disabledMat;
        }

        if (walkable && !crumble)
        {
            rend.GetComponent<Renderer>().material = normalMat;
        }


        if (crumble)
        {
            rend.GetComponent<Renderer>().material = crumbleMat;
        }
    }

    #region CreateDestroyMethods
    void CreateDestroyMethodsHub()
    {
        CreateDestroyObjectBoolean(originalPosition, "OriginalPos", originalPositionItem, 0.53f);
        CreateDestroyObjectIndex(timerChangeInputValue, "Timer+", TimerItem, 0.5f);
        CreateDestroyObjectIndex(key, "Key", KeyItem, 1f);
        //CreateDestroyObjectIndex(door, "Door", DoorItem, 1f);
        CreateDestroyObjectFloat(levelTransiIndex, "LevelTransi", LevelTransitionItem, 0.5f);
        CreateDestroyObjectIndex(tpValue, "Teleporter", TpItem, 0.52f);
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
            Selection.activeObject = PrefabUtility.InstantiatePrefab(itemType);
            var inst = Selection.activeObject as GameObject;
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
        DoorColoration();
        KeyColoration();
        TempoTileColoration();
    }

    void DoorColoration()
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
    }

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
