using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Controller _controller;
    [SerializeField] private int _meleeWeaponIndex = 1;

    private void OnValidate()
    {
        if(_animator == null) _animator = GetComponent<Animator>();
        if(_controller == null) _controller = GetComponent<Controller>();
    }

    // called from Animator component
    public void MeleeCancelAnimEvent()
    {
        _animator.ResetTrigger("Melee");
    }

    // called from Animator component
    public void MeleeHitAnimEvent(int attackIndex)
    {
        _controller.Weapons[_meleeWeaponIndex].AttackAnimEvent(attackIndex);
    }
}