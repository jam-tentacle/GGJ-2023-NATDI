using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAi : MonoBehaviour, ITarget
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CharacterAnimator _characterAnimator;
    [SerializeField] private Damageable _damageable;
    [SerializeField] private Transform _shootPoint;
    private Vector3 _walkPoint;
    private bool _alreadyAttacked;
    private bool _walkPointSet;
    private bool _isGathering;

    private MushroomTargeter _targeter = new();

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => _shootPoint.position;
    public Vector3 Velocity => _agent.velocity;
    public bool IsAlive => this != null && !_damageable.Dead;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _characterAnimator.Move += OnCharacterMove;
        _characterAnimator.GatherEnded += OnCharacterGatherEnded;
        _characterAnimator.DyingEnded += OnCharacterDyingEnded;
        Services.Get<CollectionService>().AddMushroomer(this);
        _damageable.HealthChanged += OnHealthChanged;
        _damageable.Died += OnDied;

        _targeter.Init(transform, OnTargetUpdated);
    }

    private void OnDied()
    {
        Debug.Log("Died");
        _targeter.Stop();
        _characterAnimator.SetDying();
        _agent.enabled = false;
    }

    private void OnHealthChanged(float deltaHealth, Vector3 impulse)
    {
        Debug.Log("Health changed");
    }

    private void OnCharacterDyingEnded() => Services.Get<SpawnerService>().DespawnMushroomer(this);

    private void OnCharacterGatherEnded()
    {
        _isGathering = false;

        Services.Get<SpawnerService>().DespawnMushroom(_targeter.CurrentMushroom);
        _targeter.TryChangeTarget();
    }

    private void OnCharacterMove(Vector3 delta, Quaternion rotation)
    {
        Vector3 pos = transform.position;
        pos += delta;
        pos.y = _agent.nextPosition.y;
        transform.position = pos;
        _agent.nextPosition = transform.position;
    }

    private void OnDestroy()
    {
        _characterAnimator.Move -= OnCharacterMove;
        _characterAnimator.GatherEnded -= OnCharacterGatherEnded;
        _characterAnimator.DyingEnded -= OnCharacterDyingEnded;
        _damageable.HealthChanged -= OnHealthChanged;
        _damageable.Died -= OnDied;
    }

    private void FixedUpdate()
    {
        _targeter.FixedUpdate(Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("HitReceived");
            _damageable.ReceiveHit(50f, Vector3.zero);
        }

        if (_isGathering)
        {
            return;
        }

        if (_targeter.CurrentMushroom == null)
        {
            return;
        }

        bool moving = true;
        float distance = Vector3.Distance(transform.position, _targeter.CurrentMushroom.Position);
        if (distance < 1f)
        {
            _isGathering = true;
            _characterAnimator.SetGather();
            moving = false;
        }

        if (moving)
        {
            RotateTowardsMovementDir();
        }

        _characterAnimator.SetVelocityZ(_agent.velocity.magnitude);
        _characterAnimator.SetMoving(moving);
        _agent.nextPosition = transform.position;
    }

    private void RotateTowardsMovementDir()
    {
        if (_agent.velocity.sqrMagnitude < 0.01f) return;

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_agent.velocity),
            Time.deltaTime * _agent.angularSpeed);

        // Keep X and Z rotation at 0.
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(0, q.eulerAngles.y, 0);
        transform.rotation = q;
    }

    private void OnTargetUpdated()
    {
        _agent.SetDestination(_targeter.CurrentMushroom.Position);
    }

    private void OnDrawGizmos()
    {
        if (_targeter.CurrentMushroom == null)
        {
            return;
        }

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(_targeter.CurrentMushroom.transform.position, Vector3.up, 1);
    }
}
