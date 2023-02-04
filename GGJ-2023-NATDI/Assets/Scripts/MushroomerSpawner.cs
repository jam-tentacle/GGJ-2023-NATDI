using System.Collections.Generic;
using UnityEngine;

public class MushroomerSpawner : Service, IUpdate, IStart
{
    [SerializeField] private List<MushroomerSpawnData> _mushroomerSpawnData = new();
    [SerializeField] private GameObject _gameObject;
    private float _passedTimeBetweenSpawn;
    private float _passedTimeWave;
    private int _numberOfEnemy;
    private int _completedWavesCount;
    private SpawnPoint _point;
    private float _currentOverallTime;

    public void GameUpdate(float delta)
    {
        _currentOverallTime += Time.deltaTime;

        if (_completedWavesCount == _mushroomerSpawnData.Count) return;

        MushroomerSpawnData currentSpawnData = _mushroomerSpawnData[_completedWavesCount];

        if (_currentOverallTime <= currentSpawnData.OverallSpawnTime) return;

        _passedTimeWave -= Time.deltaTime;

        if (_passedTimeWave > 0f) return;

        _passedTimeWave = currentSpawnData.SpawnBetweenMushroomerTime;
        Instantiate(_gameObject, _point.transform.position, Quaternion.identity);
        _numberOfEnemy++;

        if (_numberOfEnemy < currentSpawnData.MushroomerCount) return;

        _completedWavesCount++;
        _numberOfEnemy = 0;
        _passedTimeWave = 0;
    }

    public void GameStart()
    {
        _point = FindObjectOfType<SpawnPoint>();
    }
}
