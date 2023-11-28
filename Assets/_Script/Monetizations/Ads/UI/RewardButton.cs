using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Monetization.Ads.UI
{
    public class RewardButton : MonoBehaviour
    {
        [SerializeField] private BasePopup popup;
        [SerializeField] private UnityEvent reward;
        [SerializeField] private GameObject adImage;
        private Button button;
        private bool _isButtonActive;
        [SerializeField] private bool _isClosingFirst;
        public bool IsButtonActive
        {
            get { return _isButtonActive; }
            set
            {
                _isButtonActive = value;
                adImage?.SetActive(value);
                button.onClick.RemoveListener(() => OnClicked());
            }
        }
        private void Awake()
        {
            _isButtonActive = true;
            button = GetComponent<Button>();
            button.onClick.AddListener(() => OnClicked());
        }
        public void OnClicked()
        {
            if (!IsButtonActive)
            {
                return;
            }
            if (popup)
            {
                if (_isClosingFirst)
                {
                    popup.Close().onComplete += ()
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
                    popup?.Close();
                }
                else
                {
                    popup?.Open();
                }
            });
        }
    }
}