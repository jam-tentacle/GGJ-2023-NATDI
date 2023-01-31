using System.Collections.Generic;
using UnityEngine;

public class MyceliumVisualizer
{
    private List<Vector3> _points = new();
    private AssetsCollection _assetsCollection;
    private Transform _transform;

    public MyceliumVisualizer(Transform transform)
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _transform = transform;
    }

    public void Add(Vector3 position)
    {
        if (_points.Count > 0)
        {
            foreach (Vector3 point in _points)
            {
                LineRenderer line = Object.Instantiate(_assetsCollection.LinePrefab, _transform);
                line.SetPosition(0, position);
                line.SetPosition(1, point);
            }
        }

        _points.Add(position);
    }
}
