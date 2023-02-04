using System;
using UnityEngine;

namespace NATDI.Tower
{
    public class MushroomTower : MonoBehaviour, IUpdate
    {

        [Header("Links")]
        [SerializeField] private RandomAudioSource _randomAudioSource;
        [SerializeField] private Transform _projectilePoint;
        [SerializeField] private Damager _damagerProjectile;

        private CollectionService _collection;
        private ITarget _enemyTarget;
        private float _timeSinceLastFire;
        private AssetsCollection _assetsCollection;

        void Start()
        {
            _collection = Services.Get<CollectionService>();
            _assetsCollection = Services.Get<AssetsCollection>();
        }

        public void GameUpdate(float delta)
        {
            _timeSinceLastFire += Time.deltaTime;
            if (_timeSinceLastFire >= _assetsCollection.Settings.FireTowerCooldown)
            {
                if (TryFireProjectile())
                {
                    _timeSinceLastFire = 0;
                }
            }
        }

        protected virtual bool TryFireProjectile()
        {
            if (_enemyTarget == null || !_enemyTarget.IsAlive ||
                Vector3.Distance(_enemyTarget.ShootTargetPosition, transform.position) > _assetsCollection.Settings.FireTowerRadius)
            {
                _enemyTarget = _collection.GetNearestMushroomer(transform.position, _assetsCollection.Settings.FireTowerRadius);
            }

            if (_enemyTarget is null)
            {
                return false;
            }

            if (!_enemyTarget.IsAlive)
            {
                return false;
            }

            if (_assetsCollection.Settings.FireTowerRadius > Vector3.Distance(_enemyTarget.ShootTargetPosition, transform.position))
            {
                return false;
            }

            var newProjectile = Instantiate(_damagerProjectile);

            Launch(_enemyTarget, newProjectile.gameObject, _projectilePoint);

            if (_randomAudioSource != null)
            {
                _randomAudioSource.PlayRandomClip();
            }

            return true;
        }

        public void Launch(ITarget enemy, GameObject projectile, Transform firingPoint)
        {
            AimTurret();
            projectile.SetActive(true);
            Vector3 startPosition = firingPoint.position;
            var autoProjectile = projectile.GetComponent<AutoProjectile>();
            if (autoProjectile == null)
            {
                Debug.LogError("No ballistic projectile attached to projectile");
                DestroyImmediate(projectile);
                return;
            }

            autoProjectile.Fire(startPosition, enemy);
        }

        protected virtual void AimTurret()
        {
            Vector3 targetPosition = _enemyTarget.ShootTargetPosition;
            Vector3 direction = targetPosition - transform.position;
            Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 lookEuler = look.eulerAngles;
            // We need to convert the rotation to a -180/180 wrap so that we can clamp the angle with a min/max
            float x = Wrap180(lookEuler.x);
            lookEuler.x = Mathf.Clamp(x, 0, 359);
            look.eulerAngles = lookEuler;
            transform.rotation = look;
        }

        /// <summary>
        /// A simply function to convert an angle to a -180/180 wrap
        /// </summary>
        static float Wrap180(float angle)
        {
            angle %= 360;
            if (angle < -180)
            {
                angle += 360;
            }
            else if (angle > 180)
            {
                angle -= 360;
            }
            return angle;
        }
    }
}
