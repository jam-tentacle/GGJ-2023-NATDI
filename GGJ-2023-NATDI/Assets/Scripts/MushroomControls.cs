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

        Shoot();
    }

    private void Shoot()
    {
        Vector3 targetPosition = _shootLine.GetEndPosition();
        targetPosition.y = 0;

        Projectile projectile = Instantiate(_assetsCollection.SporePrefab);
        Vector3 position = _cameraController.Target.Position;
        position.y = 0.5f;
        projectile.transform.position = position;
        projectile.Hit += OnProjectileHit;
        projectile.Launch(targetPosition);
    }

    private void OnProjectileHit(Vector3 position)
    {
        position.y = 0;

        SpawnMushroomArea(position);
    }

    private void SpawnMushroomArea(Vector3 position)
    {
        MushroomArea area = Instantiate(_assetsCollection.MushroomAreaPrefab);
        area.transform.position = position;

        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _myceliumVisualizer.Add(area.Position);
    }
}
