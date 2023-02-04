using UnityEngine;

public class TerrainService : Service, IInject
{
    private Terrain _terrain;
    private Painter _painter;

    public void Inject()
    {
        _terrain = FindObjectOfType<Terrain>();
        _painter = _terrain.GetComponent<Painter>();
    }

    public void Modify(Vector3 position) => _painter.Modify(position);

    public TerrainLayerType GetTerrainLayerType(Vector3 position) => _painter.GetTerrainLayerType(position);
}
