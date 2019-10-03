using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_ANIMATION_PMD : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("died", false); //Clear "died" flag
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!animator.GetBool("died") && stateInfo.normalizedTime >= 1.0) {
            animator.SetBool("died", true); //When animation ends, notify everyone about it with setting "died" as true
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("died", false); //Clear "died" flag
    }
    
}
