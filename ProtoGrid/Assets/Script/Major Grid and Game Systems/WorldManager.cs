using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    GameObject parent;
    [SerializeField] TextMeshProUGUI text;
    GridTiles gridTiles;
    [SerializeField] float[] LevelsOfTheWorld;
    [SerializeField] Material worldMat;
    GameObject worldLevelUi;
    List<Scene> previewScene;
    GridTiles[,] grid;
    DoCoroutine doC;
    private Vector3 startpos;
    private Quaternion startRot;
    private float lerper;
    [SerializeField] GameObject plane;

    void Start()
    {
        grid = FindObjectOfType<GridGenerator>().grid;
        doC = FindObjectOfType<DoCoroutine>();
        parent = transform.parent.gameObject;
        text = this.transform.Find("Canvas/Name").gameObject.GetComponent<TextMeshProUGUI>();
        gridTiles = parent.GetComponent<GridTiles>();
        startpos = Camera.main.transform.localPosition;
        startRot = Camera.main.transform.localRotation;
        //text.text = "World " + gridTiles.World;
        worldLevelUi = transform.Find("CanvasCam/").gameObject;
        if (LevelsOfTheWorld.Length > 1)
        {
            for (int i = 1; i <= LevelsOfTheWorld.Length; i++)
            {
                GameObject world = worldLevelUi.transform.Find("" + i).gameObject;
                world.SetActive(true);
                world.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "" + i;               
            }
        }
        
    }

    public void OpenWorld(GameObject button)
    {
        foreach (GridTiles tile in grid)
        {
            if (tile.gameObject != grid[(int)gridTiles.transform.position.x, (int)gridTiles.transform.position.z].gameObject)
            {
                tile.levelTransiIndex = 100;

                doC.startClose(tile, tile.tiling, LevelsOfTheWorld[int.Parse(button.name) - 1], grid[(int)gridTiles.transform.position.x, (int)gridTiles.transform.position.z].GetComponent<GridTiling>());
            }
        }
        //SceneManager.LoadScene("Lvl_" + Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1]) + "," + (Mathf.RoundToInt((LevelsOfTheWorld[int.Parse(button.name)- 1] - Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1])) * 10)), LoadSceneMode.Single);
    }

    public void lerp()
    {
        StartCoroutine(Lerper());
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        foreach (Transform child in Camera.main.transform)
            child.gameObject.GetComponent<MeshRenderer>().material = worldMat;

    }

    IEnumerator Lerper()
    {
        lerper += Time.deltaTime;
        Camera.main.transform.localPosition = Vector3.Lerp(startpos, new Vector3(0, 0, -15), lerper);
        Camera.main.transform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, lerper);
        if (lerper >= 1)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, -15);
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper());
        }
    }
}
