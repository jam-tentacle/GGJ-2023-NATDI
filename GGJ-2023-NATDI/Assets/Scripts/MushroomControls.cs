using UnityEngine;

public class MushroomControls : Service, IUpdate
{
    private CameraController _cameraController;
    private ShootLine _shootLine;
    private AssetsCollection _assetsCollection;
    private MyceliumVisualizer _myceliumVisualizer;

    private void Start()
    {
        _cameraController = Services.Get<CameraController>();
        _shootLine = Services.Get<ShootLine>();
        _assetsCollection = Services.Get<AssetsCollection>();
        _myceliumVisualizer = new MyceliumVisualizer(transform);

        MushroomArea area = FindObjectOfType<MushroomArea>();
        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _myceliumVisualizer.Add(area.Position);
    }

    public void GameUpdate(float delta)
    {
        if (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space)) return;

        MushroomArea area = Instantiate(_assetsCollection.MushroomAreaPrefab);
        Vector3 position = _shootLine.GetEndPosition();
        position.y = 0;
        area.transform.position = position;

        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _myceliumVisualizer.Add(area.Position);
    }
}
