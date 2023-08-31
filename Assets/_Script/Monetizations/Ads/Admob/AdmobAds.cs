using GoogleMobileAds.Api;
using Services.Adjust;
using Services.FirebaseService.Analytics;
using System;
using System.Collections;
using UnityEngine;
using Enviroments;
using Common;
using GoogleMobileAds.Common;

namespace Monetization.Ads
{
    public class AdmobAds : MonoBehaviour
    {
        private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
        private DateTime appOpenExpireTime;

        private AppOpenAd appOpenAd;

        private bool _initilized = false;

        [SerializeField] private string _appOpenAdId = "";
        [SerializeField] private string _nativeAdId1 = "";
        [SerializeField] private string _nativeAdId2 = "";

        public void Init()
        {

            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                string appOpenAdTestId = "ca-app-pub-3940256099942544/3419835294";
                string nativeAdTestId = "ca-app-pub-3940256099942544/2247696110";
                _nativeAdId1 = nativeAdTestId;
                _nativeAdId2 = nativeAdTestId;
                _appOpenAdId = appOpenAdTestId;
            }
            MobileAds.Initialize(HandleInitCompleteAction);
        }
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            _initilized = true;
            AdsLogger.Log($"Admob initialized", AdsController.AdType.OPEN);
            LoadAppOpenAd();
            LoadNativeAds();
        }
        void HandleAdPaid(AppOpenAd ad)
        {
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Adjust.TrackAdmobRevenue(adValue);
                FirebaseAnalytics.TrackAdmobRevenue(adValue);
            };
        }
        #region APPOPEN
        private bool _isRequestingAppOpenAd = false;
        public bool IsAppOpenAdAvailable
        {
            get
            {
                bool ret = appOpenAd != null
                        && appOpenAd.CanShowAd()
                        && DateTime.Now < appOpenExpireTime;
                if (!ret)
                {
                    LoadAppOpenAd();
                }
                return ret;
            }
        }

        public void LoadAppOpenAd()
        {
            if (!_initilized || !AdsController.Instance.HasInternet)
                return;
            if (_isRequestingAppOpenAd)
                return;

            if (appOpenAd != null)
            {
                DestroyAppOpenAd();
            }
            AdsLogger.Log($"Requesting", AdsController.AdType.OPEN);
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            _isRequestingAppOpenAd = true;
            AppOpenAd.Load(_appOpenAdId, new AdRequest(),
                (AppOpenAd ad, LoadAdError loadError) => HandleLoadedAppOpenAd(ad, loadError)
            );

            void HandleLoadedAppOpenAd(AppOpenAd ad, LoadAdError loadError)
            {
                _isRequestingAppOpenAd = false;
                if (loadError != null || ad == null)
                    return;
                appOpenAd = ad;
                appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

                AdsLogger.Log($"Loaded", AdsController.AdType.OPEN);
                if (loadError != null)
                {
                    AdsLogger.Log($"Error: {loadError.ToString()}", AdsController.AdType.OPEN);
                }
                HandleOpenAdOpened(ad);
                HandleOpenAdClosed(ad);
                HandleOpenAdShowFailed(ad);
                HandleAdPaid(ad);
                void HandleOpenAdOpened(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentOpened += () =>
                    {

                        _isRequestingAppOpenAd = false;
                        AdsController.Instance.IsShowingOpenAd = true;
                        FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
                        FirebaseAnalytics.Instance.PushEvent(Constant.APP_OPEN_SHOW);
                    };
                }
                void HandleOpenAdClosed(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            AdsIntervalValidator.SetInterval(AdsController.AdType.OPEN);
                            LoadAppOpenAd();
                        });
                        AdsController.Instance.IsShowingOpenAd = false;
                    };
                }
                void HandleOpenAdShowFailed(AppOpenAd ad)
                {
                    AdsController.Instance.IsShowingOpenAd = false;
                    ad.OnAdFullScreenContentFailed += (AdError error) => LoadAppOpenAd();
                }
            }
        }
        void DestroyAppOpenAd()
        {
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }
        }
        public void ShowAppOpenAd()
        {
            StartCoroutine(PassframeShowAd());
        }

        public IEnumerator PassframeShowAd()
        {
            yield return new WaitForSeconds(.05f);
            if (AdsController.Instance.RewardedAdJustClose)
            {
                AdsController.Instance.RewardedAdJustClose = false;
                yield break;
            }
            if (AdsController.Instance.IsShowingInterAd || AdsController.Instance.IsShowingOpenAd)
            {
                yield break;
            }
            AdsLogger.Log("All conditions pass \n Show", AdsController.AdType.OPEN);
            appOpenAd.Show();
        }

        #endregion
        #region Native
        private bool _isNativeAdKey1Requesting = false;
        private bool _isNativeAdKey2Requesting = false;

        public void LoadNativeAds()
        {
            if (!_initilized)
            {
                return;
            }

            if (!_isNativeAdKey1Requesting)
            {
                _isNativeAdKey1Requesting = true;
                FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
                AdLoader adLoader1 = new AdLoader.Builder(_nativeAdId1).ForNativeAd().Build();
                adLoader1.OnNativeAdLoaded += (sender, e) => HandleNativeAdLoaded(sender, e, _nativeAdId1);
                adLoader1.OnAdFailedToLoad += (sender, e) => HandleAdFailedToLoad(sender, e, _nativeAdId1);
                adLoader1.OnNativeAdClicked += (sender, e) =>
                {
                    FirebaseAnalytics.Instance.PushEvent(Constant.CLICK_NATIVE_AD);
                };
                AdsLogger.Log($"Requesting using key 1", AdsController.AdType.NATIVE);
                adLoader1.LoadAd(new AdRequest());
            }
            if (!_isNativeAdKey2Requesting)
            {
                _isNativeAdKey2Requesting = true;
                FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
                AdLoader adLoader2 = new AdLoader.Builder(_nativeAdId2).ForNativeAd().Build();
                adLoader2.OnNativeAdLoaded += (sender, e) => HandleNativeAdLoaded(sender, e, _nativeAdId2);
                adLoader2.OnAdFailedToLoad += (sender, e) => HandleAdFailedToLoad(sender, e, _nativeAdId2);
                adLoader2.OnNativeAdClicked += (sender, e) =>
                {
                    FirebaseAnalytics.Instance.PushEvent(Constant.CLICK_NATIVE_AD);
                };
                AdsLogger.Log($"Requesting using key 2", AdsController.AdType.NATIVE);
                adLoader2.LoadAd(new AdRequest());
            }
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e, string key)
        {
            if (key == _nativeAdId1)
                _isNativeAdKey1Requesting = false;
            else
                _isNativeAdKey2Requesting = false;
            AdsLogger.Log($"Load failed {e.LoadAdError}", AdsController.AdType.NATIVE);
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e, string key)
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
            AdsController.Instance.OnNativeAdLoaded(e.nativeAd);
            e.nativeAd.OnPaidEvent += NativeAd_OnPaidEvent;
            if (key == _nativeAdId1)
                _isNativeAdKey1Requesting = false;
            else
                _isNativeAdKey2Requesting = false;
        }

        private void NativeAd_OnPaidEvent(object sender, AdValueEventArgs e)
        {
            Adjust.TrackAdmobRevenue(e.AdValue);
            FirebaseAnalytics.TrackAdmobRevenue(e.AdValue);
        }
        #endregion

    }
}