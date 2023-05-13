using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotForward : StateMachineBehaviour
{
    //referencia al controller principal
    private Enemy enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<Enemy>();
        if (enemy == null) return;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null) return;

        if (enemy.nav.enabled && enemy.target != null)
        {
            enemy.nav.SetDestination(enemy.target.position);
        }

        if (!enemy.nav.pathPending &&
            enemy.attackDistance >= enemy.nav.remainingDistance
            && enemy.active)
        {
            enemy.anim.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null) return;

        animator.ResetTrigger("Attack");
    }
}
