using DG.Tweening;
using UnityEngine;
public class PositionLoop : TweenLoop
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;

    protected override void SetupLoop()
    {
        transform.localPosition = _startPosition;
        _loop.Append(transform.DOLocalMove(_endPosition, _loopDuration / 2).SetEase(_endEase));
        _loop.Append(transform.DOLocalMove(_startPosition, _loopDuration / 2).SetEase(_startEase));
    }
}
