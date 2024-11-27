using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] private Targetable _target;

    [SerializeField] private float _waypointReachedDistance = 1f;
    [SerializeField] private float _wanderSpeedMultiplier = 0.5f;

    [BoxGroup("Debug"), ShowInInspector] private string _currentStateName;

    // useful properties
    public bool IsTargetValid => _target != null && _target.IsTargetable;
    public bool IsTargetVisible => Vision.TestVisibility(_target.transform.position);
    public float TargetDistance => Vector3.Distance(_target.transform.position, transform.position);
    public Vector3 TargetPosition => _target.transform.position;

    private IEnumerator _currentState;
    private Waypoint[] _waypoints;

    private void Start()
    {
        ChangeState(WanderState());

        // this is a one-line event listener lamdba expression
        // AddListener allows us to add a function that responds to the OnDeath event
        // (DamageInfo info) => sets up a "lambda expression" and passes the DamageInfo parameter into our code
        // in between { } is our actual function
        Health.OnDeath.AddListener( (DamageInfo info) => { ChangeState(DeadState()); });
        Health.OnDamage.AddListener(Damaged);

        // find all waypoints in scene(s)
        _waypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
    }

    private void Damaged(DamageInfo damageInfo)
    {
        if(damageInfo.Instigator.TryGetComponent(out Targetable attacker) && attacker.Team != Targetable.Team)
        {
            // enemy damaged us
            _target = attacker;
        }
    }

    private void ChangeState(IEnumerator newState)
    {
        Movement.MoveSpeedMultiplier = 1f;

        // stop current state
        if(_currentState != null) StopCoroutine(_currentState);

        // assign and start new state
        _currentState = newState;
        _currentStateName = _currentState.ToString();
        StartCoroutine(_currentState);
    }

    private void TryFindTarget()
    {
        if (IsTargetValid) return;      // stop if we have target
        _target = Vision.GetFirstVisibleTarget(Targetable.Team);
    }

    private Waypoint FindRandomWaypoint()
    {
        if (_waypoints == null || _waypoints.Length == 0) return null;

        int randomIndex = Random.Range(0, _waypoints.Length);   // Random.Range(int) is maxExclusive (it subtracts 1 from the max value automatically)
        return _waypoints[randomIndex];
    }

    private IEnumerator WanderState()
    {
        Waypoint waypoint = null;
        Movement.MoveSpeedMultiplier = _wanderSpeedMultiplier;

        // wander until valid target is spotted, then attack
        while(!IsTargetValid)
        {
            TryFindTarget();

            // find waypoint to move to
            if(waypoint == null || Vector3.Distance(waypoint.Position, transform.position) < _waypointReachedDistance) waypoint = FindRandomWaypoint();
            else Movement.MoveTo(waypoint.Position);

            yield return null;
        }

        ChangeState(AttackState());
    }

    private IEnumerator AttackState()
    {
        // enter state
        // find first equipped weapon
        Weapon weapon = Weapons[0];
        float attackDistance = weapon.EffectiveRange;

        // update state
        while(_target != null)
        {
            float distance = Vector3.Distance(transform.position, TargetPosition);
            if (distance > attackDistance)
            {
                // chasing target
                Movement.MoveTo(TargetPosition);
                Debug.DrawLine(transform.position, TargetPosition, Color.yellow);
            }
            else
            {
                // stopping and staring
                Movement.Stop();
                Movement.SetLookPosition(TargetPosition);
                Debug.DrawLine(transform.position, TargetPosition, Color.red);

                // attempt to attack with weapon
                if(weapon.TryAttack(TargetPosition + Vector3.up, gameObject, Targetable.Team))
                {
                    // if attack activates successfully, wait for duration of attack
                    yield return new WaitForSeconds(weapon.Duration);
                }
            }

            // returns at the end of every loop, and waits until next frame to continue
            yield return null;
        }

        // exit state
    }

    private IEnumerator DeadState()
    {
        Movement.Stop();
        yield return null;
    }
}