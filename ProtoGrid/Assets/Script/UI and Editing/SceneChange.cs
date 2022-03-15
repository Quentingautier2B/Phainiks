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

    IEnumerator EndBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/LevelEnd");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Lvl_" + tile.levelTransiIndex, LoadSceneMode.Single);
    }
}
