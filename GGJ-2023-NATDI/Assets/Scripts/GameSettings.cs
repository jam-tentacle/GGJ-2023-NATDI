using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float MushroomCreatorReloadTime = 10f;
    public float TowerDamage = 1000f;
}
