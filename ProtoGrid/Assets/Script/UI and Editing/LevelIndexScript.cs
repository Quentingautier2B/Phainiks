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


        text.text = debT.World + " - " + debT.levelIndex;
    }
}
