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
    [HideInInspector] public GameObject endLevelMenu;
    [HideInInspector] public float endTile;
    SceneChange sceneChange;
    [SerializeField] List<GameObject> uiEndLevelDisable;
    Button revert;
    Animator stateMachine;
    public TextMeshProUGUI nbStep;
    public RectTransform Endlevel;
    public RectTransform PauseLevel;
    public float endPosX, startPosX, pauseEndPosX, pauseStartPosX;
    public Image oneStarImage, twoStarImage, threeStarImage;
    public int oneStar,twoStar,threeStar;

    
    #endregion
     
    public void OnPauseClick()
    {
        FindObjectOfType<Animator>().SetBool("Paused", true);
    }

    public void OnUnPauseClick()
    {
        FindObjectOfType<Animator>().SetTrigger("ExitPause");
    }

    IEnumerator ResetLevelButtonEffect()
    {
        yield return new WaitForSeconds(.5f);
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

    public void OnResetClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/ResetLevel");
        StartCoroutine(ResetLevelButtonEffect());
    }

    public void OnHubClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/PauseMenu");
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
        stateMachine = FindObjectOfType<Animator>();
        debugTools = FindObjectOfType<DebugTools>();
        //endLevelMenu = transform.Find("EndlevelMenu").gameObject;
        timerText = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        sceneChange = FindObjectOfType<SceneChange>();
        revert = transform.Find("RevertTime").GetComponent<Button>();
    }

    private void Start()
    {
        
        /*oneStarImage.gameObject.SetActive(false);
        twoStarImage.gameObject.SetActive(false);
        twoStarImage.color = Color.white;
        threeStarImage.gameObject.SetActive(false);
        threeStarImage.color = Color.white;*/

        //endLevelMenu.SetActive(false);
        tps = FindObjectsOfType<LineRenderer>();
        //nbStep = transform.Find("EndlevelMenu/NbStep").gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
       TimerText();
        nbStep.text = "" + timerValue;
        if(timerValue <= 0 || stateMachine.GetBool("Rewind"))
        {
            revert.interactable = false;
        }
        else
        {
            revert.interactable = true;
        }
        
    }
    

    void TimerText()
    {
       timerText.text = timerValue.ToString();
    }

    public void LevelTransi()
    {
       StartCoroutine( sceneChange.lastLerp()); 
       //sceneChange.LevelTransi(endTile);
    }
    public void UiEndDisable()
    {
        foreach (GameObject g in uiEndLevelDisable)
        {
            g.SetActive(false);
            Stars();    
        }
    }

    void Stars()
    {
        if(timerValue > twoStar)
        {
            twoStarImage.color = Color.black;
        }

        if (timerValue > threeStar)
        {
            threeStarImage.color = Color.black;
        }
    }
}
