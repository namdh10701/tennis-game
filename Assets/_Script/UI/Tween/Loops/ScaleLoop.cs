using DG.Tweening;
using UnityEngine;

public class ScaleLoop : TweenLoop
{
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _endScale;

    protected override void SetupLoop()
    {
        transform.localScale = _startScale;
        _loop.Append(transform.DOScale(_endScale, _loopDuration / 2).SetEase(_endEase));
        _loop.Append(transform.DOScale(_startScale, _loopDuration / 2).SetEase(_startEase));
    }
}