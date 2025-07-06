using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Attack = true;

        if (Player.Instance.OnGround)
        {
            Player.Instance.Body.velocity = Vector2.zero;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Attack = false;
        animator.ResetTrigger("uppercut");
        animator.ResetTrigger("throw");
    }
}
