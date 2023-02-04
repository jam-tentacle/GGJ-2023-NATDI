using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusService : Service, IStart, IUpdate
{
    [SerializeField] public float _busArrivalSpeed;
    private List<MushroomerSpawnData> _mushroomerSpawnData = new();
    private BusEndPoint _busEndPoint;
    private BusStartPoint _busStartPoint;
    private BusStationPoint _busStationPoint;
    private Bus _bus;
    private float _currentTimeData;
    private int _currentBusArrives;
    private MushroomerSpawner _spawnService;
    private MushroomerSpawnData _spawnData;

    public void GameStart()
    {
        _busEndPoint = FindObjectOfType<BusEndPoint>();
        _busStartPoint = FindObjectOfType<BusStartPoint>();
        _busStationPoint = FindObjectOfType<BusStationPoint>();
        _bus = FindObjectOfType<Bus>();
        _spawnService = Services.Get<MushroomerSpawner>();
        _mushroomerSpawnData = _spawnService.GetSpawnData;
    }

    public void GameUpdate(float delta)
    {
        return;

        _currentTimeData += Time.deltaTime;

        if (_currentBusArrives >= _mushroomerSpawnData.Count) return;

        _spawnData = _mushroomerSpawnData[_currentBusArrives];

        if (_currentTimeData <= _spawnData.OverallSpawnTime) return;

        DOTween.Sequence()
            .AppendInterval(1f + _spawnData.OverallSpawnTime * _spawnData.MushroomerCount)
            .Append(_bus.transform.DOMove(_busEndPoint.transform.position, _busArrivalSpeed).SetEase(Ease.InCubic))
            .Append(_bus.transform.DOMove(_busStartPoint.transform.position, 0f))
            .AppendInterval(1f + _spawnData.OverallSpawnTime * _spawnData.MushroomerCount)
            .Append(_bus.transform.DOMove(_busStationPoint.transform.position, _busArrivalSpeed)
                .SetEase(Ease.OutCubic));

        _currentBusArrives++;
    }

    // public void RunBus(Action callback)
    // {
    //     DOTween.Sequence()
    //         .Append(_bus.transform.DOMove(_busEndPoint.transform.position, 4f).SetEase(Ease.OutQuint))
    //         .AppendCallback(callback.Invoke)
    //         .AppendInterval(5f)
    //         .Append(_bus.transform.DOMove(_busEndPoint.transform.position + Vector3.forward * 10f, 4f)
    //             .SetEase(Ease.OutQuint));
    // }
}
