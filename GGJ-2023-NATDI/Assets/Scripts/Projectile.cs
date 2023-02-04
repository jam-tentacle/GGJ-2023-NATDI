using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, ITarget
{
    public event Action<Vector3, Projectile> Hit;
    public abstract void Launch(Vector3 targetPosition);

    protected void InvokeHit()
    {
        Hit?.Invoke(transform.position, this);
        Destroy(gameObject);
    }

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => transform.position;
    public Vector3 Velocity => Vector3.zero;
    public bool IsAlive => this != null;
}
