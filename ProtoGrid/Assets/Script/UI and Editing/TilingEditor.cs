using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingEditor : MonoBehaviour
{
    [TextArea(minLines: 0, maxLines: 20)]
    [SerializeField] string Notes = "Comment Here.";

    #region Variables
    GridTiles tile;
    bool flag = true;
    bool walkable;
    int door;
    bool crumble;
    bool originalPosition;
    int key;
    int timerChangeInputValue;
    int levelTransiIndex;

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
    #endregion

    private void OnDrawGizmos()
    {
        GetVariablesValue();
        EditorBlocRenderering();
        CreateDestroyMethodsHub();
        EditorBlocSnapping();
        ItemColoring();
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
        crumble = tile.crumble;
        originalPosition = tile.originalPosition;
        timerChangeInputValue = tile.timerChangeInputValue;
        levelTransiIndex = tile.levelTransiIndex;
    }

    void EditorBlocRenderering()
    {
        if (!walkable && door == 0)
        {
            GetComponent<Renderer>().material = disabledMat;
        }

        if (walkable && !crumble)
        {
            GetComponent<Renderer>().material = normalMat;
        }


        if (crumble)
        {
            GetComponent<Renderer>().material = crumbleMat;
        }
    }

    #region CreateDestroyMethods
    void CreateDestroyMethodsHub()
    {
        CreateDestroyOgPosGizmo();
        CreateDestroyMoveChangeTileGizmo();
        CreateDestroyDoorTileGizmo();
        CreateDestroyKeyTileGizmo();
        CreateDestroyLevelTransitionTileGizmo();
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

    void CreateDestroyMoveChangeTileGizmo()
    {
        if (timerChangeInputValue != 0 && !transform.Find("Timer+"))
        {
            var inst = Instantiate(TimerItem, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            inst.transform.parent = this.transform;
            inst.name = "Timer+";
        }
        if (timerChangeInputValue == 0 && transform.Find("Timer+"))
        {
            var inst = transform.Find("Timer+").gameObject;
            DestroyImmediate(inst);
        }
    }

    void CreateDestroyKeyTileGizmo()
    {
        if (key != 0 && !transform.Find("Key"))
        {
            var inst = Instantiate(KeyItem, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            inst.transform.parent = this.transform;
            inst.name = "Key";
        }
        if (key == 0 && transform.Find("Key"))
        {
            var inst = transform.Find("Key").gameObject;
            DestroyImmediate(inst);
        }
    }

    void CreateDestroyDoorTileGizmo()
    {
        if (door != 0 && !transform.Find("Door"))
        {
            var inst = Instantiate(DoorItem, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            inst.transform.parent = this.transform;
            inst.name = "Door";
        }
        if (door == 0 && transform.Find("Door"))
        {
            var inst = transform.Find("Door").gameObject;
            DestroyImmediate(inst);
        }
    }

    void CreateDestroyLevelTransitionTileGizmo()
    {
        if (levelTransiIndex != 0 && !transform.Find("LevelTransi"))
        {
            var inst = Instantiate(LevelTransitionItem, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            inst.transform.parent = this.transform;
            inst.name = "LevelTransi";
        }
        if (levelTransiIndex == 0 && transform.Find("LevelTransi"))
        {
            var inst = transform.Find("LevelTransi").gameObject;
            DestroyImmediate(inst);
        }
    }
    #endregion

    #region ItemColoration
    void ItemColoring()
    {
        DoorColoration();
        KeyColoration();
    }

    void DoorColoration()
    {
        if (door == 1)
        {
            var mesh = Color.red;
            transform.Find("Door").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (door == 2)
        {
            var mesh = Color.blue;
            transform.Find("Door").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (door == 3)
        {
            var mesh = Color.black;
            transform.Find("Door").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }
    }

    void KeyColoration()
    {
        if (key == 1)
        {
            var mesh = Color.red;
            transform.Find("Key").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (key == 2)
        {
            var mesh = Color.blue;
            transform.Find("Key").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (key == 3)
        {
            var mesh = Color.black;
            transform.Find("Key").GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }
    }
    #endregion
}
