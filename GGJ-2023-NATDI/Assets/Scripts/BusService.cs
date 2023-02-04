using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusService : Service, IStart, IUpdate
{
    [SerializeField] private List<BusScheduleData> _busScheduleData = new();

    private BusEndPoint _busEndPoint;
    private Bus _bus;
    private float _currentTimeData;
    private int _currentBusArrives;

    public void GameStart()
    {
        _busEndPoint = FindObjectOfType<BusEndPoint>();
        _bus = FindObjectOfType<Bus>();
    }

    public void GameUpdate(float delta)
    {
        return;

        _currentTimeData += Time.deltaTime;
        if (_currentTimeData < _busScheduleData[_currentBusArrives].BusArriveSchedule) return;

        DOTween.Sequence()
            .Append(_bus.transform.DOMove(_busEndPoint.transform.position, 4f).SetEase(Ease.OutQuint))
            .AppendInterval(5f)
            .Append(_bus.transform.DOMove(_busEndPoint.transform.position + Vector3.forward * 10f, 4f)
                .SetEase(Ease.OutQuint));
        _currentBusArrives++;
    }
}
