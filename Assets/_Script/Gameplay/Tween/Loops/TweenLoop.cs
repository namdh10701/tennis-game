using DG.Tweening;
using UnityEngine;
public abstract class TweenLoop : MonoBehaviour
{
    protected Sequence _loop;
    [SerializeField] protected float _loopDuration = 1.0f;
    [SerializeField] protected Ease _endEase;
    [SerializeField] protected Ease _startEase;

    protected abstract void SetupLoop();

    protected virtual void Start()
    {
        _loop = DOTween.Sequence();
        _loop.SetLoops(-1);
        SetupLoop();
        _loop.Play();
    }

    protected virtual void OnDestroy()
    {
        _loop.Kill();
    }

    protected virtual void OnEnable()
    {
        _loop.Pause();
    }

    protected virtual void OnDisable()
    {
        _loop.Play();
    }
}