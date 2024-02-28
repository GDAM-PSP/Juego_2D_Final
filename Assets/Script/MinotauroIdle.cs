using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotauroIdle : StateMachineBehaviour
{
    public float distanciaMovimiento = 10f;
    private Minotaur minotaur;
    private GameObject player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minotaur = GameObject.Find("Minotaur").GetComponent<Minotaur>();
        player = GameObject.Find("PlayerRed");
        float distanceToPlayer = Vector3.Distance(minotaur.transform.position, player.transform.position);
        if (distanceToPlayer >= distanciaMovimiento)
        {
            // El enemigo est� dentro del rango de espera, no hace nada
        }
        else
        {     
            animator.SetInteger("numeroAleatorio",UnityEngine.Random.Range(0,2));
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = Vector3.Distance(minotaur.transform.position, player.transform.position);
    }




    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}