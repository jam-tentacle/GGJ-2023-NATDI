using System;
using UnityEngine;

public class ShootLine : Service, ILateUpdate
{
    [SerializeField] private float _range = 10;
    [SerializeField] private LineRenderer _lineRenderer;
    private CameraController _cameraController;

    private void Start()
    {
        _cameraController = Services.Get<CameraController>();
    }

    public void GameLateUpdate(float delta)
    {
        Vector3 start = Vector3.zero;

        Vector3 forward = _cameraController.transform.forward;
        Vector3 end = new Vector3(forward.x, 0, forward.z) * _range;

        _lineRenderer.positionCount = 5;
        for (int i = 0; i < 5; i++)
        {
            _lineRenderer.SetPosition(i, Parabola(start, end, 2, i / 5f));
        }
    }

    public Vector3 GetEndPosition()
    {
        Vector3 forward = _cameraController.transform.forward;
        return transform.position + new Vector3(forward.x, 0, forward.z) * _range;
    }

    public void SetTarget(ITarget target)
    {
        Vector3 pos = target.Position;
        pos.y += 0.1f;
        transform.position = pos;
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float Func(float x) => -4 * height * x * x + 4 * height * x;

        Vector3 mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
