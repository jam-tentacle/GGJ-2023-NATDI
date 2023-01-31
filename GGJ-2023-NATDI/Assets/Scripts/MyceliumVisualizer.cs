using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyceliumVisualizer : Service
{
    [SerializeField] private LineRenderer _linePrefab;

    private List<Vector3> _points = new();

    public void Add(ITarget target)
    {
        if (_points.Count > 0)
        {
            foreach (Vector3 point in _points)
            {
                LineRenderer line = Instantiate(_linePrefab, transform);
                line.SetPosition(0, target.Position);
                line.SetPosition(1, point);
            }
        }

        _points.Add(target.Position);
    }
}
