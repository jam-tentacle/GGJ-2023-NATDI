using System.Collections.Generic;
using UnityEngine;

public class SpikeLine : MonoBehaviour
{
    [SerializeField] private Spike _spikePrefab;
    private TerrainService _terrainService;
    private List<Spike> _spikes = new();
    private MushroomArea _area1;
    private MushroomArea _area2;
    private bool _hasMushrooms;

    private TerrainService TerrainService => _terrainService ?? Services.Get<TerrainService>();

    public void Init(Vector3 a, Vector3 b)
    {
        int points = (int)Vector3.Distance(a, b);
        points /= 3;

        for (int i = 0; i < points; i++)
        {
            float c = i / (float)points;
            Vector3 pos = Vector3.Lerp(a, b, c);

            if (TerrainService.RayCastOnTerrain(pos, out RaycastHit hit))
            {
                pos = hit.point;
            }

            Spike spike = Instantiate(_spikePrefab);
            spike.transform.position = pos;
            spike.transform.SetParent(transform, true);
            _spikes.Add(spike);
        }
    }

    public void SetAreas(MushroomArea area1, MushroomArea area2)
    {
        _area1 = area1;
        _area2 = area2;
    }

    private void FixedUpdate()
    {
        if (_hasMushrooms == _area1.HasMushrooms && _area2.HasMushrooms) return;

        _hasMushrooms = !_hasMushrooms;

        foreach (Spike spike in _spikes)
        {
            if (_hasMushrooms)
            {
                spike.Enable();
            }
            else
            {
                spike.Disable();
            }

        }
    }
}
