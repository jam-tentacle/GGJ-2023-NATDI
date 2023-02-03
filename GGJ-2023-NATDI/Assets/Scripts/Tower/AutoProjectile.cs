using System;
using UnityEngine;

public class AutoProjectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve _heightCurve;
    [SerializeField] private float _velocity = 1f;

    private Vector3 _startPoint;

    private ITarget _to;

    private Vector3 _lastEndPoint;

    private float _currentTime;

    private float _totalTime;

    public void Fire(Vector3 startPosition, ITarget to)
    {
        _startPoint = startPosition;
        _to = to;

        _totalTime = Vector3.Distance(startPosition, to.ShootTargetPosition) / _velocity;

        transform.position = _startPoint;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_to.IsAlive)
        {
            _lastEndPoint = _to.ShootTargetPosition;

            var newPosition = Vector3.Lerp(_startPoint, _lastEndPoint, Mathf.Clamp01(_currentTime / _totalTime));

            transform.position = GetNewPositionWithY(newPosition);
        }
        else
        {
            var newPosition = Vector3.Lerp(_startPoint, _lastEndPoint, _currentTime / _totalTime);

            transform.position = GetNewPositionWithY(newPosition);

            if (_currentTime > _totalTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public Vector3 GetNewPositionWithY(Vector3 position)
    {
        position.y += _heightCurve.Evaluate(_currentTime / _totalTime);
        return position;
    }
}
