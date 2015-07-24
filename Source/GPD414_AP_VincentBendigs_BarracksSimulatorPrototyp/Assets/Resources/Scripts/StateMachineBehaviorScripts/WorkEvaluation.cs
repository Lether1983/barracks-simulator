using UnityEngine;
using System.Collections;

public class WorkEvaluation : StateMachineBehaviour
{
    WorkEvaluator evaluator;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (evaluator == null)
        {
            evaluator = animator.gameObject.GetComponent<WorkEvaluator>();
        }
        evaluator.GetEvaluationForWork();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
