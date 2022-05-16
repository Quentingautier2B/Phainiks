using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCoroutine : MonoBehaviour
{
    public AnimationCurve SpeedCurve;
    public AnimationCurve oGSpeedCurve;
    float yo;
    SceneChange sChange;
    public float begin = 2.2f;
    public float lerper;
    InGameUI inGameUI;
    GridTiles[,] grid;
    GridGenerator gridG;

    public void UpdateAdjTiles(GridTiles g, int x, int y)
    {
        grid = gridG.grid;
        StartCoroutine(UpdateAdjacentTiles(g, x, y));
    }
    IEnumerator UpdateAdjacentTiles(GridTiles g, int x, int y)
    {
        yield return new WaitForEndOfFrame();
        if (x + 1 < gridG.raws && grid[x + 1, y] != null && grid[x + 1, y].walkable && grid[x + 1, y].tempoTile == 0)
        {
            grid[x + 1, y].GetComponent<GridTiling>().SetDirectionalMaterial();
        }

        if (y - 1 > - 1 && grid[x, y - 1] != null && grid[x, y - 1].walkable && grid[x, y - 1].tempoTile == 0)
        {
            grid[x, y - 1].GetComponent<GridTiling>().SetDirectionalMaterial();
        }

        if (y + 1 < gridG.columns && grid[x, y + 1] != null && grid[x, y + 1].walkable && grid[x, y + 1].tempoTile == 0)
        {
            grid[x, y + 1].GetComponent<GridTiling>().SetDirectionalMaterial();
        }

        if (x - 1 > -1 && grid[x - 1, y] != null && grid[x - 1, y].walkable && grid[x - 1, y].tempoTile == 0)
        {
            grid[x - 1, y].GetComponent<GridTiling>().SetDirectionalMaterial();
        }
    }


    private void Awake()
    {
        gridG = FindObjectOfType<GridGenerator>();
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

    public void startClose(GridTiles tile, float levelTransiIndex, GridTiling otherTile)
    {
        StartCoroutine(Queue1(tile,levelTransiIndex, otherTile));
    }

    IEnumerator Queue1(GridTiles tile, float levelTransiIndex, GridTiling otherTile)
    {
        if (Time.timeSinceLevelLoad < 1)
        {
            float speed = 2;
            yield return new WaitForSeconds(Random.Range(0f, .4f));
           
            StartCoroutine(QueueForOpen(tile, speed, oGSpeedCurve, levelTransiIndex, otherTile));

        }
        else if (tile.levelTransiIndex == 100)
        {
            inGameUI.endTile = levelTransiIndex;
            float speed = 1.5f;
            yield return new WaitForSeconds(Random.Range(0f, .4f));
            
            StartCoroutine(QueueForOpen(tile, speed, oGSpeedCurve, levelTransiIndex, otherTile));
        }
        else if (tile.tempoTile != 0)
        {
            float speed = 2;
            yield return new WaitForSeconds(1);
            StartCoroutine(QueueForOpen(tile, speed, SpeedCurve, levelTransiIndex, otherTile));
        }
        else
        {
            yield return new WaitForSeconds(0);
            StartCoroutine(QueueForOpen(tile, 1, SpeedCurve, levelTransiIndex, otherTile));

        }

    }

    IEnumerator QueueForOpen(GridTiles tile, float speed, AnimationCurve curve, float levelTransiIndex, GridTiling otherTile)
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
            
            tile.targetOpen = (int)tile.transform.position.y + 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }
        else
        {
            tile.lerpSpeed = 0f;
            tile.targetOpen = (int)tile.transform.position.y - 20;
            tile.currentOpen = (int)tile.transform.position.y;
        }

        if(tile.door != 0 && Time.timeSinceLevelLoad < 1f && !tile.open)
        {
            tile.open = true;   
        }
        else
        {
           StartCoroutine(CloseTileMovement(tile,speed, curve, levelTransiIndex, otherTile));
        }
    }

    IEnumerator CloseTileMovement(GridTiles tile, float speed, AnimationCurve curve, float levelTransiIndex, GridTiling otherTile)
    {
        if(otherTile != null)
            otherTile.SetDirectionalMaterial();




        //tile.GetComponent<GridTiling>().SetDirectionalMaterial();
        tile.opening = true;
        tile.transform.position = new Vector3(tile.transform.position.x, Mathf.Lerp(tile.currentOpen, tile.targetOpen, tile.lerpSpeed), tile.transform.position.z);
        tile.lerpSpeed += Time.deltaTime * curve.Evaluate(tile.lerpSpeed) * speed;

        if (tile.transform.position.y <= tile.targetOpen + 0.01f && tile.levelTransiIndex == 100)
        {
            //TransiLevel
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            sChange.startCoroutine(levelTransiIndex);
            tile.opening = false;
        }
        else if (tile.transform.position.y <= tile.targetOpen + 0.01f && tile.open && Time.timeSinceLevelLoad > begin && tile.levelTransiIndex != 100)
        {
            //open
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.GetComponent<GridTiling>().SetDirectionalMaterial();
            UpdateAdjTiles( tile, (int)tile.transform.position.x, (int)tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.open = false;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else if(tile.transform.position.y >= tile.targetOpen - 0.01f && !tile.open && Time.timeSinceLevelLoad > begin && tile.levelTransiIndex != 100)
        {
            //!open
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.GetComponent<GridTiling>().SetDirectionalMaterial();
            UpdateAdjTiles(tile, (int)tile.transform.position.x, (int)tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.open = true;
            tile.opening = false;
            yield return new WaitForEndOfFrame();
        }
        else if (tile.transform.position.y >= tile.targetOpen - 0.01f && Time.timeSinceLevelLoad < begin && tile.levelTransiIndex != 100)
        {
            //ogPos
            tile.transform.position = new Vector3(tile.transform.position.x, tile.targetOpen, tile.transform.position.z);
            tile.lerpSpeed = 0f;
            tile.opening = false;
            UpdateAdjTiles(otherTile.GetComponent<GridTiles>(), (int)otherTile.transform.position.x, (int)otherTile.transform.position.z);
            yield return new WaitForSeconds(.41f);
            if (tile.walkable)
            {
                tile.GetComponent<GridTiling>().SetDirectionalMaterial();
            }
        }
        else
        {
            //not yet
            yield return new WaitForEndOfFrame();
            StartCoroutine(CloseTileMovement(tile, speed, curve, levelTransiIndex, otherTile));
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
