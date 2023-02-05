using NATDI;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
    void GameUpdate(float delta);
}

public interface ILateUpdate
{
    void GameLateUpdate(float delta);
}

public interface IStart
{
    void GameStart();
}

public interface IInject
{
    void Inject();
}

public abstract class Service : MonoBehaviour { }

public class Services : MonoBehaviour
{
    private static Dictionary<Type, Service> _services = new();
    private static List<IUpdate> _updates = new();
    private static List<ILateUpdate> _lateUpdates = new();
    private static List<IStart> _starts = new();
    private static List<IInject> _injects = new();
    private FlowService _flowService;

    private void Awake()
    {
        Service[] services =  FindObjectsOfType<Service>();
        foreach (Service service in services)
        {
            _services.Add(service.GetType(), service);

            if (service is IUpdate update)
            {
                _updates.Add(update);
            }

            if (service is ILateUpdate lateUpdate)
            {
                _lateUpdates.Add(lateUpdate);
            }

            if (service is IStart start)
            {
                _starts.Add(start);
            }

            if (service is IInject inject)
            {
                _injects.Add(inject);
            }
        }

        foreach (IInject inject in _injects)
        {
            inject.Inject();
        }

        _flowService = Get<FlowService>();
    }

    private void Start()
    {
        foreach (IStart start in _starts)
        {
            start.GameStart();
        }
    }

    private void Update()
    {
        if (_flowService.CurrentFlowState == FlowService.FlowState.Paused)
        {
            return;
        }

        foreach (IUpdate update in _updates)
        {
            update.GameUpdate(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (_flowService.CurrentFlowState == FlowService.FlowState.Paused)
        {
            return;
        }

        foreach (ILateUpdate update in _lateUpdates)
        {
            update.GameLateUpdate(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _services.Clear();
        _updates.Clear();
        _lateUpdates.Clear();
        _starts.Clear();
        _injects.Clear();
    }

    public static T Get<T>() where T : Service => (T) _services[typeof(T)];
}
