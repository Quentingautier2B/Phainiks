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
    public RectTransform[] starImage = new RectTransform[3];
    public float[] starSizeValue = new float[3];
    private int[] starValue = new int[3];
    int starIndex;
    public int oneStar,twoStar,threeStar;
    public GameObject inGameUI;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject rotateLeft;
    [SerializeField] GameObject rotateRight;
    float starLerper;
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
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Rouge", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Bleu", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Vert", 0);




        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnResetClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/ResetLevel");
        StartCoroutine(ResetLevelButtonEffect());
    }

    public void HubClick()
    {
        StartCoroutine(OnHubClick());
    }

    IEnumerator OnHubClick()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Rouge", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Bleu", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Vert", 0);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/PauseMenu");
        yield return new WaitForSeconds(0.3f);

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
        timerText = inGameUI.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        sceneChange = FindObjectOfType<SceneChange>();
        revert = inGameUI.transform.Find("RevertTime").GetComponent<Button>();
    }

    private void Start()
    {
        starValue[0] = oneStar;
        starValue[1] = twoStar;
        starValue[2] = threeStar;
    }
    void Update()
    {
        if (sceneChange.Hub)
        {
            rotateLeft.SetActive(false);
            rotateRight.SetActive(false);
            restart.SetActive(false);
            revert.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
        }
        else if (timerValue <= 0 || stateMachine.GetBool("Rewind"))
        {
            TimerText();
            nbStep.text = "" + timerValue;
            revert.interactable = false;
        }
        else
        {
            TimerText();
            nbStep.text = "" + timerValue;
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Rouge", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Bleu", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Tile Vert", 0);
        //sceneChange.LevelTransi(endTile);
    }
    public void UiEndDisable()
    {
        starValue[0] = twoStar;
        starValue[1] = threeStar;
        starValue[2] = threeStar;
        foreach (GameObject g in uiEndLevelDisable)
        {
            g.SetActive(false);
        }
            StartCoroutine(Stars(starImage[starIndex], starSizeValue[starIndex], starValue[starIndex]));    
    }

    IEnumerator Stars(RectTransform Star, float size, int starCap)
    {
        starLerper += Time.deltaTime * 2;
        Star.sizeDelta = Vector2.Lerp(Vector2.zero, Vector2.one * size, starLerper);
        
        if(starLerper >= 1 && starIndex < 2 && timerValue <= starCap)
        {
            Star.sizeDelta = Vector2.one * size;
            starLerper = 0;
            starIndex++;
            yield return new WaitForSeconds(.3f);
            StartCoroutine(Stars(starImage[starIndex], starSizeValue[starIndex], starValue[starIndex]));
        }
        else if(starLerper < 1)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Stars(Star, size, starCap));
        }
        else
        {

        }
    }
}
