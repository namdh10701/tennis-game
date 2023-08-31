using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System;

namespace Monetization.Ads.UI
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField] protected Image _panel;
        [SerializeField] protected Transform _contents;
        protected Tween openTween;
        protected Tween closeTween;
        public enum State
        {
            CLOSED, OPENED
        }
        public State CurrentState { get; protected set; }
        protected virtual void Awake()
        {
            _panel?.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Close();
               }
               );
            CurrentState = State.CLOSED;
        }
        public virtual Tween Open()
        {
            if (CurrentState == State.OPENED)
            {
                return null;
            }
            gameObject.SetActive(true);
            if (openTween != null && openTween.IsPlaying())
            {
                return null;
            }
            _contents.transform.localScale = Vector3.zero;
            openTween = _contents.transform.DOScale(1, .2f).SetEase(Ease.OutBack).SetUpdate(true);
            openTween.onComplete += () =>
            {
                openTween = null;
                CurrentState = State.OPENED;
            };
            return openTween;
        }
        public virtual Tween Close()
        {
            if (CurrentState == State.CLOSED)
            {
                return null;
            }
            if (closeTween != null && closeTween.IsPlaying())
            {
                return null;
            }
            _contents.transform.localScale = Vector3.one;
            closeTween = _contents.transform.DOScale(0, .2f).SetEase(Ease.InBack).SetUpdate(true);
            closeTween.onComplete += () =>
            {
                gameObject.SetActive(false);
                closeTween = null;
                CurrentState = State.CLOSED;
            };
            return closeTween;
        }

        public virtual void Close(Action onClosed)
        {
            if (Close() != null)
            {
                closeTween.onComplete += () => { onClosed?.Invoke(); };
            }
        }

        private void OnDestroy()
        {
            openTween.Kill();
            closeTween.Kill();
        }
    }
}