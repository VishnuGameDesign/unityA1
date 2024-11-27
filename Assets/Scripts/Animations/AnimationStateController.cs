using UnityEngine;

public class AnimationStateController : StateMachineBehaviour
{
    [SerializeField] private bool _applyRootMotion = false;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _canShoot = true;

    private CustomCharacterMovement _movement;

    // acts like OnEnable
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // the 'animator' we receive here is the animator on our character
        // which means we can get access to the gameObject, transform, or any components we need
        _movement = animator.GetComponent<CustomCharacterMovement>();

        // set movement states
        _movement.CanMove = _canMove;
        animator.applyRootMotion = _applyRootMotion;
    }

    // acts like Update
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    // acts like OnDisable
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    // 'Move" refers to root motion movement
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if(_applyRootMotion)
        {
            // tell animator to continue with normal root motion
            animator.ApplyBuiltinRootMotion();

            // force character to aim in LookDirection (Quaternion.LookRotation turns a Vector3 Direction into a Quaternion Rotation)
            animator.transform.rotation = Quaternion.LookRotation(_movement.LookDirection);
        }
    }
}