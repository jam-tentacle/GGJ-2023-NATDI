using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mushroom : MonoBehaviour, ITarget, IUpdate
{
    [SerializeField] private Transform _leg;
    //[SerializeField] private Transform _cap;
    [SerializeField] private Rigidbody _rb;
    private IUpdate[] _updatables;

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => transform.position;
    public Vector3 Velocity => _rb.velocity;
    public bool IsAlive => this != null;

    private void Awake()
    {
        _updatables = GetComponentsInChildren<MonoBehaviour>().OfType<IUpdate>().Where(i=>i is not Mushroom).ToArray();
    }

    private void Start()
    {
        StartGrowing();
    }

    private void StartGrowing()
    {
        Vector3 endLegScale = _leg.localScale;
        // Vector3 endCapScale = _cap.localScale;

        _leg.localScale = Vector3.zero;
        // _cap.localScale = Vector3.zero;

        DOTween.Sequence()
            .Append(_leg.DOScale(endLegScale, 0.3f));

        // .Append(_cap.DOScale(endCapScale, 0.3f));
    }

    public void GameUpdate(float delta)
    {
        foreach (IUpdate updatable in _updatables)
        {
            updatable.GameUpdate(delta);
        }
    }
}
