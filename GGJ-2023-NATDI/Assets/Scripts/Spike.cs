using DG.Tweening;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
        transform.position = _startPos + Vector3.down * 5f;
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    public void Enable()
    {
        transform.DOMove(_startPos, 0.5f);
    }

    public void Disable()
    {
        transform.DOMove(_startPos + Vector3.down * 5f, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable receiver = other.gameObject.GetComponent<Damageable>();
        if (receiver == null) return;

        receiver.ReceiveHit(Services.Get<AssetsCollection>().Settings.SpikeDamage, Vector3.zero);
        other.transform.gameObject.GetComponent<CharacterFX>().SpikeEffect();
    }
}
