using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerService : Service, IInject
{
    private CollectionService _collectionService;
    private TerrainService _terrainService;

    public void Inject()
    {
        _collectionService = Services.Get<CollectionService>();
        _terrainService = Services.Get<TerrainService>();
    }

    public Mushroom SpawnMushroom(Transform t, float radius, TerrainLayerType terrain)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        Vector3 localPos = new(randomPos.x, 0, randomPos.y);
        Mushroom mushroom = Instantiate(Services.Get<AssetsCollection>().GetMushroomByTerrain(terrain), t);
        if (_terrainService.RayCastOnTerrain(t.position + localPos, out RaycastHit hit))
        {
            mushroom.transform.position = hit.point;
        }
        else
        {
            mushroom.transform.localPosition = localPos;
        }

        _collectionService.AddMushroom(mushroom);
        return mushroom;
    }

    public void DespawnMushroom(Mushroom mushroom)
    {
        if (!mushroom.IsAlive)
        {
           Debug.LogError("mushroom already destroyed");
           return;
        }
        _collectionService.RemoveMushroom(mushroom);
        Destroy(mushroom.gameObject);
    }

    public void DespawnMushroomer(EnemyMovementAi value)
    {
        _collectionService.RemoveMushroomer(value);
        Destroy(value.gameObject);
    }

    public bool TrySpawnMushroomArea(Vector3 position, TerrainLayerType terrain, out MushroomArea area)
    {
        var areaPrefab = Services.Get<AssetsCollection>().GetMushroomAreaByTerrain(terrain);
        if (areaPrefab is null)
        {
            area = null;
            return false;
        }
        area = Instantiate(areaPrefab);
        area.CachedTerrainLayerType = terrain;
        Services.Get<CollectionService>().AddMushroomArea(area);
        area.transform.position = position;
        return true;
    }
}
