using UnityEngine;

public class SporeProjectile : Projectile
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _impulseMultiplier = 10;
    public override void Launch(Vector3 targetPosition)
    {
        Vector3 impulse = targetPosition - transform.position;
        impulse = impulse.normalized;
        impulse.y = 0.4f;
        impulse *= _impulseMultiplier;
        _rigidbody.AddForce(impulse, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        InvokeHit();
    }
}
