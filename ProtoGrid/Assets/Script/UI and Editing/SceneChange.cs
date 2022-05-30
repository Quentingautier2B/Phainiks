using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    DebugTools debugTools;
    [SerializeField]public bool Hub;
    InGameUI inGameUI;
    GameObject stateMachine;
    float lerper;
    public AnimationCurve endAnimation;
    [SerializeField] int World;
    [HideInInspector] public float endLerper;
    [HideInInspector] public bool loadScene = false;
    private void Awake()
    {
        stateMachine = GameObject.Find("StateMachine");
        debugTools = GetComponent<DebugTools>();
        inGameUI = FindObjectOfType<InGameUI>();
    }


    public void startCoroutine(float tile)
    {
        StartCoroutine(EndBehavior(tile));
    }

    public IEnumerator EndBehavior(float tile)
    {
        //stateMachine.SetActive(false);
        inGameUI.UiEndDisable();

        yield return new WaitForSeconds(0.1f);
        if (tile < 1 || SceneManager.GetActiveScene().name == "Lvl_0,5" || Hub)
        {
            LevelTransi(tile);
        }
        else
        {
            inGameUI.endTile = tile;
        }

            inGameUI.endTile = tile;
            
    }

    public IEnumerator Lerper(float startLerp, float endLerp)
    {

        endLerper += Time.deltaTime * 1;
        var vec = inGameUI.Endlevel.anchoredPosition;
        vec.x = Mathf.Lerp(startLerp, endLerp, endAnimation.Evaluate(endLerper)) ;
        inGameUI.Endlevel.anchoredPosition = vec;

        if(endLerper >= 1)
        {

               
            yield return new WaitForEndOfFrame();

        }
        else
        {
            
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper(startLerp, endLerp));
        }
    }

    public IEnumerator lastLerp()
    {
        endLerper = 0;
        StartCoroutine(Lerper(inGameUI.endPosX, inGameUI.startPosX));
        yield return new WaitForSeconds(1.2f);
        LevelTransi(inGameUI.endTile);
    }

    public void LevelTransi(float tile)
    {
        if (tile == 1)
            SceneManager.LoadScene("Lvl_" + 1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene("Lvl_" + Mathf.Floor(tile) + "," + (Mathf.RoundToInt((tile - Mathf.Floor(tile)) * 10)), LoadSceneMode.Single);
    }

}
