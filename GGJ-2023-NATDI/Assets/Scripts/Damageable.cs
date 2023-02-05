using UnityEngine;

public delegate void HealthChangeDelegate(float deltaHealth, Vector3 impulse);
public delegate void DeathDelegate();

public class Damageable : MonoBehaviour
{

    public event DeathDelegate Died;
    public event HealthChangeDelegate HealthChanged;

    [SerializeField] private float _healthMax;

    public float Health { get; private set; }
    public bool Dead { get; private set; }
    public float Percentage => Health / _healthMax;

    private void Awake()
    {
        Health = _healthMax;
    }

    public virtual void ReceiveHit(float damage, Vector3 impulse)
    {
        if (Dead)
        {
            return;
        }

        Health -= damage;
        HealthChanged?.Invoke(-damage, impulse);

        if (Health > 0)
        {
            return;
        }

        Health = 0;
        Dead = true;
        Died?.Invoke();
    }
}
