using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    DebugTools debugTools;
    [SerializeField] bool Hub;
    InGameUI inGameUI;
    GameObject stateMachine;

    private void Awake()
    {
        stateMachine = GameObject.Find("StateMachine");
        debugTools = GetComponent<DebugTools>();
        inGameUI = FindObjectOfType<InGameUI>();
    }
    public void startCoroutine(GridTiles tile)
    {
        StartCoroutine(EndBehavior(tile));
    }

    public IEnumerator EndBehavior(GridTiles tile)
    {
        stateMachine.SetActive(false);
        inGameUI.UiEndDisable();
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        debugTools.mainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.redMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.blueMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        debugTools.greenMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        debugTools.mainMusic.release();
        debugTools.redMusic.release();
        debugTools.blueMusic.release();
        debugTools.greenMusic.release();
        yield return new WaitForSeconds(0.1f);
        if (Hub)
            LevelTransi(tile);
        else
        {

            inGameUI.endLevelMenu.SetActive(true);
            inGameUI.oneStarImage.gameObject.SetActive(true);
            inGameUI.twoStarImage.gameObject.SetActive(true);
            inGameUI.threeStarImage.gameObject.SetActive(true);
        }

            inGameUI.endTile = tile;
            
    }

    public void LevelTransi(GridTiles tile)
    {
        if (tile.levelTransiIndex == 1)
            SceneManager.LoadScene("Lvl_" + 1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene("Lvl_" + Mathf.Floor(tile.levelTransiIndex) + "," + (Mathf.RoundToInt((tile.levelTransiIndex - Mathf.Floor(tile.levelTransiIndex)) * 10)), LoadSceneMode.Single);
    }

}
