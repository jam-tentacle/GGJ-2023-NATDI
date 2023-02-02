using UnityEngine;

public interface ITarget
{
    Vector3 Position { get; }
    Vector3 ShootTargetPosition { get; }
    Vector3 Velocity { get; }
    bool IsAlive { get; }
}
