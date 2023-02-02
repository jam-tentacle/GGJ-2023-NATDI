using System;
using UnityEngine;

public class ChooseMushroomAreaTarget : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private GameObject _icon;
    [SerializeField] private MushroomArea _mushroomArea;
    [SerializeField] private Transform _startRayPoint;
    public Transform StartRayPoint => _startRayPoint;

    public MushroomArea MushroomArea => _mushroomArea;

    private bool _isHighlight;

    private void Awake()
    {
        if (!_isHighlight)
        {
            StopHighlight();
        }
    }

    public void StartHighlight()
    {
        _effect.Play();
        _icon.SetActive(true);
        _isHighlight = true;
    }

    public void StopHighlight()
    {
        _effect.Stop();
        _effect.Clear();
        _icon.SetActive(false);
        _isHighlight = false;
    }
}
