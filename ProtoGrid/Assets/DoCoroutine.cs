using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCoroutine : MonoBehaviour
{
    public void startCoroutine(GridTiles tile)
    {
        StartCoroutine(RandomLerpTimer(tile));
    }

    IEnumerator RandomLerpTimer(GridTiles tile)
    {

        yield return new WaitForSeconds(Random.Range(0f, .4f));
        tile.pauseAnim = true;
    }
}
