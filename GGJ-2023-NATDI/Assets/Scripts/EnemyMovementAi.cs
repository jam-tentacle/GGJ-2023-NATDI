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
    private float _velocity;
    private bool _alreadyAttacked;
    private bool _walkPointSet;
    private float _passedTime;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        StartCoroutine(CalcSpeed());
        _characterAnimator.Move += OnCharacterMove;
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
            moving = false;
            _passedTime += Time.deltaTime;
            if (_passedTime > 2)
            {
                Services.Get<CollectionService>().RemoveMushroom(_mushroom);
                Destroy(_mushroom.gameObject);
                SetTarget();
                _passedTime = 0f;
            }
        }

        _characterAnimator.SetVelocityZ(_agent.velocity.magnitude);

        Debug.Log(_agent.velocity.magnitude);
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

    private IEnumerator CalcSpeed()
    {
        while (true)
        {
            Vector3 prevPos = transform.position;
            yield return new WaitForEndOfFrame();

            _velocity = (Vector3.Distance(transform.position, prevPos) / Time.deltaTime) / 10;
        }
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
