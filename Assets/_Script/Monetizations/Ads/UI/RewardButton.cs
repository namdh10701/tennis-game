using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Monetization.Ads.UI
{
    public class RewardButton
    {
        [SerializeField] private InterPopup popup;
        [SerializeField] private UnityEvent reward;
        public void OnClicked()
        {
            if (popup)
            {
                popup.Close(false).onComplete += ()
                 =>
                {
                    HandleShowRewardAd();
                };
            }
            else
            {
                HandleShowRewardAd();
            }
        }
        private void HandleShowRewardAd()
        {
            AdsController.Instance.ShowReward(watched =>
            {
                if (watched)
                {
                    reward.Invoke();
                }
            });
        }
    }
}