using UnityEngine;
using Monetization.Ads.UI;
using DG.Tweening;

namespace Monetization.Ads
{
    public class AdsUIController : MonoBehaviour
    {
        [SerializeField] private BasePopup _waitingBox;
        [SerializeField] private BasePopup _rewardUnavailable;
        [SerializeField] private BasePopup _rewardNotRewarded;
        public void ShowWaitingBox()
        {
            _waitingBox.Open();
        }
        public Tween CloseWaitingBox()
        {
            return _waitingBox.Close();
        }

        public void ShowRewardUnavailableBox()
        {
            _rewardUnavailable.Open();
        }
        public void CloseRewardUnavailableBox()
        {
            _rewardUnavailable.Close();
        }
        public void ShowNotRewardedBox()
        {
            _rewardNotRewarded.Open();
        }
        public void HideNotRewardedBox()
        {
            _rewardNotRewarded.Close();
        }

        private void Start()
        {
            AdsController.Instance.RegisterAdsUI(this);
        }
    }
}
