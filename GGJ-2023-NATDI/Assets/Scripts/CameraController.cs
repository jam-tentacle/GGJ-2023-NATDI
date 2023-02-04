using NATDI;
using UnityEngine;

public class CameraController : Service, ILateUpdate
{
    [SerializeField] private float _distance = 2f;
    [SerializeField] private float _xSpeed = 150f;
    [SerializeField] private float _ySpeed = 150f;
    [SerializeField] private float _yMinLimit = -90f;
    [SerializeField] private float _yMaxLimit = 90f;
    [SerializeField] private float _smoothTime = 2f;
    [SerializeField] private float _smoothFollowSpeed = 3f;
    [SerializeField] private float _followFovCoeff = 0.7f;
    [SerializeField] private float _followRotation = 33f;

    private float _originalFOV;
    private Vector3 _originalRotation;
    private float _rotationYAxis;
    private float _rotationXAxis;
    private float _velocityX;
    private float _velocityY;

    private ITarget _target;
    private ITarget _followTarget;
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

        _originalFOV = Camera.main.fieldOfView;
        _originalRotation = Camera.main.transform.localEulerAngles;
    }

    public void GameLateUpdate(float delta)
    {
        if (_followTarget is not null)
        {
            SmoothFollow(_followTarget);
            return;
        }

        if (_target == null)
        {
            return;
        }

        Move(delta);
    }

    private void Move(float delta)
    {
        transform.position = _target.Position;
        _velocityX += _xSpeed * Input.GetAxis("Mouse X") * _distance * 0.02f;
        _velocityY += _ySpeed * Input.GetAxis("Mouse Y") * 0.02f;

        _rotationYAxis += _velocityX;
        _rotationXAxis = ClampAngle(_rotationXAxis, _yMinLimit, _yMaxLimit);
        Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);

        transform.rotation = toRotation;
        _velocityX = Mathf.Lerp(_velocityX, 0, delta * _smoothTime);
        _velocityY = Mathf.Lerp(_velocityY, 0, delta * _smoothTime);
        _velocityX = 0;
        _velocityY = 0;
    }

    private void SmoothFollow(ITarget target)
    {
        Camera.main.fieldOfView = _originalFOV * _followFovCoeff;
        Vector3 targetPos = target.Position;
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
            targetPos, _smoothFollowSpeed);

        transform.position = smoothFollow;
        transform.LookAt(target);
        Camera.main.transform.localEulerAngles = new Vector3(_followRotation,
            _originalRotation.y,
            _originalRotation.z);
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

    public void RemoveFollowTarget(ITarget target)
    {
        if (_followTarget != target)
        {
            return;
        }

        _followTarget = null;
        Camera.main.fieldOfView = _originalFOV;
        Camera.main.transform.localEulerAngles = _originalRotation;
    }

    public void SetFollowTarget(ITarget target)
    {
        _followTarget = target;
    }

}
