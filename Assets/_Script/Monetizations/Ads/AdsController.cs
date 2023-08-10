using UnityEngine;
using System;
using System.Collections;
using Common;
using Monetization.Ads.UI;
using System.Collections.Generic;
using GoogleMobileAds.Api;

namespace Monetization.Ads
{

    public class AdsController : Singleton<AdsController>
    {
        public enum AdType
        {
            BANNER, INTER, REWARD, OPEN
        }

        public enum RewardType
        {

        }

        [SerializeField] private IronSourceAds ironsource;
        [SerializeField] private AdmobAds admob;

        public RewardType rewardType;
        public Action onInterClosed;
        public Action<bool> onRewardClosed;
        public Action onOpenAdClosed;

        public AdsUIController adsUIController;

        private List<NativeAdPanel> _nativeAdPanels = new List<NativeAdPanel>();

        public bool HasBanner { get; set; }
        public bool IsShowingAd { get; set; }
        public bool HasInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }

        protected override void Awake()
        {
            base.Awake();
            IsShowingAd = false;
        }

        public void SetInterval(AdType type)
        {
            switch (type)
            {
                case AdType.REWARD:
                    break;
                case AdType.INTER:
                    break;
                case AdType.OPEN:
                    break;
            }
        }
        public bool CheckInterval(AdType type)
        {
            bool ret = true;
            switch (type)
            {
                case AdType.REWARD:
                    break;
                case AdType.INTER:
                    break;
                case AdType.OPEN:
                    break;
            }
            return ret;
        }

        #region NativeAd
        public void RegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            _nativeAdPanels.Add(nativeAdPanel);
        }
        public void UnRegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            if (_nativeAdPanels.Contains(nativeAdPanel))
                _nativeAdPanels.Remove(nativeAdPanel);
        }
        public void OnNativeAdLoaded(NativeAd nativeAd)
        {
            foreach (NativeAdPanel nativeAdPanel in _nativeAdPanels)
            {
                if (nativeAdPanel.NativeAd == null)
                {
                    nativeAdPanel.NativeAd = nativeAd;
                }
            }
        }
        public void ShowNativeAd(NativeAdPanel nativeAdPanel)
        {
            if (_nativeAdPanels.Contains(nativeAdPanel))
                nativeAdPanel.Show();
        }
        public void HideNativeAd(NativeAdPanel nativeAdPanel)
        {
            if (_nativeAdPanels.Contains(nativeAdPanel))
                nativeAdPanel.Hide();
        }
        #endregion
        #region OpenAd
        public void ShowAppOpenAd()
        {
            if (admob.IsAppOpenAdAvailable)
            {
                admob.ShowAppOpenAd();
            }
            // else if (other ad source)
        }
        #endregion
        #region Reward
        public void ShowReward(Action<bool> watched)
        {
            onRewardClosed = watched;

            if (ironsource.IsRewardReady)
            {
                ironsource.ShowReward();
            }
            //else if(other ad source)
            else if (!HasInternet)
            {
                onRewardClosed.Invoke(false);
                onRewardClosed = null;
            }
            else
            {
                StartCoroutine(WaitForRewardVideo());
            }

            IEnumerator WaitForRewardVideo()
            {
                ironsource.LoadReward();
                //load ad here 
                adsUIController.ShowWaitingBox();
                yield return new WaitForSeconds(3f);
                adsUIController.CloseWaitingBox();
                if (ironsource.IsRewardReady)
                {
                    ironsource.ShowReward();
                }
                //else if(other ad source)
                else
                {
                    adsUIController.ShowRewardUnavailableBox();
                    onRewardClosed.Invoke(false);
                    onRewardClosed = null;
                }
            }
        }

        #endregion
        #region Banner
        public void ShowBanner()
        {
            ironsource.LoadBanner();
        }
        public void ToggleBanner(bool visible)
        {
            ironsource.ToggleBanner(visible);
        }
        #endregion
        #region Inter
        public void ShowInter(Action onInterClosed)
        {
            this.onInterClosed = onInterClosed;
            if (ironsource.IsInterReady)
            {
                ironsource.ShowInter();
            }
            // else if (other.isInterReady)
        }
        #endregion
    }
}