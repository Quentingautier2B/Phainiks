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
    public bool tpOn = true;
    LineRenderer[] tps;
    DebugTools debugTools;
    #endregion

    

    public void OnResetClick()
    {
        debugTools.mainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.redMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.blueMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.greenMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        debugTools.mainMusic.release();
        debugTools.redMusic.release();
        debugTools.blueMusic.release();
        debugTools.greenMusic.release();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHubClick()
    {
        debugTools.mainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.redMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.blueMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.greenMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        debugTools.mainMusic.release();
        debugTools.redMusic.release();
        debugTools.blueMusic.release();
        debugTools.greenMusic.release();
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
        debugTools = FindObjectOfType<DebugTools>();
        
        timerText = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        tps = FindObjectsOfType<LineRenderer>();
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
