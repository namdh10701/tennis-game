﻿using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Monetization.Ads.UI
{
    public class RewardButton : MonoBehaviour
    {
        [SerializeField] private InterPopup popup;
        [SerializeField] private UnityEvent reward;
        [SerializeField] private GameObject adImage;
        private Button button;
        private bool _isButtonActive;
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