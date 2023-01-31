using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerService : Service
{
    private CollectionService _collectionService;
    private AssetsCollection _assetsCollection;

    private void Start()
    {
        _collectionService = Services.Get<CollectionService>();
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public Mushroom SpawnMushroom(Transform t, float radius)
    {
        Vector2 pos = Random.insideUnitCircle * radius;
        Mushroom mushroom = Instantiate(_assetsCollection.MushroomPrefab, t);
        mushroom.transform.localPosition = new Vector3(pos.x, 0, pos.y);
        _collectionService.AddMushroom(mushroom);

        return mushroom;
    }

    public void DespawnMushroom(Mushroom mushroom)
    {
        _collectionService.RemoveMushroom(mushroom);
        Destroy(mushroom.gameObject);
    }

    public MushroomArea SpawnMushroomArea(Vector3 position)
    {
        MushroomArea area = Instantiate(_assetsCollection.MushroomAreaPrefab);
        area.transform.position = position;

        return area;
    }
}
