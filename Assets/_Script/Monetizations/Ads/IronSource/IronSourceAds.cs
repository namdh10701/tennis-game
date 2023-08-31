using UnityEngine;
using Common;
using Services.FirebaseService.Analytics;
using Services.Adjust;
using Enviroments;
using System.Collections;
using Services.FirebaseService.Crashlytics;
using System;

namespace Monetization.Ads
{

    public class IronSourceAds : MonoBehaviour
    {
        [SerializeField] private string appkey;
        private bool _isRequestingBanner;
        private bool _isRequestingInter;
        private bool _isRequestingReward;
        private bool _isRewarded;

        public bool Initilized { get; private set; }
        public bool IsInterReady
        {
            get
            {
                if (!Initilized) return false;
                bool ret = IronSource.Agent.isInterstitialReady();
                if (!ret)
                {
                    LoadInter();
                }
                return ret;
            }
        }
        public bool IsRewardReady
        {
            get
            {
                if (!Initilized) return false;
                bool ret = IronSource.Agent.isRewardedVideoAvailable();
                return ret;
            }
        }

        private void Awake()
        {
            Initilized = false;
        }
        public void Init()
        {
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                string testKey = "85460dcd";
                appkey = testKey;
            }
            #region Banner_Event
            IronSourceBannerEvents.onAdLoadedEvent += Banner_onLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent += Banner_onLoadFailed;
            #endregion
            #region Inter_Event
            IronSourceInterstitialEvents.onAdLoadFailedEvent += Inter_onLoadFailed;
            IronSourceInterstitialEvents.onAdReadyEvent += Inter_onReady;
            IronSourceInterstitialEvents.onAdShowFailedEvent += Inter_onShowFailed;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += Inter_onShowSucceeded;
            IronSourceInterstitialEvents.onAdClosedEvent += Inter_onAdClosed;
            #endregion
            #region Reward_Event
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += Reward_onUnavailable;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += Reward_onAvailable;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Reward_onLoadFailed;
            IronSourceRewardedVideoEvents.onAdReadyEvent += Reward_onReady;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += Reward_onShowFailed;
            IronSourceRewardedVideoEvents.onAdClosedEvent += Reward_onClosed;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += Reward_onReward;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += Reward_onOpen;
            #endregion
            IronSourceEvents.onImpressionDataReadyEvent += Impression_onDataReady;
            IronSourceEvents.onSdkInitializationCompletedEvent += OnInitilized;
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(appkey);
        }

        private void Impression_onDataReady(IronSourceImpressionData impressionData)
        {
            Adjust.TrackIronsourceRevenue(impressionData);
            FirebaseAnalytics.TrackIronSourceRevenue(impressionData);
        }

        private void OnInitilized()
        {
            Initilized = true;
            LoadInter();
            //LoadReward();
        }

