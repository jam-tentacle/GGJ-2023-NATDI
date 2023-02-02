using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAi : MonoBehaviour, ITarget
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CharacterAnimator _characterAnimator;
    [SerializeField] private Damageable _damageable;
    [SerializeField] private Transform _shootPoint;
    private Mushroom _mushroom;
    private Vector3 _walkPoint;
    private bool _alreadyAttacked;
    private bool _walkPointSet;
    private bool _isGathering;
    private CollectionService _collectionService;

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => _shootPoint.position;
    public Vector3 Velocity => _agent.velocity;

    private void Start()
    {
        _collectionService = Services.Get<CollectionService>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _characterAnimator.Move += OnCharacterMove;
        _characterAnimator.GatherEnded += OnCharacterGatherEnded;
        _characterAnimator.DyingEnded += OnCharacterDyingEnded;
        Services.Get<CollectionService>().AddMushroomer(this);
        _damageable.HealthChanged += OnHealthChanged;
        _damageable.Died += OnDied;
    }

    private void OnDied()
    {
        Debug.Log("Died");
        _characterAnimator.SetDying();
    }

    private void OnHealthChanged(float deltaHealth, Vector3 impulse)
    {
        Debug.Log("Health changed");
    }

    private void OnCharacterDyingEnded() => Destroy(gameObject);

    private void OnCharacterGatherEnded()
    {
        _isGathering = false;

        Services.Get<SpawnerService>().DespawnMushroom(_mushroom);
        SetTarget();
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("HitReceived");
            _damageable.ReceiveHit(50f, Vector3.zero);
        }

        if (_isGathering)
        {
            return;
        }

        if (_mushroom == null)
        {
            SetTarget();
        }

        if (_mushroom == null)
        {
            return;
        }

        bool moving = true;
        float distance = Vector3.Distance(transform.position, _mushroom.Position);
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

    private void SetTarget()
    {
        _mushroom = _collectionService.GetNearestMushroom(transform.position);

        if (_mushroom == null)
        {
            return;
        }

        transform.LookAt(_mushroom.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _agent.SetDestination(_mushroom.Position);
    }

    private void OnDrawGizmos()
    {
        if (_mushroom == null)
        {
            return;
        }

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(_mushroom.transform.position, Vector3.up, 1);
    }
}
