using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BusService : Service, IStart, IUpdate
{
    [SerializeField] public float _busArrivalSpeed;
    [SerializeField] public float _offset;
    [SerializeField] private float _delayToEnd;
    private List<MushroomerSpawnData> _mushroomerSpawnData = new();
    private BusEndPoint _busEndPoint;
    private BusStartPoint _busStartPoint;
    private BusStationPoint _busStationPoint;
    private Bus _bus;
    private float _currentTimeData;
    private int _currentBusArrives;
    private MushroomerSpawner _spawnService;
    private MushroomerSpawnData _spawnData;
    private bool _firstTime;

    public void GameStart()
    {
        _busEndPoint = FindObjectOfType<BusEndPoint>();
        _busStartPoint = FindObjectOfType<BusStartPoint>();
        _busStationPoint = FindObjectOfType<BusStationPoint>();
        _bus = FindObjectOfType<Bus>();
        _spawnService = Services.Get<MushroomerSpawner>();
        _mushroomerSpawnData = _spawnService.GetSpawnData;
        DOTween.Sequence()
            .Append(_bus.transform.DOMove(_busEndPoint.transform.position, _busArrivalSpeed).SetEase(Ease.InCubic))
            .Append(_bus.transform.DOMove(_busStartPoint.transform.position, 0f));
    }

    public void GameUpdate(float delta)
    {
        _currentTimeData += Time.deltaTime;

        if (_currentBusArrives >= _mushroomerSpawnData.Count) return;

        _spawnData = _mushroomerSpawnData[_currentBusArrives];

        if (_currentTimeData <= _spawnData.OverallSpawnTime - _offset) return;

        _currentBusArrives++;

        if (_spawnData.OverallSpawnTime == 0) return;

        DOTween.Sequence()
            .Append(_bus.transform.DOMove(_busStationPoint.transform.position, _busArrivalSpeed).SetEase(Ease.OutCubic))
            .AppendInterval(_delayToEnd)
            .Append(_bus.transform.DOMove(_busEndPoint.transform.position, _busArrivalSpeed).SetEase(Ease.InCubic))
            .Append(_bus.transform.DOMove(_busStartPoint.transform.position, 0f));
    }
}
