using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class InGameUI : MonoBehaviour
{
    #region variables
    TextMeshProUGUI timerText;
    [HideInInspector]public int timerValue;
    #endregion

    public void OnResetClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        timerText = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {       
       TimerText();
    }

    void TimerText()
    {
       timerText.text = timerValue.ToString();
    }
}
