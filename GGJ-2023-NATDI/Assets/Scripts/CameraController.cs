using UnityEngine;

public class CameraController : Service, ILateUpdate
{
    [SerializeField] private float _distance = 2f;
    [SerializeField] private float _xSpeed = 150f;
    [SerializeField] private float _ySpeed = 150f;
    [SerializeField] private float _yMinLimit = -90f;
    [SerializeField] private float _yMaxLimit = 90f;
    [SerializeField] private float _smoothTime = 2f;

    private float _rotationYAxis;
    private float _rotationXAxis;
    private float _velocityX;
    private float _velocityY;

    private ITarget _target;
    public ITarget Target => _target;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _rotationYAxis = angles.y;
        _rotationXAxis = angles.x;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    public void GameLateUpdate(float delta)
    {
        if (_target == null)
        {
            return;
        }

        transform.position = _target.Position;
        _velocityX += _xSpeed * Input.GetAxis("Mouse X") * _distance * 0.02f;
        _velocityY += _ySpeed * Input.GetAxis("Mouse Y") * 0.02f;

        _rotationYAxis += _velocityX;
        _rotationXAxis -= _velocityY;
        _rotationXAxis = ClampAngle(_rotationXAxis, _yMinLimit, _yMaxLimit);
        Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);

        transform.rotation = toRotation;
        _velocityX = Mathf.Lerp(_velocityX, 0, delta * _smoothTime);
        _velocityY = Mathf.Lerp(_velocityY, 0, delta * _smoothTime);
        _velocityX = 0;
        _velocityY = 0;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }
}
