using System;
using UnityEngine;

public class MushroomTargeter
{
    private float _currentTime;

    private AssetsCollection _assetsCollection;
    private CollectionService _collectionService;

    private MushroomArea _currentArea;

    private Mushroom _currentMushroom;
    public Mushroom CurrentMushroom => _currentMushroom;

    public Transform _holder;

    private Action _onTargetUpdated;

    public void Init(Transform holder, Action onTargetUpdated)
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _collectionService = Services.Get<CollectionService>();

        _holder = holder;
        _onTargetUpdated = onTargetUpdated;
    }

    public void FixedUpdate(float delta)
    {
        if (_currentArea == null || _currentMushroom == null)
        {
            TryChangeTarget();
            return;
        }

        _currentTime += delta;

        if (_assetsCollection.Settings.ChangeAreaTargetTime <= _currentTime)
        {
            TryChangeTarget();
            _currentTime = 0f;
        }
    }

    public void TryChangeTarget()
    {
        TryChangeAreaTarget();

        TryChangeMushroomTarget();
    }

    public void TryChangeAreaTarget()
    {
        if (_currentArea != null && _currentArea.HasMushrooms && _assetsCollection.Settings.SaveAreaTargetDistance >=
            Vector3.Distance(_holder.position, _currentArea.Position))
        {
            return;
        }

        if (_currentArea != null && _currentArea.HasMushrooms && _currentArea.Position.x <= _holder.position.x)
        {
            return;
        }

        if (_currentArea != null)
        {
            _currentArea.IsUnderAim = false;
        }

        _currentArea = _collectionService.GetNearestMushroomArea(_holder.position);
        _currentArea.IsUnderAim = true;
    }

    private void TryChangeMushroomTarget()
    {
        _currentMushroom = _collectionService.GetNearestMushroom(_holder.position, _currentArea);

        if (_currentMushroom == null)
        {
            return;
        }

        _onTargetUpdated?.Invoke();
    }
}
