using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CharacterAnimator _characterAnimator;
    private Mushroom _mushroom;
    private Vector3 _walkPoint;
    private bool _alreadyAttacked;
    private bool _walkPointSet;
    private float _passedTime; //TEMP VARIABLE for Dying animation test
    private bool _isGathering;
    private bool _isDying;
    private CollectionService _collectionService;

    private void Start()
    {
        _collectionService = Services.Get<CollectionService>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _characterAnimator.Move += OnCharacterMove;
        _characterAnimator.GatherEnded += OnCharacterGatherEnded;
        _characterAnimator.DyingEnded += OnCharacterDyingEnded;
    }

    private void OnCharacterDyingEnded()
    {
        _isDying = false;
        Debug.Log(_isDying);
    }

    private void OnCharacterGatherEnded()
    {
        _isGathering = false;

        Services.Get<SpawnerService>().DespawnMushroom(_mushroom);
        SetTarget();
    }

    private void OnCharacterMove(Vector3 delta, Quaternion rotation)
    {
        transform.position += delta;

        transform.rotation = rotation;
    }

    private void OnDestroy()
    {
        _characterAnimator.Move -= OnCharacterMove;
        _characterAnimator.GatherEnded -= OnCharacterGatherEnded;
        _characterAnimator.DyingEnded -= OnCharacterDyingEnded;
    }

    private void FixedUpdate()
    {
        if (_isDying)
        {
            return;
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

    private void KillCharacter()
    {
        _characterAnimator.SetDying();
        // Destroy(gameObject);
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
