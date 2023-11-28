using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Gameplay;

namespace Monetization.Ads.UI
{
    //Extends class này cho những popup mở lên sau khi xem Inter ad
    public class InterPopup : BasePopup
    {
        // khi đóng popup thì có show ad không
        [SerializeField] protected bool _isShowAd;

        protected override void Awake()
        {
            //Đóng popup bằng cách click vào cái nền đen
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
                AdsController.Instance.ShowInter(
                     () => { base.Open(); }
                );
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
                AdsController.Instance.ShowInter(
                     () => { base.Close(); }
                );
            }
            else
            {
                base.Close();
            }
            return closeTween;
        }
    }
}