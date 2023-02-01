using JetBrains.Annotations;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    public event Action<Vector3, Quaternion> Move;
    public event Action GatherEnded;
    public event Action DyingEnded;

    private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Gather = Animator.StringToHash("Gather");
    private static readonly int Dying = Animator.StringToHash("Dying");

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

    public void SetGather()
    {
        _animator.SetTrigger(Gather);
    }

    public void SetDying()
    {
        _animator.SetTrigger(Dying);
    }

    private void OnAnimatorMove()
    {
        Move?.Invoke(_animator.deltaPosition, _animator.rootRotation);
    }

    [UsedImplicitly]
    public void GatherEnd()
    {
        GatherEnded?.Invoke();
        Debug.Log("Gather ended");
    }

    [UsedImplicitly]
    public void DyingEnd()
    {
        DyingEnded?.Invoke();
        Debug.Log("Dying ended");
    }

    [UsedImplicitly]
    private void FootL() { }

    [UsedImplicitly]
    private void FootR() { }


}
