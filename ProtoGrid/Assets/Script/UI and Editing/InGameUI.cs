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
    public bool tpOn;
    LineRenderer[] tps;
    #endregion

    

    public void OnResetClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHubClick()
    {
        SceneManager.LoadScene("Lvl_1");
    }

    public void OnTPClick()
    {
        
        foreach(LineRenderer tp in tps)
        {
            if (tpOn)
            {
                tp.gameObject.SetActive(false);
            }
            else if(!tpOn)
            {
                tp.gameObject.SetActive(true);
            }
        }
        if (tpOn)
            tpOn = false;
        else if(!tpOn)
            tpOn = true;
    }

    private void Awake()
    {
        tps = FindObjectsOfType<LineRenderer>();
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
