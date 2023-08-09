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
            IronSourceInterstitialEvents.onAdOpenedEvent += Inter_onAdOpened;
            IronSourceInterstitialEvents.onAdClosedEvent += Inter_onAdClosed;
            #endregion
            #region Reward_Event
            IronSource.Agent.setManualLoadRewardedVideo(true);
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += Reward_onUnavailable;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += Reward_onAvailable;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Reward_onLoadFailed;
            IronSourceRewardedVideoEvents.onAdReadyEvent += Reward_onReady;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += Reward_onShowFailed;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += Reward_onOpened;
            IronSourceRewardedVideoEvents.onAdClosedEvent += Reward_onClosed;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += Reward_onReward;
            #endregion
            IronSourceEvents.onImpressionDataReadyEvent += Impression_onDataReady;
            IronSourceEvents.onSdkInitializationCompletedEvent += PostInit;
            IronSource.Agent.init(appkey);
        }

        private void Impression_onDataReady(IronSourceImpressionData impressionData)
        {
            Adjust.TrackIronsourceRevenue(impressionData);
            TrackRevenueOnFirebase(impressionData);
        }

        private void TrackRevenueOnFirebase(IronSourceImpressionData impressionData)
        {
            Firebase.Analytics.Parameter[] AdParameters = {
                 new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
                  new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
                  new Firebase.Analytics.Parameter("ad_unit_name", impressionData.adUnit),
                new Firebase.Analytics.Parameter("ad_format", impressionData.instanceName),
                  new Firebase.Analytics.Parameter("currency","USD"),
                new Firebase.Analytics.Parameter("value", (double)impressionData.revenue)
            };
            FirebaseAnalytics.Instance.PushEvent("ad_impression", AdParameters);

        }

        private void PostInit()
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
            IronSource.Agent.displayBanner();
        }
        private void Banner_onLoadFailed(IronSourceError obj)
        {
            if (!AdsController.Instance.HasBanner && AdsController.Instance.HasInternet)
            {
                LoadBanner();
            }
        }
        private void LoadBanner()
        {
            if (!Initilized)
                return;

            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        }
        public void ShowBanner()
        {
            if (!AdsController.Instance.HasBanner && AdsController.Instance.HasInternet && Initilized)
            {
                FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
                IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            }
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
            LoadInter();
            AdsController.Instance.onInterClosed?.Invoke();
            AdsController.Instance.onInterClosed = null;
            AdsController.Instance.SetInterval(AdsController.AdType.INTER);
            AdsController.Instance.IsShowingAd = false;
        }

        private void Inter_onAdOpened(IronSourceAdInfo obj)
        {
            Debug.Log("Inter opened");
        }

        private void Inter_onShowSucceeded(IronSourceAdInfo obj)
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.INTER_SHOW);
        }

        private void Inter_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            Debug.Log("Inter show failed with details: " + arg1);
            AdsController.Instance.IsShowingAd = false;
            LoadInter();
        }

        private void Inter_onReady(IronSourceAdInfo obj)
        {
            Debug.Log("Inter ready");
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
        }

        private void Inter_onLoadFailed(IronSourceError obj)
        {
            LoadInter();
        }

        public void LoadInter()
        {
            if (!Initilized || !AdsController.Instance.HasInternet)
                return;

            IronSource.Agent.loadInterstitial();
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
        }
        public void ShowInter()
        {
            if (!AdsController.Instance.IsShowingAd)
            {
                Debug.LogWarning("Another ad is being displayed");
                return;
            }
            AdsController.Instance.IsShowingAd = true;
            IronSource.Agent.showInterstitial();
        }
        #endregion
        #region Reward
        private void Reward_onReward(IronSourcePlacement arg1, IronSourceAdInfo arg2)
        {
            AdsController.Instance.onRewardClosed.Invoke(true);
            switch (AdsController.Instance.rewardType)
            {
                default:
                    FirebaseAnalytics.Instance.PushEvent("");
                    break;
            }
        }

        private void Reward_onClosed(IronSourceAdInfo obj)
        {
            AdsController.Instance.IsShowingAd = false;
        }

        private void Reward_onOpened(IronSourceAdInfo obj)
        {
            Debug.Log("reward ad opened");
        }

        private void Reward_onShowFailed(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            Debug.Log("reward ad show failed");
            LoadReward();
            AdsController.Instance.IsShowingAd = false;
            AdsController.Instance.onRewardClosed.Invoke(false);
        }

        private void Reward_onReady(IronSourceAdInfo obj)
        {
            Debug.Log("reward ad ready");
        }

        private void Reward_onLoadFailed(IronSourceError obj)
        {
            Debug.Log("reward ad load failed");
            LoadReward();
        }

        private void Reward_onAvailable(IronSourceAdInfo obj)
        {
            Debug.Log("reward ad available");
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
        }

        private void Reward_onUnavailable()
        {
            Debug.Log("reward ad unavailable");
        }

        public void LoadReward()
        {
            if (!Initilized && AdsController.Instance.HasInternet)
                return;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            IronSource.Agent.loadRewardedVideo();
        }
        public void ShowReward()
        {
            if (!AdsController.Instance.IsShowingAd)
            {
                Debug.LogWarning("Another ad is being displayed");
                return;
            }
            if (!IronSource.Agent.isRewardedVideoAvailable())
            {
                LoadReward();
                Debug.LogWarning("Reward ad is not ready yet");
                return;
            }
            AdsController.Instance.IsShowingAd = true;
            IronSource.Agent.showRewardedVideo();
        }
        #endregion
    }
}