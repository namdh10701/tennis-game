using GoogleMobileAds.Api;
using Services.Adjust;
using Services.FirebaseService.Analytics;
using System;
using System.Collections;
using UnityEngine;
using Enviroments;
using Common;
using GoogleMobileAds.Common;
using Monetization.Ads.UI;

namespace Monetization.Ads
{
    public class AdmobAds : MonoBehaviour
    {
        private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
        private DateTime appOpenExpireTime;

        private AppOpenAd appOpenAd;
        public bool _isRequestingAd = false;
        public bool _initilized = false;

        [SerializeField] private string _admobId = "";
        [SerializeField] private string _appOpenAdId = "";
        [SerializeField] private string _nativeAdId1 = "";
        [SerializeField] private string _nativeAdId2 = "";

        public void Init()
        {
            MobileAds.Initialize(HandleInitCompleteAction);
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                string appOpenAdTestId = "ca-app-pub-3940256099942544/3419835294";
                string nativeAdTestId = "ca-app-pub-3940256099942544/2247696110";
                _nativeAdId1 = nativeAdTestId;
                _nativeAdId2 = nativeAdTestId;
                _appOpenAdId = appOpenAdTestId;
            }
        }
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            _initilized = true;
            LoadAppOpenAd();
            LoadNativeAd();
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

        public void RegisterNativePanel()
        {
            throw new NotImplementedException();
        }

        public void LoadAppOpenAd()
        {
            if (!_initilized || !AdsController.Instance.HasInternet)
                return;
            if (_isRequestingAd)
                return;

            if (appOpenAd != null)
            {
                DestroyAppOpenAd();
            }
            void DestroyAppOpenAd()
            {
                if (appOpenAd != null)
                {
                    appOpenAd.Destroy();
                    appOpenAd = null;
                }
            }
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            _isRequestingAd = true;
            AppOpenAd.Load(_appOpenAdId, new AdRequest(),
                (AppOpenAd ad, LoadAdError loadError) => HandleLoadedAppOpenAd(ad, loadError)
            );

            void HandleLoadedAppOpenAd(AppOpenAd ad, LoadAdError loadError)
            {
                _isRequestingAd = false;
                if (loadError != null || ad == null)
                    return;
                appOpenAd = ad;
                appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

                HandleOpenAdOpened(ad);
                HandleOpenAdClosed(ad);
                HandleOpenAdShowFailed(ad);
                HandleAdPaid(ad);

                void HandleOpenAdOpened(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentOpened += () =>
                    {
                        _isRequestingAd = false;
                        AdsController.Instance.IsShowingAd = true;
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
                            AdsController.Instance.SetInterval(AdsController.AdType.OPEN);
                            LoadAppOpenAd();
                        });
                        AdsController.Instance.IsShowingAd = false;
                    };
                }
                void HandleOpenAdShowFailed(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentFailed += (AdError error) => LoadAppOpenAd();
                }
            }
        }

        public void ShowAppOpenAd()
        {
            if (!IsAppOpenAdAvailable)
            {
                return;
            }
            AdsController.Instance.IsShowingAd = true;
            appOpenAd.Show();
        }
        #endregion
        #region Native

        private void LoadNativeAd()
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST);
            AdLoader adLoader = new AdLoader.Builder(_nativeAdId1).ForNativeAd().Build();
            adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
            adLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
            adLoader.LoadAd(new AdRequest());
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.Log(e.LoadAdError);
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
        {
            AdsController.Instance.OnNativeAdLoaded(e.nativeAd);
            e.nativeAd.OnPaidEvent += NativeAd_OnPaidEvent;
            FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
        }

        private void NativeAd_OnPaidEvent(object sender, AdValueEventArgs e)
        {
            Adjust.TrackAdmobRevenue(e.AdValue);
            FirebaseAnalytics.TrackAdmobRevenue(e.AdValue);
        }
        #endregion

    }
}