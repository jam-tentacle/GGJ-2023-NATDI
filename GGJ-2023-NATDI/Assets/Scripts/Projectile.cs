using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public event Action<Vector3> Hit;
    public abstract void Launch(Vector3 targetPosition);

    protected void InvokeHit()
    {
        Hit?.Invoke(transform.position);
        Destroy(gameObject);
    }
}
