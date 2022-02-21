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

    [Header("Input Values")]
    [Space]
    public int resetTimer;
    public int resetTimerValue;
    #endregion

    private void Awake()
    {
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
        player.gameObject.GetComponent<NavMeshAgent>().SetDestination(pMovement.ogPos);

        player.position = pMovement.ogPos;
    }
}
