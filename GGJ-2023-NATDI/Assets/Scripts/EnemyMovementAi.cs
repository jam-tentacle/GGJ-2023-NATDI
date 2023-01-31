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
        StartCoroutine(CalcSpeed());
    }

    private void FixedUpdate()
    {
        ChaseMushroom();

        if (_mushroom == null) return;

        _mushroom = Services.Get<CollectionService>().GetNearestMushroom(transform.position);
        var distance = Vector3.Distance(transform.position, _mushroom.Position);
        if (distance < 1f)
        {
            _passedTime += Time.deltaTime;
            if (_passedTime > 2)
            {
                Services.Get<CollectionService>().RemoveMushroom(_mushroom);
                Destroy(_mushroom.gameObject);
                SetTarget();
                _passedTime = 0f;
            }
        }

        _characterAnimator.SetVelocityZ(_velocity < 0.1f ? 0f : 1f);

        // Debug.Log(_velocity);
        _characterAnimator.SetMoving(true);
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
}
