using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MushroomArea : MonoBehaviour, ITarget
{
    [SerializeField] private float _radius = 2;
    [MinMaxSlider(1, 10)] [SerializeField] private Vector2Int _count = new(1, 1);

    private AssetsCollection _assetsCollection;
    private MyceliumVisualizer _myceliumVisualizer;

    public Vector3 Position => transform.position;

    private void Start()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _myceliumVisualizer = new MyceliumVisualizer(transform);

        int count = Random.Range(_count.x, _count.y);
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = Random.insideUnitCircle * _radius;
            Mushroom mushroom = Instantiate(_assetsCollection.MushroomPrefab, transform);
            mushroom.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            _myceliumVisualizer.Add(mushroom.transform.localPosition);
        }
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _radius);
    }
}
