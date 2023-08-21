using DG.Tweening;
using UnityEngine;

public class OpacityLoop : TweenLoop
{
    [SerializeField] private float _startOpacity;
    [SerializeField] private float _endOpacity;
    protected override void SetupLoop()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, _startOpacity);
            _loop.Append(spriteRenderer.DOFade(_endOpacity, _loopDuration / 2).SetEase(_endEase));
            _loop.Append(spriteRenderer.DOFade(_startOpacity, _loopDuration / 2).SetEase(_startEase));
        }
    }
}