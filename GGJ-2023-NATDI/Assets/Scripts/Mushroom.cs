using DG.Tweening;
using UnityEngine;

public class Mushroom : MonoBehaviour, ITarget
{
    [SerializeField] private Transform _leg;
    [SerializeField] private Transform _cap;

    public Vector3 Position => transform.position;

    private void Start()
    {
        StartGrowing();
    }

    private void StartGrowing()
    {
        Vector3 endLegScale = _leg.localScale;
        Vector3 endCapScale = _cap.localScale;

        _leg.localScale = Vector3.zero;
        _cap.localScale = Vector3.zero;

        DOTween.Sequence()
            .Append(_leg.DOScale(endLegScale, 0.3f))
            .Append(_cap.DOScale(endCapScale, 0.3f));
    }
}
