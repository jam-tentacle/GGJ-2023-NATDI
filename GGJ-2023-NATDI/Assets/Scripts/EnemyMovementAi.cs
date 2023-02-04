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

    private MushroomTargeter _targeter = new MushroomTargeter();

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => _shootPoint.position;
    public Vector3 Velocity => _agent.velocity;
    public bool IsAlive => this != null;

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

        _characterAnimator.SetVelocityZ(_agent.velocity.magnitude);
        _characterAnimator.SetMoving(moving);
        _agent.nextPosition = transform.position;
    }

    private void OnTargetUpdated()
    {
        transform.LookAt(_targeter.CurrentMushroom.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
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
