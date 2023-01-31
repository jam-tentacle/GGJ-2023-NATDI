using System;
using System.Collections;
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
    private float _passedTime;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _characterAnimator.Move += OnCharacterMove;
        _characterAnimator.GatherEnded += OnCharacterGatherEnded;
    }

    private void OnCharacterGatherEnded()
    {
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
    }

    private void FixedUpdate()
    {
        ChaseMushroom();

        if (_mushroom == null) return;

        _mushroom = Services.Get<CollectionService>().GetNearestMushroom(transform.position);
        bool moving = true;
        var distance = Vector3.Distance(transform.position, _mushroom.Position);
        if (distance < 1f)
        {
            _characterAnimator.SetGather();
            moving = false;
        }


        _characterAnimator.SetVelocityZ(_agent.velocity.magnitude);

        _characterAnimator.SetMoving(moving);
        _agent.nextPosition = transform.position;
    }

    private void ChaseMushroom()
    {
        if (_mushroom == null)
        {
            SetTarget();
        }

        _agent.SetDestination(_mushroom.Position);
    }

    private void SetTarget()
    {
        _mushroom = Services.Get<CollectionService>().GetNearestMushroom(transform.position);
        transform.LookAt(_mushroom.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
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
