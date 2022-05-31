using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelIndexScript : MonoBehaviour
{
    DebugTools debT;
    int levelIndexEntier, levelIndexDecimal;
    TextMeshProUGUI text;
    void Start()
    {
        debT = FindObjectOfType<DebugTools>();
        text = GetComponent<TextMeshProUGUI>();

        levelIndexEntier = Mathf.FloorToInt(debT.levelIndex);
        levelIndexDecimal = Mathf.RoundToInt((debT.levelIndex - Mathf.FloorToInt(debT.levelIndex))*10);

        text.text = levelIndexEntier + " - " + levelIndexDecimal;
    }
}
