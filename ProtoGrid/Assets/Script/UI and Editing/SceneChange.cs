using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void startCoroutine(GridTiles tile)
    {
        StartCoroutine(EndBehavior(tile));
    }

   public IEnumerator EndBehavior(GridTiles tile)
    {
      
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        
        yield return new WaitForSeconds(0.1f);
        if(tile.levelTransiIndex == 1)
            SceneManager.LoadScene("Lvl_" + 1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene("Lvl_" + Mathf.Floor(tile.levelTransiIndex) + "," +(Mathf.RoundToInt((tile.levelTransiIndex-Mathf.Floor(tile.levelTransiIndex))*10)), LoadSceneMode.Single);
    }
}
