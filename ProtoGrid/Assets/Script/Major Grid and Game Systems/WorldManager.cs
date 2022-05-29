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
    GameObject worldLevelUi;
    List<Scene> previewScene;
    GridTiles[,] grid;
    DoCoroutine doC;

    void Start()
    {
        grid = FindObjectOfType<GridGenerator>().grid;
        doC = FindObjectOfType<DoCoroutine>();
        parent = transform.parent.gameObject;
        text = this.transform.Find("Canvas/Name").gameObject.GetComponent<TextMeshProUGUI>();
        gridTiles = parent.GetComponent<GridTiles>();
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
            if (tile != grid[(int)gridTiles.transform.position.x, (int)gridTiles.transform.position.y])
            {
                tile.levelTransiIndex = 100;

                doC.startClose(tile, tile.tiling, LevelsOfTheWorld[int.Parse(button.name) - 1], grid[(int)gridTiles.transform.position.x, (int)gridTiles.transform.position.y].GetComponent<GridTiling>());
            }
        }
        //SceneManager.LoadScene("Lvl_" + Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1]) + "," + (Mathf.RoundToInt((LevelsOfTheWorld[int.Parse(button.name)- 1] - Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1])) * 10)), LoadSceneMode.Single);
    }
}
