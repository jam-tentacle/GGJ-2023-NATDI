using System.Collections.Generic;
using UnityEngine;

namespace NATDI.Skunk
{
    public class MushroomSkunk : MonoBehaviour, IUpdate
    {
        private GameSettings _settings;
        private LinkedList<MushroomerDamageData> _mushroomersData = new LinkedList<MushroomerDamageData>();
        private bool _initialized;

        void Start()
        {
            _settings = Services.Get<AssetsCollection>().Settings;
            var collection = Services.Get<CollectionService>();
            foreach (var mushroomer in collection.Mushroomers)
            {
                SaveMushroomer(mushroomer);
            }

            collection.OnAddMushroomer += SaveMushroomer;
            collection.OnRemoveMushroomer += RemoveMushroomer;
            _initialized = true;
        }

        private void RemoveMushroomer(EnemyMovementAi mushroomer)
        {
            var node = _mushroomersData.First;

            while (node != null)
            {
                var next = node.Next;
                var info = node.Value;

                if (info.Mushroomer == mushroomer)
                {
                    _mushroomersData.Remove(node);
                }

                node = next;
            }
        }

        private void UpdateMushroomer(MushroomerDamageData data)
        {
            data.TimeSinceLastDamage += Time.deltaTime;
            if (data.TimeSinceLastDamage > _settings.SkunkDamageCooldown)
            {
                data.TimeSinceLastDamage = 0;
                data.Damagable.ReceiveHit(_settings.SkunkDamage, Vector3.up);
            }
        }

        private void SaveMushroomer(EnemyMovementAi mushroomer)
        {
            _mushroomersData.AddLast(new MushroomerDamageData()
            {
                Mushroomer = mushroomer,
                TimeSinceLastDamage = 0,
                Damagable = mushroomer.GetComponent<Damageable>()
            });
        }

        private class MushroomerDamageData
        {
            public float TimeSinceLastDamage;
            public EnemyMovementAi Mushroomer;
            public Damageable Damagable;
        }

        private bool IsInRange(EnemyMovementAi mushroomer) =>
            Vector3.Distance(mushroomer.Position, transform.position) < _settings.SkunkDamageRadius;

        public void GameUpdate(float delta)
        {
            if (!_initialized)
            {
                return;
            }
            var node = _mushroomersData.First;

            while (node != null)
            {
                var next = node.Next;
                var data = node.Value;

                if (!IsInRange(data.Mushroomer))
                {
                    node = next;
                    continue;
                }

                UpdateMushroomer(data);

                node = next;
            }
        }
    }
}
