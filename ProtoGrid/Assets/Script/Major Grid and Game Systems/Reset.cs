using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Reset : MonoBehaviour
{
    #region variables
    [Header("Component References")]
    [Space]
    Transform player;
    PlayerMovement pMovement;
    StepAssignement sAssignement;
    GridGenerator gridGenerator;

    [Header("Input Values")]
    [Space]
    public int resetTimer;
    public int resetTimerValue;
    #endregion

    private void Awake()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
        player = FindObjectOfType<Player>().transform;
        sAssignement = FindObjectOfType<StepAssignement>();
        pMovement = FindObjectOfType<PlayerMovement>();
        resetTimer = resetTimerValue;    
    }


    private void Update()
    {
        if(resetTimer <= 0)
        {
            
            ResetEffect();
            resetTimer = resetTimerValue;
        }
    }

    void ResetEffect()
    {
        ResObjects();
        ResPlayerPos();
        ResHighLight();
        sAssignement.Initialisation();
    }

    void ResHighLight()
    {

        foreach (GridTiles obj in pMovement.highlightedTiles)
        {
            obj.highLight = false;
        }

        pMovement.highlightedTiles.Clear();
    }

    void ResPlayerPos()
    {
        pMovement.moveState = false;
        pMovement.currentPathIndex = 0;
        player.position = pMovement.ogPos;
    }

    void ResObjects()
    {
        foreach(GridTiles obj in gridGenerator.grid)
        {
            Transform objT = obj.transform;
            if (objT.Find("Timer+"))
            {
                obj.timerChangeValue = obj.timerChangeInputValue;
                objT.Find("Timer+").Find("TimerPSys").GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
