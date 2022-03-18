using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TextMeshProUGUI resetText;
    Reset resetScript;
    int resetValue;
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
