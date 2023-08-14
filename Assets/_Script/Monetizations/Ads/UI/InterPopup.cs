using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Gameplay;

namespace Monetization.Ads.UI
{
    public class InterPopup : BasePopup
    {
        [SerializeField] protected bool _isShowAd;

        protected override void Awake()
        {
            _panel?.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Close(_isShowAd);
               }
               );
        }
        public Tween Open(bool showAd = true)
        {
            if (showAd)
            {
                Tween openTween = base.Open().Pause();
                AdsController.Instance.ShowInter(
                     () => { openTween.Play(); });
            }
            else
            {
                base.Open();
            }
            return openTween;
        }

        public Tween Close(bool showAd = true)
        {
            if (showAd)
            {
                Tween closeTween = base.Close().Pause();
                AdsController.Instance.ShowInter(
                     () => { closeTween.Play(); });
            }
            else
            {
                base.Close();
            }
            return closeTween;
        }
    }
}