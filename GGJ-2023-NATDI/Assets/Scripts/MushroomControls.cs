using UnityEngine;

public class MushroomControls : Service, IUpdate
{
    [SerializeField] private Mushroom _mushroomPrefab;
    private CameraController _cameraController;
    private ShootLine _shootLine;

    private void Start()
    {
        _cameraController = Services.Get<CameraController>();
        _shootLine = Services.Get<ShootLine>();

        Mushroom mushroom = FindObjectOfType<Mushroom>();
        _cameraController.SetTarget(mushroom);
        _shootLine.SetTarget(mushroom);
    }

    public void GameUpdate(float delta)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Mushroom mushroom = Instantiate(_mushroomPrefab);
            mushroom.transform.position = _shootLine.GetEndPosition();

            _cameraController.SetTarget(mushroom);
            _shootLine.SetTarget(mushroom);
        }
    }
}
