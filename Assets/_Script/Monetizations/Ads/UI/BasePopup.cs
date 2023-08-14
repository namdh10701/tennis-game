using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Monetization.Ads.UI
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField] protected Image _panel;
        [SerializeField] protected Transform _contents;
        protected Tween openTween;
        protected Tween closeTween;
        protected virtual void Awake()
        {
            _panel?.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Close();
               }
               );
        }
        public virtual Tween Open()
        {

            gameObject.SetActive(true);
            _contents.transform.localScale = Vector3.zero;
            openTween = _contents.transform.DOScale(1, .2f).SetEase(Ease.OutBack).SetUpdate(true);
            return openTween;
        }
        public virtual Tween Close()
        {
            _contents.transform.localScale = Vector3.one;
            closeTween = _contents.transform.DOScale(0, .2f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(
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