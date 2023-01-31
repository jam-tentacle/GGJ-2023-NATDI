using UnityEngine;

public class Mushroom : MonoBehaviour, ITarget
{
    public Vector3 Position => transform.position;
}
