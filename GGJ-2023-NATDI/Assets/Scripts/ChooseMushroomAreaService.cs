using UnityEngine;

public class ChooseMushroomAreaService : Service, IUpdate
{
    [SerializeField] private LayerMask _chooseMushroomUIMask;

    private RaycastHit[] _hits = new RaycastHit[2];

    private CameraController _cameraController;

    private Camera _camera;
    private static Vector3 ScreenCenter = new Vector3(0.5f, 0.5f, 0f);

    float _rayLength = 500f;

    private ChooseMushroomAreaTarget _target;

    private void Start()
    {
        _camera = Camera.main;
        _cameraController = Services.Get<CameraController>();
    }

    public void GameUpdate(float delta)
    {
        UpdateChooseMushroomAreaTarget();

        TryChangeTargetForCamera();
    }

    private void UpdateChooseMushroomAreaTarget()
    {
        var ray = _camera.ViewportPointToRay(ScreenCenter);

        var count = Physics.RaycastNonAlloc(ray, _hits, _rayLength, _chooseMushroomUIMask);

        if (count <= 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var target = _hits[i].collider.GetComponent<ChooseMushroomAreaTarget>();

            if (_target == target)
            {
                continue;
            }

            if (_cameraController.Target is MushroomArea area && area.ChooseMushroomAreaTarget == target)
            {
                continue;
            }

            if (_target != null)
            {
                _target.StopHighlight();
            }

            _target = target;
            _target.StartHighlight();
        }
    }

    private void TryChangeTargetForCamera()
    {
        if (_target == null)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(1))
        {
            return;
        }

        if (_cameraController.Target is MushroomArea area && area.ChooseMushroomAreaTarget == _target)
        {
            return;
        }

        _cameraController.SetTarget(_target.MushroomArea);
        _target.StopHighlight();
        _target = null;
    }
}
