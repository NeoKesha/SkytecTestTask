using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_ANIMATION_TOP_SHOOTING : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("shooting", true); 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("shooting", false);
    }
}
