using System.Collections.Generic;
using UnityEngine;

public class MyceliumVisualizer
{
    private List<Vector3> _points = new();
    private AssetsCollection _assetsCollection;
    private TerrainService _terrainService;
    private Transform _transform;

    public MyceliumVisualizer(Transform transform)
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _terrainService = Services.Get<TerrainService>();
        _transform = transform;
    }

    public void Add(Vector3 position)
    {
        if (_points.Count > 0)
        {
            foreach (Vector3 point in _points)
            {
                DrawLine(position, point);
            }
        }

        _points.Add(position);
    }

    public void DrawLine(Vector3 a, Vector3 b)
    {
        LineRenderer line = Object.Instantiate(_assetsCollection.LinePrefab, _transform);
        int points = (int)Vector3.Distance(a, b);
        points *= 2;
        line.positionCount = points;

        for (int i = 0; i < points; i++)
        {
            float c = i / (float)points;
            Vector3 pos = Vector3.Lerp(a, b, c);

            if (_terrainService.RayCastOnTerrain(pos, out RaycastHit hit))
            {
                pos = hit.point;
            }

            pos.y += 0.4f;

            line.SetPosition(i, pos);
        }
    }

    public void DrawLineWithSpikes(Vector3 a, Vector3 b, MushroomArea area1, MushroomArea area2)
    {
        SpikeLine line = Object.Instantiate(_assetsCollection.SpikeLinePrefab);
        line.Init(a, b);
        line.SetAreas(area1, area2);
    }
}
