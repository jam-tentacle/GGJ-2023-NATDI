using UnityEngine;

public class MushroomControls : Service, IUpdate
{
    [SerializeField] private Mushroom _mushroomPrefab;
    private CameraController _cameraController;
    private ShootLine _shootLine;
    private MyceliumVisualizer _myceliumVisualizer;

    private void Start()
    {
        _cameraController = Services.Get<CameraController>();
        _shootLine = Services.Get<ShootLine>();
        _myceliumVisualizer = Services.Get<MyceliumVisualizer>();

        Mushroom mushroom = FindObjectOfType<Mushroom>();
        _cameraController.SetTarget(mushroom);
        _shootLine.SetTarget(mushroom);
        _myceliumVisualizer.Add(mushroom);
    }

    public void GameUpdate(float delta)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Mushroom mushroom = Instantiate(_mushroomPrefab);
            mushroom.transform.position = _shootLine.GetEndPosition();

            _cameraController.SetTarget(mushroom);
            _shootLine.SetTarget(mushroom);
            _myceliumVisualizer.Add(mushroom);
        }
    }
}
