using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        parent = transform.parent.gameObject;
        text = this.transform.Find("Canvas/Name").gameObject.GetComponent<TextMeshProUGUI>();
        gridTiles = parent.GetComponent<GridTiles>();
        text.text = "World " + gridTiles.World;
        worldLevelUi = transform.Find("CanvasCam/").gameObject;
        if (LevelsOfTheWorld.Length > 1)
        {
            for (int i = 1; i < LevelsOfTheWorld.Length; i++)
            {
                worldLevelUi.transform.Find(""+ i).gameObject.SetActive(true);
            }
        }
    }

    
    void Update()
    {
        
    }

    public void OpenWorld(GameObject button)
    {
        SceneManager.LoadScene("Lvl_" + Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)]) + "," + (Mathf.RoundToInt((LevelsOfTheWorld[int.Parse(button.name)] - Mathf.Floor(LevelsOfTheWorld[int.Parse(button.name)])) * 10)), LoadSceneMode.Single);
    }
}
