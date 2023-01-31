using System.Collections.Generic;
using UnityEngine;

public class CollectionService : Service
{
    private List<Mushroom> _mushrooms = new();

    public void AddMushroom(Mushroom mushroom)
    {
        _mushrooms.Add(mushroom);
    }

    public void RemoveMushroom(Mushroom mushroom)
    {
        _mushrooms.Remove(mushroom);
    }

    public Mushroom GetNearestMushroom(Vector3 position)
    {
        Mushroom nearest = null;
        float minDistance = float.MaxValue;
        foreach (Mushroom mushroom in _mushrooms)
        {
            float distance = Vector3.Distance(position, mushroom.Position);

            if (distance >= minDistance) continue;

            minDistance = distance;
            nearest = mushroom;
        }

        return nearest;
    }
}
