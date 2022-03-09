using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCycle : MonoBehaviour
{
    #region Variables

    [Header("Input Values")]
    [Space]
    [HideInInspector] public int tempoValue;

    [Header("Components")]
    [Space]
    GridTiles[,] grid;
    StepAssignement sAss;
    PlayerMovement pMov;
    Reset reset;
    Transform Player;

    [Header("Hidden Values")]
    [Space]
    int resetTimer;
    int inverseResetTimer;
    int resetStart;
    [HideInInspector] public int tempoIndexValue;
    bool flag = true;
    int maxIndexValue = 0;
    #endregion


     public int redTimer = 0;
    [HideInInspector] public int redOffValue = 2;
    [HideInInspector] public bool redFlag = false;

     public int blueTimer = 0;
    [HideInInspector] public int blueOffValue = 3;
    [HideInInspector] public bool blueFlag = false;

     public int greenTimer = 0;
    [HideInInspector] public int greenOffValue = 4;
    [HideInInspector] public bool greenFlag = true;


    private void Awake()
    {
        reset = GetComponent<Reset>();  
        sAss = GetComponent<StepAssignement>();
        pMov = GetComponent<PlayerMovement>();
        
        grid = GetComponent<GridGenerator>().grid;
        Player = FindObjectOfType<Player>().transform;
        
        
        //resetStart = reset.resetTimerValue;
        /*foreach(GridTiles obj in grid)
        {
            if(obj.tempoTile > maxIndexValue)
            {
                maxIndexValue = obj.tempoTile;
            }
        }*/

    }

    private void Update()
    {
        /*inverseResetTimerValueSet();

        if (maxIndexValue > 1)
        {
            tempoTileCycleIncr();
        }*/

        NewTempoTile(redOffValue,redTimer,redFlag);


    }




    void NewTempoTile(int offTime, int timerd, bool flagd)
    {
        if (redFlag && pMov.moveFlag)
            redTimer--;

        if (!redFlag && pMov.moveFlag)
            redTimer++;

        if (redTimer <= 0)
            redFlag = false;

        if (redTimer >= redOffValue)
            redFlag = true;



        if (blueFlag && pMov.moveFlag)
            blueTimer--;

        if (!blueFlag && pMov.moveFlag)
            blueTimer++;

        if (blueTimer <= 0)
            blueFlag = false;

        if (blueTimer >= blueOffValue)
            blueFlag = true;



        if (greenFlag && pMov.moveFlag)
            greenTimer--;

        if (!greenFlag && pMov.moveFlag)
            greenTimer++;

        if(greenTimer <= 0)
            greenFlag = false;

        if (greenTimer >= greenOffValue)
            greenFlag = true;
    }









   /* void inverseResetTimerValueSet()
    {
        resetTimer = reset.resetTimer;
        inverseResetTimer = resetStart - resetTimer;
    }*/

    /*void tempoTileCycleIncr()
    {

        

        if (flag)
        {
            if (inverseResetTimer % tempoValue >= tempoValue - 1)
            {
                tempoIndexValue++;
                pMov.highlightedTiles.Clear();
                pMov.currentPathIndex = 0;
                StartCoroutine(InvokeIni());
                flag = false;
            }
        }

        if (inverseResetTimer % tempoValue < tempoValue - 1)
        {
            flag = true;
        }
        tempoIndexValue %= maxIndexValue;
    }*/


    //Rubberband fix to change later
/*    IEnumerator InvokeIni()
    {
        yield return new WaitForSeconds(.1f);
        sAss.Initialisation();
        Player.position = new Vector3(Player.position.x, grid[(int)Player.position.x, (int)Player.position.z].transform.position.y + 1.5f, Player.position.z);
    }*/


}
