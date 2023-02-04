using UnityEngine;

public class TerrainService : Service, IInject
{
    private const int Ground = 1 << 6;

    private Terrain _terrain;
    private Painter _painter;

    public void Inject()
    {
        _terrain = FindObjectOfType<Terrain>();
        _painter = _terrain.GetComponent<Painter>();
    }

    public void Modify(Vector3 position) => _painter.Modify(position);

    public TerrainLayerType GetTerrainLayerType(Vector3 position) => _painter.GetTerrainLayerType(position);

    public bool RayCastOnTerrain(Vector3 position, out RaycastHit hit) => Physics.Raycast(position + Vector3.up * 1000,
        Vector3.down,
        out hit,
        Mathf.Infinity,
        Ground);

    public Vector3 TryGetTerrainPosition(Vector3 position) =>
        RayCastOnTerrain(position, out RaycastHit hit) ? hit.point : position;
}
