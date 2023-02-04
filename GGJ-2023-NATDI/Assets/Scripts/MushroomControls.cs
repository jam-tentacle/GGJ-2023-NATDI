using UnityEngine;

public class MushroomControls : Service, IUpdate, IStart, IInject
{
    [SerializeField] private int _maxMushroomAreas = 50;
    private CameraController _cameraController;
    private ShootLine _shootLine;
    private AssetsCollection _assetsCollection;
    private MyceliumVisualizer _myceliumVisualizer;
    private SpawnerService _spawnerService;
    private UIService _uiService;

    private float _leftReloadTime;

    public void Inject()
    {
        _cameraController = Services.Get<CameraController>();
        _shootLine = Services.Get<ShootLine>();
        _assetsCollection = Services.Get<AssetsCollection>();
        _spawnerService = Services.Get<SpawnerService>();
        _uiService = Services.Get<UIService>();
    }

    public void GameStart()
    {
        _myceliumVisualizer = new MyceliumVisualizer(transform);
        MushroomArea area = FindObjectOfType<MushroomArea>();
        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _myceliumVisualizer.Add(area.Position);
    }

    public void GameUpdate(float delta)
    {
        UpdateReloadTime(delta);

        if (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space)) return;

        if (_leftReloadTime <= 0)
        {
            Shoot();
        }
    }

    private void UpdateReloadTime(float delta)
    {
        _leftReloadTime -= delta;

        _uiService.ReloadShootUI.UpdateView(_leftReloadTime, _assetsCollection.Settings.MushroomCreatorReloadTime);
    }

    private void Shoot()
    {
        if (Services.Get<CollectionService>().GetAreaCount >= _maxMushroomAreas)
        {
            return;
        }
        Vector3 targetPosition = _shootLine.GetEndPosition();
        targetPosition.y = 0;

        Projectile projectile = Instantiate(_assetsCollection.SporePrefab);
        Vector3 position = _cameraController.Target.Position;
        position.y += 0.5f;
        projectile.transform.position = position;
        projectile.Hit += OnProjectileHit;
        projectile.Launch(targetPosition);

        _leftReloadTime = _assetsCollection.Settings.MushroomCreatorReloadTime;
    }

    private void OnProjectileHit(Vector3 position)
    {
        SpawnMushroomArea(position);
    }

    private void SpawnMushroomArea(Vector3 position)
    {
        var terrain = GetTerrainByPosition(position);
        MushroomArea area = _spawnerService.SpawnMushroomArea(position, terrain);

        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _myceliumVisualizer.Add(area.Position);
    }

    private TerrainLayerType GetTerrainByPosition(Vector3 position)
    {
        return Services.Get<TerrainService>().GetTerrainLayerType(position);
    }
}
