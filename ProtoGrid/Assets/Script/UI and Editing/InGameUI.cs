using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    #region variables
    public TextMeshProUGUI resetText;
    Reset resetScript;
    int resetValue;
    #endregion

    void Start()
    {
        resetScript = FindObjectOfType<Reset>();
        
    }
    
    void Update()
    {
        
        ResetTimerText();
    }

    void ResetTimerText()
    {
        resetValue = resetScript.resetTimer;
        resetText.text = resetValue.ToString();
    }
}
