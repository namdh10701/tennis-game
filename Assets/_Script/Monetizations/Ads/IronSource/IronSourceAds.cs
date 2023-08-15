using UnityEngine;
using Common;
using Services.FirebaseService.Analytics;
using Services.Adjust;
using Enviroments;

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
                    Debug.Log("inter is called but not ready");
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
                if (!ret)
                {
                    LoadReward();
                }
                return ret;
            }
        }

        public void Init()
        {
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                string testKey = "85460dcd";
                appkey = testKey;
            }
            Initilized = false;
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
            IronSource.Agent.setManualLoadRewardedVideo(true);
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += Reward_onUnavailable;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += Reward_onAvailable;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Reward_onLoadFailed;
            IronSourceRewardedVideoEvents.onAdReadyEvent += Reward_onReady;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += Reward_onShowFailed;
            IronSourceRewardedVideoEvents.onAdClosedEvent += Reward_onClosed;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += Reward_onReward;
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
            LoadReward();
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
            IronSource.Agent.displayBanner();
        }
        private void Banner_onLoadFailed(IronSourceError obj)
        {
            _isRequestingBanner = false;
            LoadBanner();
        }
        public void LoadBanner()
        {
            if (!Initilized || !AdsController.Instance.HasInternet || _isRequestingBanner
                || AdsController.Instance.HasBanner)
                return;
            _isRequestingBanner = true;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        }

        public void ToggleBanner(bool visible)
        {
            if (visible)
            {
                IronSource.Agent.displayBanner();
            }
            else
            {
                IronSource.Agent.hideBanner();
            }
        }
        #endregion
        #region Inter
        private void Inter_onAdClosed(IronSourceAdInfo obj)
        {
            Debug.Log("Inter closed");
            LoadInter();
            AdsController.Instance.InvokeOnInterClose();
            AdsIntervalValidator.SetInterval(AdsController.AdType.INTER);
            AdsController.Instance.IsShowingAd = false;
        }

        private void Inter_onShowSucceeded(IronSourceAdInfo obj)
        {
            Debug.Log("Inter show");
            FirebaseAnalytics.Instance.PushEvent(Constant.INTER_SHOW);
        }

        private void Inter_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            Debug.Log("Inter show failed with details: " + arg1);
            LoadInter();
            AdsController.Instance.IsShowingAd = false;
        }

        private void Inter_onReady(IronSourceAdInfo obj)
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            Debug.Log("Inter ready");
            _isRequestingInter = false;
        }

        private void Inter_onLoadFailed(IronSourceError obj)
        {
            Debug.Log("Inter loaded failed" + obj.getDescription());
            _isRequestingInter = false;
            LoadInter();
        }

        public void LoadInter()
        {
            Debug.Log("requesting inter" + _isRequestingInter);
            if (!Initilized || !AdsController.Instance.HasInternet || _isRequestingInter)
                return;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            _isRequestingInter = true;
            IronSource.Agent.loadInterstitial();
        }
        public void ShowInter()
        {
            AdsController.Instance.IsShowingAd = true;
            IronSource.Agent.showInterstitial();
        }
        #endregion
        #region Reward
        private void Reward_onReward(IronSourcePlacement arg1, IronSourceAdInfo arg2)
        {
            AdsController.Instance.InvokeOnRewarded(true);
            _isRewarded = true;
        }

        private void Reward_onClosed(IronSourceAdInfo obj)
        {
            Debug.Log("reward ad closed");
            LoadReward();
            AdsController.Instance.IsShowingAd = false;
            if (!_isRewarded)
                AdsController.Instance.OpenNotRewardedPanel();
            if (_isRewarded)
            {
                AdsIntervalValidator.SetInterval(AdsController.AdType.REWARD);
            }
        }

        private void Reward_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            Debug.Log("reward ad show failed");
            LoadReward();
            AdsController.Instance.IsShowingAd = false;
            AdsController.Instance.InvokeOnRewarded(false);
        }

        private void Reward_onReady(IronSourceAdInfo obj)
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            Debug.Log("reward ad ready");
            _isRequestingReward = false;
        }

        private void Reward_onLoadFailed(IronSourceError obj)
        {
            Debug.Log("reward ad load failed" + obj.getDescription());
            LoadReward();
            _isRequestingReward = false;
        }

        private void Reward_onAvailable(IronSourceAdInfo obj)
        {
            Debug.Log("reward ad available");
            _isRequestingReward = false;
        }

        private void Reward_onUnavailable()
        {
            Debug.Log("reward ad unavailable");
            _isRequestingReward = false;
        }

        public void LoadReward()
        {
            Debug.Log(_isRequestingReward);
            if (!Initilized || !AdsController.Instance.HasInternet || _isRequestingReward)
                return;
            _isRequestingReward = true;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            IronSource.Agent.loadRewardedVideo();
        }
        public void ShowReward()
        {
            AdsController.Instance.IsShowingAd = true;
            _isRewarded = false;
            IronSource.Agent.showRewardedVideo();
        }
        #endregion
    }
}