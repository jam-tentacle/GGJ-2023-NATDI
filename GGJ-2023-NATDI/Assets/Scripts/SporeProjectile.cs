using Sirenix.Serialization;
using System;
using UnityEngine;

public class SporeProjectile : Projectile
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _impulseMultiplier = 10;
    [SerializeField] private LayerMask _layerMask;
    private float _passedTime;
    private bool _projectileExist;
    private bool _collisionEntered;
    private bool _mushroomAreaEntered;

    public override void Launch(Vector3 targetPosition)
    {
        _projectileExist = true;
        Vector3 impulse = targetPosition - transform.position;
        impulse = impulse.normalized;
        impulse.y = 0.4f;
        impulse *= _impulseMultiplier;
        _rigidbody.AddForce(impulse, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionEntered = true;
    }

    private void FixedUpdate()
    {
        _mushroomAreaEntered = Physics.SphereCast(transform.position + Vector3.up * 10, 1f, Vector3.down,
            out RaycastHit hitInfo, Mathf.Infinity, _layerMask);
        Debug.Log(_mushroomAreaEntered);

        if (_projectileExist)
        {
            _passedTime += Time.deltaTime;
        }

        if (!_collisionEntered || !(_passedTime >= 2f)) return;
        if (_mushroomAreaEntered) return;

        InvokeHit();
        _collisionEntered = false;
        _projectileExist = false;
        _passedTime = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
