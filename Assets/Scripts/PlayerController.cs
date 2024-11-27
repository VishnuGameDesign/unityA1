using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// enforce components in this GameObject
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : Controller
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private float _aimOffset = 1f;
    [SerializeField] private float _interactRadius = 1f;

    // if we want to serialize interfaces, we have to use SerializeReference
    // this technically serialized the object behind the interface
    public IInteractable InteractTarget { get; private set; }

    // common addition to standard C# naming conventions - private fields get a leading _ (underscore)
    private Vector2 _moveInput2D;
    private Vector3 _aimPosition;

    public UnityEvent<IInteractable> OnInteractTargetChanged;

    // called by PlayerInput component
    public void OnJump()
    {
        if(!Health.IsAlive) return;
        Movement.TryJump();
    }

    // called by PlayerInput component, sends Vector2 direction value
    public void OnMove(InputValue inputValue)
    {
        if (!Health.IsAlive) return;

        // Get<> converts the inputValue into our expect type
        _moveInput2D = inputValue.Get<Vector2>();

        // debug input visually
        Vector3 moveInput3D = new Vector3(_moveInput2D.x, 0f, _moveInput2D.y);
        Debug.DrawRay(transform.position, moveInput3D * 2f, Color.magenta, 0.5f);
    }

    public void OnTeleport()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
        {
            // teleport to cursor location
            Vector3 location = hitInfo.point;
            Movement.Teleport(location);
        }
    }

    public void OnShoot()
    {
        if (!Health.IsAlive) return;

        // assume shotgun is weapon 0
        Weapon shotgun = Weapons[0];
        shotgun.TryAttack(_aimPosition, gameObject, Targetable.Team);
    }

    public void OnMelee()
    {
        if (!Health.IsAlive) return;

        // assume baseball bat is weapon 1
        Weapon bat = Weapons[1];
        bat.TryAttack(_aimPosition, gameObject, Targetable.Team);
    }

    public void OnInteract()
    {
        if (!Health.IsAlive) return;

        InteractTarget?.Interact(gameObject);
    }

    private void Update()
    {
        if(!Health.IsAlive)
        {
            Movement.Stop();
            return;
        }

        // map 2D input to 3D space before moving character
        Vector3 right = Camera.main.transform.right;    // thumb
        Vector3 up = Vector3.up;                        // pointer finger
        Vector3 forward = Vector3.Cross(right, up);     // middle finger
        Vector3 moveInput3D = forward * _moveInput2D.y + right * _moveInput2D.x;

        // send move input to movement component
        Movement.SetMoveInput(moveInput3D);

        // find ray travelling through mouse into world
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
        {
            // make character look at hit point
            Movement.SetLookPosition(hitInfo.point);

            // find aim position
            Vector3 offset = -mouseRay.direction * _aimOffset;
            _aimPosition = hitInfo.point + offset;
        }

        // handle interaction
        Collider[] interactors = Physics.OverlapSphere(transform.position, _interactRadius, _interactMask);
        IInteractable currentInteractTarget = InteractTarget;
        InteractTarget = null;
        foreach(Collider interactor in interactors)
        {
            if(interactor.TryGetComponent(out IInteractable interactable))
            {
                InteractTarget = interactable;
                break;
            }
        }
        if (InteractTarget != currentInteractTarget) OnInteractTargetChanged.Invoke(InteractTarget);
    }
}