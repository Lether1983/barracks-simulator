﻿using UnityEngine;
using System.Collections;

public class EatFoodState : StateMachineBehaviour
{
    Soldiers me;
    Evaluator evaluator;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (me == null)
        {
            me = animator.gameObject.GetComponent<Soldiers>();
        }
		if(evaluator == null)
		{
			evaluator = animator.gameObject.GetComponent<Evaluator>();
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((Vector2)me.transform.position == evaluator.tempObject.position)
        {
            
            if (me.hungry > 0)
            {
                me.hungry -= (5 / 3600f+76f / 1800f) * Time.deltaTime * me.manager.speed;
            }
            else
            {
                animator.SetBool("EatFood", false);
            }
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        
        animator.SetBool("EatFood", false);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
