using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectionService : Service, IStart
{
    private List<Mushroom> _mushrooms = new();
    private List<EnemyMovementAi> _mushroomers = new();
    private List<MushroomArea> MushroomAreas = new();
    public int GetAreaCount => MushroomAreas.Count;

    public List<EnemyMovementAi> Mushroomers => _mushroomers;
    public List<Mushroom> Mushrooms => _mushrooms;

    public event Action<EnemyMovementAi> OnAddMushroomer;
    public event Action<EnemyMovementAi> OnRemoveMushroomer;

    private AssetsCollection _assetsCollection;

    public void GameStart()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public void AddMushroom(Mushroom mushroom)
    {
        Mushrooms.Add(mushroom);
    }

    public void AddMushroomArea(MushroomArea area)
    {
        MushroomAreas.Add(area);
    }

    public void RemoveMushroom(Mushroom mushroom)
    {
        Mushrooms.Remove(mushroom);
    }

    public void AddMushroomer(EnemyMovementAi value)
    {
        Mushroomers.Add(value);
        OnAddMushroomer?.Invoke(value);
    }

    public void RemoveMushroomer(EnemyMovementAi value)
    {
        Mushroomers.Remove(value);
        OnRemoveMushroomer?.Invoke(value);
    }

    public Mushroom GetNearestMushroom(Vector3 position)
    {
        return GetNearestTarget(position, Mushrooms);
    }

    public Mushroom GetNearestMushroom(Vector3 position, MushroomArea mushroomArea)
    {
        return GetNearestTarget(position, mushroomArea.Mushrooms);
    }

    public EnemyMovementAi GetNearestMushroomer(Vector3 position, float minDistance)
    {
        return GetNearestTarget(position, Mushroomers, minDistance);
    }

    private T GetNearestTarget<T>(Vector3 position, ICollection<T> collection, float minDistance = float.MaxValue) where T : ITarget
    {
        T nearest = default;
        foreach (T candidate in collection)
        {
            if (!candidate.IsAlive)
            {
                continue;
            }

            float distance = Vector3.Distance(position, candidate.Position);

            if (distance >= minDistance) continue;

            minDistance = distance;
            nearest = candidate;
        }

        return nearest;
    }

    public MushroomArea GetNearestMushroomArea(Vector3 position)
    {
        var directionToMain = Vector3.right;

        var maxAngle = 90;

        float currentAngle = 0f;
        MushroomArea currentArea = null;

        foreach (var mushroomArea in MushroomAreas)
        {
            if (mushroomArea == _assetsCollection.MainMushroomArea)
            {
                continue;
            }

            if (position.x > mushroomArea.Position.x)
            {
                continue;
            }

            if (!mushroomArea.HasMushrooms)
            {
                continue;
            }

            if (mushroomArea.IsUnderAim)
            {
                continue;
            }

            var directionToArea = ( mushroomArea.Position - position).normalized;

            var angle = Vector3.Angle(directionToMain, directionToArea);

            if (angle >= maxAngle)
            {
                continue;
            }

            if (angle > currentAngle)
            {
                currentArea = mushroomArea;
                currentAngle = angle;
            }
        }

        if (currentArea == null)
        {
            currentArea = _assetsCollection.MainMushroomArea;
        }

        return currentArea;
    }
}
