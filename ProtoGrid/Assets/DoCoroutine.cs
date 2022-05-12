using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCoroutine : MonoBehaviour
{
    public AnimationCurve SpeedCurve;
    public AnimationCurve oGSpeedCurve;
    float yo;
    SceneChange sChange;
    public float begin = 5;
    public float lerper;
    InGameUI inGameUI;


    private void Awake()
    {
        sChange = FindObjectOfType<SceneChange>();
        inGameUI = FindObjectOfType<InGameUI>();
    }

    public void startCoroutine(GridTiles tile)
    {
        StartCoroutine(RandomLerpTimer(tile));
    }

    IEnumerator RandomLerpTimer(GridTiles tile)
    {

        yield return new WaitForSeconds(Random.Range(0f, .4f));
        tile.pauseAnim = true;
    }

    public void pauseTileMovement(GridTiles tile, GridTiling curTile)
    {

       
            curTile.SetDirectionalMaterial();

        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.transform.position.y, tile.target, tile.pauseLerpSpeed * Time.deltaTime), tile.transform.position.z);

        tile.pauseLerpSpeed = Mathf.Lerp(tile.pauseLerpSpeed, 3, Time.deltaTime * tile.pauseLerpSpeed);


        //Called on last loop
        if (tile.transform.position.y <= tile.target + 0.3f)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.target, tile.transform.position.z);

            tile.pauseAnim = false;
        }

    }

    public void startClose(GridTiles tile, float levelTransiIndex)
    {

            StartCoroutine(Queue1(tile,levelTransiIndex));
    }

    IEnumerator Queue1(GridTiles tile, float levelTransiIndex)
    {
        if (Time.timeSinceLevelLoad < 1)
        {
            float speed = 1;
            yield return new WaitForSeconds(Random.Range(0f, .4f));
           
            StartCoroutine(QueueForOpen(tile, speed, oGSpeedCurve, levelTransiIndex));

        }
        else if (tile.levelTransiIndex == 100)
        {
            inGameUI.endTile = levelTransiIndex;
            float speed = 1f;
            yield return new WaitForSeconds(Random.Range(0f, .4f));
            
            StartCoroutine(QueueForOpen(tile, speed, oGSpeedCurve, levelTransiIndex));
        }
        else if (tile.tempoTile != 0)
        {
            float speed = 2;
            yield return new WaitForSeconds(1);
            StartCoroutine(QueueForOpen(tile, speed, SpeedCurve, levelTransiIndex));
        }
        else
        {
            yield return new WaitForSeconds(0);
            StartCoroutine(QueueForOpen(tile, 1, SpeedCurve, levelTransiIndex));

        }

    }

    IEnumerator QueueForOpen(GridTiles tile, float speed, AnimationCurve curve, float levelTransiIndex)
    {
       
        yield return new WaitUntil(() => !tile.opening);
        if (Time.timeSinceLevelLoad < .5f)
        {
            tile.lerpSpeed = 0f;
            tile.targetOpen = (int)tile.transform.position.y + 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else if(tile.levelTransiIndex == 100)
        {
            tile.lerpSpeed = 0f;
            tile.targetOpen = (int)tile.transform.position.y - 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else if (!tile.open)
        {
            tile.lerpSpeed = 0f;
            //t.open = false;
            
            tile.targetOpen = (int)tile.transform.position.y - 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else
        {
            tile.lerpSpeed = 0f;
            tile.targetOpen = (int)tile.transform.position.y + 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }

        if(tile.door != 0 && Time.timeSinceLevelLoad < 1f && !tile.open)
        {
            tile.open = true;
   
        }
        else
        {

           StartCoroutine(CloseTileMovement(tile,speed, curve, levelTransiIndex));
        }
    }

    IEnumerator CloseTileMovement(GridTiles tile, float speed, AnimationCurve curve, float levelTransiIndex)
    {
        //tile.GetComponent<GridTiling>().SetDirectionalMaterial();
        tile.opening = true;
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.currentOpen, tile.targetOpen, tile.lerpSpeed), tile.transform.position.z);
        tile.lerpSpeed += Time.deltaTime * curve.Evaluate(tile.lerpSpeed);

        if (tile.transform.position.y <= tile.targetOpen + 0.01f && tile.levelTransiIndex == 100)
        {
   
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            sChange.startCoroutine(levelTransiIndex);
            tile.opening = false;
        }
        else if (tile.transform.position.y >= tile.targetOpen - 0.01f && tile.open && Time.timeSinceLevelLoad > begin && tile.levelTransiIndex != 100)
        {
   

            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.open = false;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else if(tile.transform.position.y <= tile.targetOpen + 0.01f && !tile.open && Time.timeSinceLevelLoad > begin && tile.levelTransiIndex != 100)
        {
  

            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.open = true;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else if (tile.transform.position.y >= tile.targetOpen - 0.01f && Time.timeSinceLevelLoad < begin && tile.levelTransiIndex != 100)
        {
            

            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CloseTileMovement(tile, speed, curve, levelTransiIndex));
        }

    }


    public void startLerper()
    {
        lerper = 0;
        StartCoroutine(Lerper(inGameUI.pauseStartPosX, inGameUI.pauseEndPosX));
    }

    public void revertLerper()
    {
        lerper = 0;
        StartCoroutine(Lerper(inGameUI.pauseEndPosX, inGameUI.pauseStartPosX));
    }

    IEnumerator Lerper(float startPos, float endPos)
    {
        
        lerper += Time.deltaTime * 1;
        var vec = inGameUI.PauseLevel.anchoredPosition;
        vec.x = Mathf.Lerp(startPos, endPos, lerper);
        inGameUI.PauseLevel.anchoredPosition = vec;

        if (lerper >= 1)
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper(startPos, endPos));
        }
    }
}
