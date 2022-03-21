using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : StateMachineBehaviour
{
    bool awake = true;
    GameObject inGameUI;
    GameObject PauseMenu;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (awake)
        {
            PauseMenu = FindObjectOfType<PauseMenuUI>().gameObject;
            inGameUI = FindObjectOfType<InGameUI>().gameObject;
            awake = false;

        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        
    }
    
}
