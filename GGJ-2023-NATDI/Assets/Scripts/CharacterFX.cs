using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private AssetsCollection _assetsCollection;

    // Start is called before the first frame update
    void Start()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    [Button]
    public void BloodEffect()
    {
        ParticleSystem _particle = Instantiate(_assetsCollection.BloodEffect);
        _particle.transform.position = transform.position;
        Destroy(_particle.gameObject, 3f);
    }

    [Button]
    public void SpikeEffect()
    {
        ParticleSystem _particle = Instantiate(_assetsCollection.SpikeEffect);
        _particle.transform.position = transform.position + Vector3.up;
        Destroy(_particle.gameObject, 3f);
    }
}
