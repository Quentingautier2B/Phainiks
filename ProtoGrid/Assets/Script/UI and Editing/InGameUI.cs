using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    #region variables
    public TextMeshProUGUI resetText;
    PlayerMovement pMov;
    #endregion

    private void Awake()
    {
        pMov = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {       
       TimerText();
    }

    void TimerText()
    {
        resetText.text = pMov.timerValue.ToString();
    }
}
