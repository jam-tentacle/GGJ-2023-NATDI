using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class MushroomArea : MonoBehaviour, ITarget
{
    [SerializeField] private Transform _cylinder;
    [SerializeField] private float _radius = 2;
    [MinMaxSlider(1, 20)] [SerializeField] private Vector2Int _count = new(1, 1);
    [SerializeField] private float _respawnTime = 10f;
    [SerializeField] private ChooseMushroomAreaTarget _chooseMushroomAreaTarget;
    public ChooseMushroomAreaTarget ChooseMushroomAreaTarget => _chooseMushroomAreaTarget;

    private MyceliumVisualizer _myceliumVisualizer;
    private SpawnerService _spawnerService;

    public Vector3 Position => transform.position;
    public Vector3 ShootTargetPosition => transform.position;
    public Vector3 Velocity => Vector3.zero;
    public bool IsAlive => this != null;

    private float _currentRespawnTime;

    private LinkedList<Mushroom> _mushrooms = new LinkedList<Mushroom>();

    private void Start()
    {
        _spawnerService = Services.Get<SpawnerService>();
        _myceliumVisualizer = new MyceliumVisualizer(transform);
        UpdateCylinderScale();

        for (int i = 0; i < _count.x; i++)
        {
            SpawnMushroom();
        }
    }

    private void SpawnMushroom()
    {
        Mushroom mushroom = _spawnerService.SpawnMushroom(transform, _radius);
        _mushrooms.AddLast(mushroom);
        _myceliumVisualizer.Add(mushroom.transform.localPosition);
    }

    private void Update()
    {
        TryClear();

        TryUpdateRespawn(Time.deltaTime);
    }

    private void TryClear()
    {
        var currentNode = _mushrooms.First;
        while (currentNode != null)
        {
            var next = currentNode.Next;
            if (currentNode.Value == null)
            {
                _mushrooms.Remove(currentNode);
            }

            currentNode = next;
        }
    }

    private void TryUpdateRespawn(float delta)
    {
        if (_count.y <= _mushrooms.Count)
        {
            _currentRespawnTime = 0f;
            return;
        }

        _currentRespawnTime += delta;

        if (_respawnTime <= _currentRespawnTime)
        {
            _currentRespawnTime = 0f;
            SpawnMushroom();
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
