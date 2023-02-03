using System.Collections.Generic;
using UnityEngine;

public class CollectionService : Service
{
    private List<Mushroom> _mushrooms = new();
    private List<EnemyMovementAi> _mushroomers = new();
    public List<MushroomArea> MushroomAreas = new();

    public void AddMushroom(Mushroom mushroom)
    {
        _mushrooms.Add(mushroom);
    }

    public void RemoveMushroom(Mushroom mushroom)
    {
        _mushrooms.Remove(mushroom);
    }

    public void AddMushroomer(EnemyMovementAi mushrromer)
    {
        _mushroomers.Add(mushrromer);
    }

    public void RemoveMushroomer(EnemyMovementAi value)
    {
        _mushroomers.Remove(value);
    }

    public Mushroom GetNearestMushroom(Vector3 position)
    {
        return GetNearestTarget(position, _mushrooms);
    }

    public EnemyMovementAi GetNearestMushroomer(Vector3 position, float minDistance)
    {
        return GetNearestTarget(position, _mushroomers, minDistance);
    }

    private T GetNearestTarget<T>(Vector3 position, ICollection<T> collection, float minDistance = float.MaxValue) where T : ITarget
    {
        T nearest = default;
        foreach (T candidate in collection)
        {
            float distance = Vector3.Distance(position, candidate.Position);

            if (distance >= minDistance) continue;

            minDistance = distance;
            nearest = candidate;
        }

        return nearest;
    }
}
