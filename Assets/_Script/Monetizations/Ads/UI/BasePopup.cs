using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Monetization.Ads.UI
{
    public class BasePopup : MonoBehaviour
    {
        protected Tween openTween;
        protected Tween closeTween;

        public virtual Tween Open()
        {

            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            openTween = transform.DOScale(1, .2f).SetEase(Ease.OutBack);
            return openTween;
        }
        public virtual Tween Close()
        {
            closeTween = transform.DOScale(0, .2f).SetEase(Ease.InBack).OnComplete(
                () =>
                {
                    gameObject.SetActive(false);
                });
            return closeTween;
        }

        private void OnDestroy()
        {
            openTween.Kill();
            closeTween.Kill();
        }
    }
}