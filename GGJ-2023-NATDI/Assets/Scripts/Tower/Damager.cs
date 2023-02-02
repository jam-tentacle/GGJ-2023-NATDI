using UnityEngine;

namespace NATDI.Tower
{
	/// <summary>
	/// A component that does damage to damageables
	/// </summary>
	public class Damager : MonoBehaviour
	{
        /// <summary>
		/// Random chance to spawn collision projectile prefab
		/// </summary>
		[Range(0, 1)]
		public float chanceToSpawnCollisionPrefab = 1.0f;

		/// <summary>
		/// The particle system to fire off when the damager attacks
		/// </summary>
		public ParticleSystem collisionParticles;

		/// <summary>
		/// Instantiate particle system and play it
		/// </summary>
		void OnCollisionEnter(Collision other)
		{
			if (collisionParticles == null || Random.value > chanceToSpawnCollisionPrefab)
			{
				return;
			}

			var pfx = Poolable.TryGetPoolable<ParticleSystem>(collisionParticles.gameObject);

			pfx.transform.position = transform.position;
			pfx.Play();
            var receiver = other.gameObject.GetComponent<Damageable>();
            Debug.Log($"tower projectile collision w: {other.gameObject.name}");
            if (receiver != null)
            {
                receiver.ReceiveHit(Services.Get<AssetsCollection>().Settings.TowerDamage, GetComponent<Rigidbody>().velocity);
            }
        }
	}
}
