using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMovementAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    // [SerializeField] private LayerMask _whatIsGround;
    // [SerializeField] private float _walkPointRange;
    [SerializeField] private float _sightRange;
    [SerializeField] private float _requiredRange;
    [SerializeField] private bool _attacking;
    private ITarget _target;
    private Vector3 _walkPoint;
    private float _velocity;
    // private int _idleTime;
    private bool _alreadyAttacked;
    private bool _walkPointSet;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        SetTarget();
        // StartCoroutine(IdleChecker());
    }


    private void FixedUpdate()
    {
        // Vector3 agentPosition = transform.position;
        // Vector3 playerPosition = _target.Position;
        // float distance = Vector3.Distance(agentPosition, playerPosition);
        // bool inSight = Vector3.SignedAngle(playerPosition - agentPosition,
        //     transform.forward, new Vector3(0, 0, 1)) < 90f;
        // if (_idleTime == 3)
        // {
        //     SearchWalkPoint();
        //     _idleTime = 0;
        // }
        ChasePlayer();
    }

    // private IEnumerator IdleChecker()
    // {
    //     while (true)
    //     {
    //         Vector3 prevPos = transform.position;
    //         yield return new WaitForEndOfFrame();
    //         _velocity = (Vector3.Distance(transform.position, prevPos) / Time.deltaTime) / 10;
    //         if (_velocity == 0)
    //         {
    //             _idleTime += 1;
    //             yield return new WaitForSeconds(1f);
    //         }
    //     }
    // }



    private void ChasePlayer()
    {
        if (_attacking) return;
        _agent.SetDestination(_target.Position);
        _agent.speed = gameObject.name == "CasterEnemy" ? 2f : 3.5f;
    }

    // private void SearchWalkPoint()
    // {
    //     float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
    //     float randomX = Random.Range(-_walkPointRange, _walkPointRange);
    //     Vector3 position = transform.position;
    //     _walkPoint = new Vector3(position.x + randomX, position.y, position.z + randomZ);
    //     if (Physics.Raycast(_walkPoint, -transform.up, 2f, _whatIsGround))
    //         _walkPointSet = true;
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, _requiredRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, _sightRange);
    }

    private void SetTarget()
    {
        _target = Services.Get<CollectionService>().GetNearestMushroom(transform.position);
    }
}
