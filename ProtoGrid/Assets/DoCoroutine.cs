using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCoroutine : MonoBehaviour
{
    public AnimationCurve SpeedCurve;
    public AnimationCurve oGSpeedCurve;
    int yo;
    public void startCoroutine(GridTiles tile)
    {
        StartCoroutine(RandomLerpTimer(tile));
    }

    IEnumerator RandomLerpTimer(GridTiles tile)
    {

        yield return new WaitForSeconds(Random.Range(0f, .4f));
        tile.pauseAnim = true;
    }

    public void pauseTileMovement(GridTiles tile)
    {
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tile.lerpSpeed * Time.deltaTime), tile.transform.position.z);

        tile.lerpSpeed = Mathf.Lerp(tile.lerpSpeed, 3, Time.deltaTime * tile.lerpSpeed);


        //Called on last loop
        if (tile.transform.position.y <= tile.target + 0.3f)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);

            tile.pauseAnim = false;
        }

    }

    public void startClose(GridTiles tile)
    {

            StartCoroutine(Queue1(tile));
    }

    IEnumerator Queue1(GridTiles tile)
    {
        if (tile.tempoTile != 0)
        {
            yo = 1;
            float speed = 2;
            yield return new WaitForSeconds(yo);
            StartCoroutine(QueueForOpen(tile, speed, SpeedCurve));
        }
        else if (tile.originalPosition)
        {
            yo = 0;
            float speed = 10f;
            yield return new WaitForSeconds(yo);
            StartCoroutine(QueueForOpen(tile, speed, oGSpeedCurve));
        }
        else if (tile.levelTransiIndex != 0)
        {
            print(1);
            yo = 0;
            float speed = 1f;
            yield return new WaitForSeconds(yo);
            StartCoroutine(QueueForOpen(tile, speed, SpeedCurve));
        }
        else
        {
            yo = 0;
            yield return new WaitForSeconds(yo);
            StartCoroutine(QueueForOpen(tile, 1, SpeedCurve));

        }

    }

    IEnumerator QueueForOpen(GridTiles tile, float speed, AnimationCurve curve)
    {
       
        yield return new WaitUntil(() => !tile.opening);
        if(tile.levelTransiIndex != 0)
        {
            tile.targetOpen = (int)tile.transform.position.y + 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else if (!tile.open)
        {
            //t.open = false;
            tile.targetOpen = (int)tile.transform.position.y - 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else
        {
            tile.targetOpen = (int)tile.transform.position.y + 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        StartCoroutine(CloseTileMovement(tile,speed, curve));
    }

    IEnumerator CloseTileMovement(GridTiles tile, float speed, AnimationCurve curve)
    {

        tile.opening = true;
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.currentOpen, tile.targetOpen, tile.lerpSpeed), tile.transform.position.z);
        tile.lerpSpeed += Time.deltaTime * curve.Evaluate(tile.lerpSpeed);


        if (tile.transform.position.y >= tile.targetOpen - 0.01f && tile.open)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.open = false;
            yo = 0;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else if(tile.transform.position.y <= tile.targetOpen + 0.01f && !tile.open)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            yo = 0;
            tile.open = true;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CloseTileMovement(tile, speed, curve));
        }

    }
}
