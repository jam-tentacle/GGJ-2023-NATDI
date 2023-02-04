using UnityEngine;

public class Spike : MonoBehaviour
{
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable receiver = other.gameObject.GetComponent<Damageable>();
        if (receiver == null) return;

        receiver.ReceiveHit(Services.Get<AssetsCollection>().Settings.SpikeDamage, Vector3.zero);
        other.transform.gameObject.GetComponent<CharacterFX>().SpikeEffect();
    }
}