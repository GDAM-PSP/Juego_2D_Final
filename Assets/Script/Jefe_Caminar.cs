using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Jefe_Caminar : StateMachineBehaviour
{
    private Minotaur minotaur;
    private GameObject player;
    private Rigidbody2D rb2D;
    [SerializeField] private float velocidadMovimiento;
    public float distanciaMovimiento = 10;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minotaur = animator.GetComponent<Minotaur>();
        player = GameObject.Find("PlayerRed");
        rb2D = minotaur.rb;

        minotaur.MirarJugador();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = Vector3.Distance(minotaur.transform.position, player.transform.position);

        if (distanceToPlayer >= distanciaMovimiento)
        {
            // El enemigo est� dentro del rango de espera, no hace nada
        }
        else
        {
            rb2D.velocity = new Vector2(velocidadMovimiento, rb2D.velocity.y) * animator.transform.right;
        }

       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }

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
