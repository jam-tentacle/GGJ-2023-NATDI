using System;
using UnityEngine;

public class ShootLine : Service, ILateUpdate, IInject
{
    public enum AimingMode
    {
        Default,
        Sand,
    }

    [SerializeField] private float _range = 10;
    [SerializeField] private float _sandAimingRange = 21;
    [SerializeField] private LineRenderer _lineRenderer;
    private CameraController _cameraController;
    public AimingMode Mode { get; set; } = AimingMode.Default;
    private TerrainService _terrainService;

    public void Inject()
    {
        _terrainService = Services.Get<TerrainService>();
        _cameraController = Services.Get<CameraController>();
    }

    public void GameLateUpdate(float delta)
    {
        switch (Mode)
        {
            case AimingMode.Default:
                DefaultAimingUpdate();
                break;
            case AimingMode.Sand:
                SandAimingUpdate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private void DefaultAimingUpdate()
    {
        _lineRenderer.useWorldSpace = false;

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
        if (Mode == AimingMode.Sand)
        {
            Vector3 forward = _cameraController.transform.forward;
            return transform.position + new Vector3(forward.x, 0, forward.z) * _sandAimingRange;
        }
        else
        {
            Vector3 forward = _cameraController.transform.forward;
            return transform.position + new Vector3(forward.x, 0, forward.z) * _range;
        }
    }

    public void SetTarget(ITarget target)
    {
        Vector3 pos = target.Position;
        pos.y += 0.1f;
        transform.position = pos;
    }

    private static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float Func(float x) => -4 * height * x * x + 4 * height * x;

        Vector3 mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    private void SandAimingUpdate()
    {
        Vector3 forward = _cameraController.transform.forward;
        Vector3 center = _cameraController.transform.position + new Vector3(forward.x, 0, forward.z) * _sandAimingRange;

        _lineRenderer.positionCount = 4;
        _lineRenderer.useWorldSpace = true;

        Vector3 a = center + Vector3.left + Vector3.forward;
        Vector3 b = center + Vector3.left - Vector3.forward;
        Vector3 c = center - Vector3.left + Vector3.forward;
        Vector3 d = center - Vector3.left - Vector3.forward;
        _lineRenderer.SetPosition(0, _terrainService.TryGetTerrainPosition(a) + Vector3.up * 0.5f);
        _lineRenderer.SetPosition(1, _terrainService.TryGetTerrainPosition(b) + Vector3.up * 0.5f);
        _lineRenderer.SetPosition(2, _terrainService.TryGetTerrainPosition(c) + Vector3.up * 0.5f);
        _lineRenderer.SetPosition(3, _terrainService.TryGetTerrainPosition(d) + Vector3.up * 0.5f);
    }
}
