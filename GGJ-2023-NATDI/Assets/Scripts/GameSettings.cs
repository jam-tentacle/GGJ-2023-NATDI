﻿using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float MushroomCreatorReloadTime = 10f;
    [Header("Tower")]
    public float TowerDamage = 1f;
    public float FireTowerRadius = 10f;
    [Header("Skunk")]
    public float SkunkDamage = 1f;
    public float SkunkDamageCooldown = 2f;
    public float SkunkDamageRadius = 5f;
    [Space(20)]
    public float ChangeAreaTargetTime = 1f;
    public float SaveAreaTargetDistance = 7f;
}