        private void OnApplicationPause(bool pause)
        {
            IronSource.Agent.onApplicationPause(pause);
        }
        #region Banner
        private void Banner_onLoaded(IronSourceAdInfo obj)
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            AdsController.Instance.HasBanner = true;
            _isRequestingBanner = false;
            AdsLogger.Log("Loaded", AdsController.AdType.BANNER);
            IronSource.Agent.displayBanner();
        }
        private void Banner_onLoadFailed(IronSourceError obj)
        {
            _isRequestingBanner = false;
            AdsLogger.Log($"Load failed with des: {obj.getDescription()}", AdsController.AdType.BANNER);
            LoadBanner();
        }
        public void LoadBanner()
        {
            if (!Initilized || _isRequestingBanner)
                return;
            _isRequestingBanner = true;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            AdsLogger.Log($"Request", AdsController.AdType.BANNER);
        }

        public void ToggleBanner(bool visible)
        {
            if (visible)
            {
                AdsLogger.Log($"Show", AdsController.AdType.BANNER);
                IronSource.Agent.displayBanner();
            }
            else
            {
                AdsLogger.Log($"Hide", AdsController.AdType.BANNER);
                IronSource.Agent.hideBanner();
            }
        }
        #endregion
        #region Inter
        private void Inter_onAdClosed(IronSourceAdInfo obj)
        {
            AdsLogger.Log("Closed", AdsController.AdType.INTER);
            LoadInter();
            AdsController.Instance.InvokeOnInterClose();
            AdsIntervalValidator.SetInterval(AdsController.AdType.INTER);
            AdsController.Instance.IsShowingInterAd = false;
        }

        private void Inter_onShowSucceeded(IronSourceAdInfo obj)
        {
            AdsLogger.Log("Show", AdsController.AdType.INTER);
            FirebaseAnalytics.Instance.PushEvent(Constant.INTER_SHOW);
        }

        private void Inter_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            AdsLogger.Log($"Show failed {arg1.getDescription()}", AdsController.AdType.INTER);
            LoadInter();
            AdsController.Instance.IsShowingInterAd = false;
            AdsController.Instance.InvokeOnInterClose();
        }

        private void Inter_onReady(IronSourceAdInfo obj)
        {
            AdsLogger.Log($"Ready", AdsController.AdType.INTER);
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            _isRequestingInter = false;
        }

        private void Inter_onLoadFailed(IronSourceError obj)
        {
            AdsLogger.Log($"Load failed {obj.getDescription()}", AdsController.AdType.INTER);
            _isRequestingInter = false;
            LoadInter();
        }

        public void LoadInter()
        {
            if (!Initilized || !AdsController.Instance.HasInternet || _isRequestingInter)
                return;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            _isRequestingInter = true;
            IronSource.Agent.loadInterstitial();
            AdsLogger.Log($"Request", AdsController.AdType.INTER);
        }
        public void ShowInter()
        {
            AdsLogger.Log($"Show", AdsController.AdType.INTER);
            AdsController.Instance.IsShowingInterAd = true;
            IronSource.Agent.showInterstitial();
        }
        #endregion
        #region Reward
        private void Reward_onOpen(IronSourceAdInfo info)
        {
            AdsLogger.Log($"Open", AdsController.AdType.REWARD);
            FirebaseAnalytics.Instance.PushEvent(Constant.REWARD_AD_SHOW);
            _isRewarded = false;
            AdsController.Instance.IsShowingReward = true;

        }

        private void Reward_onReward(IronSourcePlacement arg1, IronSourceAdInfo arg2)
        {
            AdsLogger.Log($"Rewarded", AdsController.AdType.REWARD);
            AdsController.Instance.InvokeOnRewarded(true);
            _isRewarded = true;
        }

        private void Reward_onClosed(IronSourceAdInfo obj)
        {
            AdsLogger.Log($"Closed", AdsController.AdType.REWARD);
            AdsController.Instance.IsShowingReward = false;
            AdsController.Instance.RewardedAdJustClose = true;
            StartCoroutine(CheckRewarded());
        }

        private IEnumerator CheckRewarded()
        {
            yield return new WaitForSeconds(.05f);
            AdsLogger.Log($"Check rewarded", AdsController.AdType.REWARD);
            if (!_isRewarded)
            {
                AdsLogger.Log($"Rewarded check failed", AdsController.AdType.REWARD);
                FirebaseAnalytics.Instance.PushEvent(Constant.REWARD_FAILED);
                AdsController.Instance.OpenNotRewardedPanel();
            }
            if (_isRewarded)
            {
                AdsLogger.Log($"Rewarded check pass", AdsController.AdType.REWARD);
                AdsIntervalValidator.SetInterval(AdsController.AdType.REWARD);
            }
        }

        private void Reward_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            AdsLogger.Log($"Show failed with des: {arg1.getDescription()}", AdsController.AdType.REWARD);
            //LoadReward();
            AdsController.Instance.IsShowingReward = false;
            AdsController.Instance.OpenNotAvailableRewardedPanel();
        }

        private void Reward_onReady(IronSourceAdInfo obj)
        {
            AdsLogger.Log($"Ready", AdsController.AdType.REWARD);
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            //_isRequestingReward = false;
        }

        private void Reward_onLoadFailed(IronSourceError obj)
        {
            AdsLogger.Log($"Load failed with des: {obj.getDescription()}", AdsController.AdType.REWARD);
            LoadReward();
            _isRequestingReward = false;
        }

        private void Reward_onAvailable(IronSourceAdInfo obj)
        {
            AdsLogger.Log($"Available", AdsController.AdType.REWARD);
            //_isRequestingReward = false;
        }

        private void Reward_onUnavailable()
        {
            AdsLogger.Log($"Unavailable", AdsController.AdType.REWARD);
        }

        public void LoadReward()
        {
            Debug.Log(_isRequestingReward + "Is rewarded requesting in LoadReward");
            if (!Initilized || !AdsController.Instance.HasInternet || _isRequestingReward)
                return;
            _isRequestingReward = true;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            AdsLogger.Log($"Request", AdsController.AdType.REWARD);
            IronSource.Agent.loadRewardedVideo();
        }
        public void ShowReward()
        {
            AdsLogger.Log($"Show", AdsController.AdType.REWARD);
            IronSource.Agent.showRewardedVideo();
        }
        #endregion
    }
}