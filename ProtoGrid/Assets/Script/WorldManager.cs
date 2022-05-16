using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
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

    void Start()
    {
        parent = transform.parent.gameObject;
        text = this.transform.Find("Canvas/Name").gameObject.GetComponent<TextMeshProUGUI>();
        gridTiles = parent.GetComponent<GridTiles>();
        text.text = "World " + gridTiles.World;
        worldLevelUi = transform.Find("CanvasCam/").gameObject;
        if (LevelsOfTheWorld.Length > 1)
        {
            for (int i = 1; i <= LevelsOfTheWorld.Length; i++)
            {
                GameObject world = worldLevelUi.transform.Find("" + i).gameObject;
                world.SetActive(true);
                world.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Level " + i;

                
            }
        }
    }

    
    void Update()
    {
        
    }

    public void OpenWorld(GameObject button)
    {
        SceneManager.LoadScene("Lvl_" + Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1]) + "," + (Mathf.RoundToInt((LevelsOfTheWorld[int.Parse(button.name)- 1] - Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)- 1])) * 10)), LoadSceneMode.Single);
    }
}
