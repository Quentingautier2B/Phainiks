using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCycle : MonoBehaviour
{
    GridTiles[,] grid;
    Reset reset;
    public int resetTimer;
    public int timerStart;
    public List<GridTiles> tempoTiles;
    public int tempoChangeValue;
    public int reverseResetTempo = 0;
    public int tempoIndex;
    public int maxIndex;
    public int index = 1;

    private void Awake()
    {
        reset = GetComponent<Reset>();
        timerStart = reset.resetTimerValue;
        grid = GetComponent<GridGenerator>().grid;
        maxIndex = 0;
        CreateTempoTilesList();
        TempoChanger();


    }
    private void Update()
    {
        resetTimer = reset.resetTimer;
        reverseResetTempo = timerStart - resetTimer;

        TempoChanger();
    }

    void CreateTempoTilesList()
    {
        foreach (GridTiles obj in grid)
        {
            if (obj.tempoTile > 0)
                tempoTiles.Add(obj);

            if (obj.tempoTile > maxIndex)
                maxIndex = obj.tempoTile;
        }
    }

    void TempoChanger()
    {
        tempoIndex = (index) / tempoChangeValue;
        if (index < reverseResetTempo)
        {
            index += tempoChangeValue;
        }

        if(tempoIndex >= maxIndex)
        {
            index = 1;
            timerStart = resetTimer;
        }
    }
}
