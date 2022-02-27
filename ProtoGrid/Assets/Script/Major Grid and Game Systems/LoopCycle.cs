using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCycle : MonoBehaviour
{
    public int tempoValue;
    GridTiles[,] grid;
    StepAssignement sAss;
    PlayerMovement pMov;
    Reset reset;
    int resetTimer;
    int inverseResetTimer;
    int resetStart;
    [HideInInspector] public int tempoIndexValue;
    bool flag = true;
    int maxIndexValue = 0;
    Transform Player;
    

    private void Awake()
    {
        reset = GetComponent<Reset>();
        resetStart = reset.resetTimerValue;
        sAss = GetComponent<StepAssignement>();
        pMov = GetComponent<PlayerMovement>();
        
        grid = GetComponent<GridGenerator>().grid;
        Player = FindObjectOfType<Player>().transform;
        foreach(GridTiles obj in grid)
        {
            if(obj.tempoTile > maxIndexValue)
            {
                maxIndexValue = obj.tempoTile;
            }
        }

    }

    private void Update()
    {
        resetTimer = reset.resetTimer;
        inverseResetTimer = resetStart - resetTimer;
        if (maxIndexValue > 1)
        {

            if (flag)
        {
            if(inverseResetTimer % tempoValue >= tempoValue - 1)
            {
                tempoIndexValue++;
                pMov.highlightedTiles.Clear();
                StartCoroutine(InvokeIni());
                
                flag = false;
            }
        }

        if (inverseResetTimer % tempoValue < tempoValue - 1)
        {
            flag = true;
        }
      
        tempoIndexValue %= maxIndexValue;        
    }

    IEnumerator InvokeIni()
    {
        yield return new WaitForSeconds(.1f);
        sAss.Initialisation();
        Player.position = new Vector3(Player.position.x, grid[(int)Player.position.x, (int)Player.position.z].transform.position.y + 1.5f, Player.position.z);
    }
        }









    /*    private void Update()
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
            if(tempoChangeValue == 0)
            {
                tempoChangeValue = 1;
            }
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
        }*/
}
