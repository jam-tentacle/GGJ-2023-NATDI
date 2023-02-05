using UnityEngine;

public class MushroomControls : Service, IUpdate, IStart, IInject
{
    private CameraController _cameraController;
    private ShootLine _shootLine;
    private AssetsCollection _assetsCollection;
    private MyceliumVisualizer _myceliumVisualizer;
    private SpawnerService _spawnerService;
    private TerrainService _terrainService;
    private UIService _uiService;
    private TerrainLayerType _layerType;

    private float _leftReloadTime;
    private CollectionService _collection;
    private MushroomArea _currentArea;

    public void Inject()
    {
        _cameraController = Services.Get<CameraController>();
        _shootLine = Services.Get<ShootLine>();
        _assetsCollection = Services.Get<AssetsCollection>();
        _spawnerService = Services.Get<SpawnerService>();
        _uiService = Services.Get<UIService>();
        _collection = Services.Get<CollectionService>();
        _terrainService = Services.Get<TerrainService>();
    }

    public void GameStart()
    {
        _myceliumVisualizer = new MyceliumVisualizer(transform);
        MushroomArea area = FindObjectOfType<MushroomArea>();
        area.CachedTerrainLayerType = _terrainService.GetTerrainLayerType(area.Position);
        _layerType = area.CachedTerrainLayerType;
        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _currentArea = area;
    }

    public void GameUpdate(float delta)
    {
        UpdateReloadTime(delta);
        UpdateMushrooms(delta);

        if (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (_leftReloadTime <= 0)
        {
            Shoot();
        }
        else
        {
#if DEBUG
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Shoot();
            }
#endif
        }
    }

    private void UpdateMushrooms(float delta)
    {
        foreach (var mushroom in _collection.Mushrooms)
        {
            mushroom.GameUpdate(delta);
        }
    }

    private void UpdateReloadTime(float delta)
    {
        _leftReloadTime -= delta;

        _uiService.ReloadShootUI.UpdateView(_leftReloadTime, _assetsCollection.Settings.MushroomCreatorReloadTime);
    }

    private void Shoot()
    {
        if (_layerType == TerrainLayerType.Sand)
        {
            Vector3 targetPosition = _shootLine.GetEndPosition();
            SpawnMushroomArea(_terrainService.TryGetTerrainPosition(targetPosition));
        }
        else
        {
            Vector3 targetPosition = _shootLine.GetEndPosition();
            targetPosition.y = 0;

            Projectile projectile = Instantiate(_assetsCollection.SporePrefab);
            Vector3 position = _cameraController.Target.Position;
            position.y += 0.5f;
            projectile.transform.position = position;
            projectile.Hit += OnProjectileHit;
            projectile.Launch(targetPosition);
            _cameraController.SetFollowTarget(projectile);
            _leftReloadTime = _assetsCollection.Settings.MushroomCreatorReloadTime;
        }
    }

    private void OnProjectileHit(Vector3 position, Projectile projectile)
    {
        _cameraController.RemoveFollowTarget(projectile);
        SpawnMushroomArea(position);
    }

    private void SpawnMushroomArea(Vector3 position)
    {
        TerrainLayerType terrain = GetTerrainByPosition(position);
        if (!_spawnerService.TrySpawnMushroomArea(position, terrain, out var area))
        {
            Debug.Log($"no mushroom area spawned. terrain {terrain} is banned");
            return;
        }

        if (_layerType == TerrainLayerType.Sand)
        {
            _layerType = TerrainLayerType.Rock;
            _shootLine.Mode = ShootLine.AimingMode.Default;
            _myceliumVisualizer.DrawLineWithSpikes(_currentArea.Position, area.Position, _currentArea, area);
        }
        else
        {
            _myceliumVisualizer.DrawLine(_currentArea.Position, area.Position);
            if (terrain == TerrainLayerType.Sand)
            {
                _layerType = terrain;
                _shootLine.Mode = ShootLine.AimingMode.Sand;
            }
            else
            {
                _layerType = TerrainLayerType.Rock;
            }
        }

        _cameraController.SetTarget(area);
        _shootLine.SetTarget(area);
        _currentArea = area;
    }

    private TerrainLayerType GetTerrainByPosition(Vector3 position)
    {
        return Services.Get<TerrainService>().GetTerrainLayerType(position);
    }

    public void SetForceReload()
    {
        _leftReloadTime = Mathf.Min(_leftReloadTime, _assetsCollection.Settings.ForceMushroomCreatorReloadTime);
    }
}
