using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    public event Action<Vector3, Quaternion> Move;

    private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
    private static readonly int Moving = Animator.StringToHash("Moving");

    private Animator _animator;
    private float _velocity;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetVelocityZ(float value)
    {
        _animator.SetFloat(VelocityZ, value);
    }

    public void SetMoving(bool value)
    {
        _animator.SetBool(Moving, value);
    }

    private void OnAnimatorMove()
    {
        Move?.Invoke(_animator.deltaPosition, _animator.rootRotation);
    }
}
