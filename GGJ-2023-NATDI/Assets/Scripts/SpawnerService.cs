using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerService : Service, IInject
{
    private const int Ground = 1 << 6;
    private CollectionService _collectionService;
    private AssetsCollection _assetsCollection;

    public void Inject()
    {
        _collectionService = Services.Get<CollectionService>();
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public Mushroom SpawnMushroom(Transform t, float radius, TerrainLayerType terrain)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        Vector3 localPos = new(randomPos.x, 0, randomPos.y);
        Mushroom mushroom = Instantiate(Services.Get<AssetsCollection>().GetMushroomByTerrain(terrain), t);
        if (RayCastOnTerrain(t.position + localPos, out RaycastHit hit))
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
        _collectionService.RemoveMushroom(mushroom);
        Destroy(mushroom.gameObject);
    }

    public void DespawnMushroomer(EnemyMovementAi value)
    {
        _collectionService.RemoveMushroomer(value);
        Destroy(value.gameObject);
    }

    public MushroomArea SpawnMushroomArea(Vector3 position, TerrainLayerType terrain)
    {
        var areaPrefab = Services.Get<AssetsCollection>().GetMushroomAreaByTerrain(terrain);
        MushroomArea area = Instantiate(areaPrefab);
        area.CachedTerrainLayerType = terrain;
        Services.Get<CollectionService>().MushroomAreas.Add(area);
        area.transform.position = position;

        return area;
    }

    private bool RayCastOnTerrain(Vector3 position, out RaycastHit hit) => Physics.Raycast(position + Vector3.up * 1000,
        Vector3.down,
        out hit,
        Mathf.Infinity,
        Ground);
}
