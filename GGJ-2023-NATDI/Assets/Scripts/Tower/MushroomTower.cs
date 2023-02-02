using UnityEngine;

namespace NATDI.Tower
{
    public class MushroomTower : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] private float _fireCooldown = 5;
        [Header("Links")]
        [SerializeField] private RandomAudioSource _randomAudioSource;
        [SerializeField] private ParticleSystem _fireParticleSystem;
        [SerializeField] private Transform _projectilePoint;
        [SerializeField] private Damager _damagerProjectile;

        private CollectionService _collection;
        private ITarget _enemyTarget;
        private float _currentFireCooldown;
        private float _timeSinceLastFire;

        void Start()
        {
            _collection = Services.Get<CollectionService>();
        }

        void Update()
        {
            _timeSinceLastFire += Time.deltaTime;
            if (_timeSinceLastFire >= _currentFireCooldown)
            {
                _currentFireCooldown = _fireCooldown;
                _timeSinceLastFire = 0;
                FireProjectile();
            }
        }

        protected virtual void FireProjectile()
        {
            if (_enemyTarget == null)
            {
                _enemyTarget = _collection.GetNearestMushroomer(transform.position);
            }

            var newProjectile = Instantiate(_damagerProjectile);

            Launch(_enemyTarget, newProjectile.gameObject, _projectilePoint);

            if (_randomAudioSource != null)
            {
                _randomAudioSource.PlayRandomClip();
            }
        }

        public void Launch(ITarget enemy, GameObject projectile, Transform firingPoint)
        {
            AimTurret();
            projectile.SetActive(true);
            Vector3 startPosition = firingPoint.position;
            var ballisticProjectile = projectile.GetComponent<BallisticProjectile>();
            if (ballisticProjectile == null)
            {
                Debug.LogError("No ballistic projectile attached to projectile");
                DestroyImmediate(projectile);
                return;
            }

            Vector3 targetPoint;
            if (ballisticProjectile.fireMode == BallisticFireMode.UseLaunchSpeed)
            {
                // use speed
                targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithSpeed(startPosition,
                    enemy.ShootTargetPosition,
                    enemy.Velocity,
                    ballisticProjectile.startSpeed,
                    ballisticProjectile.arcPreference,
                    Physics.gravity.y,
                    4);
            }
            else
            {
                // use angle
                targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithAngle(startPosition,
                    enemy.ShootTargetPosition,
                    enemy.Velocity,
                    ballisticProjectile.firingAngle,
                    ballisticProjectile.arcPreference,
                    Physics.gravity.y,
                    4);
            }

            ballisticProjectile.FireAtPoint(startPosition, targetPoint);
            //ballisticProjectile.IgnoreCollision(LevelManager.instance.environmentColliders);
            PlayParticles(_fireParticleSystem, startPosition, targetPoint);
        }

        public void PlayParticles(ParticleSystem particleSystemToPlay, Vector3 origin, Vector3 lookPosition)
        {
            if (particleSystemToPlay == null)
            {
                return;
            }

            particleSystemToPlay.transform.position = origin;
            particleSystemToPlay.transform.LookAt(lookPosition);
            particleSystemToPlay.Play();
        }

        protected virtual void AimTurret()
        {
            Vector3 targetPosition = _enemyTarget.ShootTargetPosition;
            /*if (onlyYTurretRotation)
        {
            targetPosition.y = transform.position.y;
        }*/
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
