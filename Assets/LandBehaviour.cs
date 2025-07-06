using UnityEngine;

public class LandBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player.Instance.OnGround)
        {
            animator.SetBool("land", false);
            animator.ResetTrigger("jump");
        }
    }
}
