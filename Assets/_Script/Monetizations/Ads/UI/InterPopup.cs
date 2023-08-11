using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
namespace Monetization.Ads.UI
{
    public class InterPopup : BasePopup
    {
        [SerializeField] protected bool _isShowAd;

        protected override void Awake()
        {
            _panel.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Close(_isShowAd);
               }
               );
        }
        public void Open(bool showAd = true)
        {
            if (showAd)
            {
                AdsController.Instance.ShowInter(
                    () => { base.Open(); });
            }
            else
            {
                base.Open();
            }
        }

        public void Close(bool showAd = true)
        {
            if (showAd)
            {
                AdsController.Instance.ShowInter(
                     () => { base.Close(); });
            }
            else
            {
                base.Close();
            }
        }
    }
}