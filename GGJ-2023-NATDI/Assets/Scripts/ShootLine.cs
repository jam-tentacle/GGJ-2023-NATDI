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
        Vector3 forward = _cameraController.transform.forward;
        _lineRenderer.SetPosition(0, Vector3.zero);
        Vector3 pos1 = new Vector3(forward.x, 0, forward.z) * _range;
        _lineRenderer.SetPosition(1, pos1);
    }

    public Vector3 GetEndPosition()
    {
        Vector3 forward = _cameraController.transform.forward;
        return transform.position + new Vector3(forward.x, 0, forward.z) * _range;
    }

    public void SetTarget(ITarget target)
    {
        Vector3 pos = target.Position;
        pos.y = 0.1f;
        transform.position = pos;
    }
}
