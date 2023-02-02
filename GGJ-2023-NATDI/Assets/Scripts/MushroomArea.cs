using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MushroomArea : MonoBehaviour, ITarget
{
    [SerializeField] private Transform _cylinder;
    [SerializeField] private float _radius = 2;
    [MinMaxSlider(1, 10)] [SerializeField] private Vector2Int _count = new(1, 1);

    private MyceliumVisualizer _myceliumVisualizer;
    private SpawnerService _spawnerService;

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => transform.position;
    public Vector3 Velocity => Vector3.zero;

    private void Start()
    {
        _spawnerService = Services.Get<SpawnerService>();
        _myceliumVisualizer = new MyceliumVisualizer(transform);
        UpdateCylinderScale();

        int count = Random.Range(_count.x, _count.y);
        for (int i = 0; i < count; i++)
        {
            Mushroom mushroom = _spawnerService.SpawnMushroom(transform, _radius);
            _myceliumVisualizer.Add(mushroom.transform.localPosition);
        }
    }

    private void UpdateCylinderScale()
    {
        if (_cylinder == null)
        {
            return;
        }

        _cylinder.localScale = new Vector3(_radius * 2, 0.1f, _radius * 2);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _radius);
    }

    private void OnValidate() => UpdateCylinderScale();
}
