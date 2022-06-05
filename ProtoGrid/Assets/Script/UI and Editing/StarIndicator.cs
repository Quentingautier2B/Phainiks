using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StarIndicator : MonoBehaviour
{
    public RawImage  star2I, star3I;
    public TextMeshProUGUI  star2T, star3T;
    InGameUI inGameUi;
    public Texture text1, text2;

    private void Awake()
    {
        inGameUi = FindObjectOfType<InGameUI>();
    }

    private void Start()
    {
        star2T.text = "" + inGameUi.twoStar;
        star3T.text = "" + inGameUi.threeStar;
    }

    private void Update()
    {


        if (inGameUi.twoStar < inGameUi.timerValue)
        {
            star2I.texture = text1;
            star2I.color = Color.black;
        }
        else
        {
            star2I.texture = text2;
            star2I.color = Color.white;
        }

        if (inGameUi.threeStar < inGameUi.timerValue)
        {
            star3I.texture = text1;
            star3I.color = Color.black;
        }
        else
        {
            star3I.texture = text2;
            star3I.color = Color.white;
        }
    }
}
