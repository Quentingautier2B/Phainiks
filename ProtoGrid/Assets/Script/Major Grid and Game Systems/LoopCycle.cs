using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCycle : MonoBehaviour
{
    #region Variables

    public int redTimer = 0;
    [HideInInspector] public int redOffValue = 1;
    [HideInInspector] public bool redFlag = false;

    public int blueTimer = 0;
    [HideInInspector] public int blueOffValue = 2;
    [HideInInspector] public bool blueFlag = false;

    public int greenTimer = 0;
    [HideInInspector] public int greenOffValue = 3;
    [HideInInspector] public bool greenFlag = true;

    #endregion


    private void Update()
    {
        NewTempoTile();
    }

    public void tempoTileIncr()
        {
        if (redFlag)
            redTimer--;

        if (!redFlag)
            redTimer++;




        if (blueFlag)
            blueTimer--;

        if (!blueFlag)
            blueTimer++;




        if (greenFlag)
            greenTimer--;

        if (!greenFlag)
            greenTimer++;
    }

    void NewTempoTile()
    {
        if (redTimer <= 0)
            redFlag = false;

        if (redTimer >= redOffValue)
            redFlag = true;



        if (blueTimer <= 0)
            blueFlag = false;

        if (blueTimer >= blueOffValue)
            blueFlag = true;




        if(greenTimer <= 0)
            greenFlag = false;

        if (greenTimer >= greenOffValue)
            greenFlag = true;
    }
}
