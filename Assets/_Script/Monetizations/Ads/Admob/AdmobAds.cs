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
        public bool isRequestingAd = false;
        public bool initilized = false;

        private string admobId = "";
        private string appOpenAdId = "";
        private string nativeAdId1 = "";
        private string nativeAdId2 = "";

        #region UNITY MONOBEHAVIOR METHODS
        public void Init()
        {
            MobileAds.Initialize(HandleInitCompleteAction);
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                string appOpenAdTestId = "ca-app-pub-3940256099942544/3419835294";
                appOpenAdId = appOpenAdTestId;
            }
        }
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            LoadAppOpenAd();
            initilized = true;
        }
        #endregion

        #region APPOPEN ADS
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
            if (!initilized || !AdsController.Instance.HasInternet)
                return;
            if (isRequestingAd)
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
            AppOpenAd.Load(
                appOpenAdId,
                new AdRequest(),
                (AppOpenAd ad, LoadAdError loadError) => HandleLoadedAppOpenAd(ad, loadError)
            );

            void HandleLoadedAppOpenAd(AppOpenAd ad, LoadAdError loadError)
            {
                if (loadError != null || ad == null)
                    return;
                appOpenAd = ad;
                appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    AdsController.Instance.IsShowingAd = true;
                    FirebaseAnalytics.Instance.PushEvent(Constant.AD_REQUEST_SUCCEED);
                    FirebaseAnalytics.Instance.PushEvent(Constant.APP_OPEN_SHOW);
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        AdsController.Instance.SetInterval(AdsController.AdType.OPEN);
                        LoadAppOpenAd();
                    });
                    AdsController.Instance.IsShowingAd = false;
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                    LoadAppOpenAd();
                {
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    Adjust.TrackAdmobRevenue(adValue);
                    TrackRevenueOnFirebase(adValue);
                };

                void TrackRevenueOnFirebase(AdValue adValue)
                {
                    Firebase.Analytics.Parameter[] LTVParameters = {
                        new Firebase.Analytics.Parameter("ad_platform", "adMob"),
                        new Firebase.Analytics.Parameter("ad_source", "adMob"),
                        new Firebase.Analytics.Parameter("value", adValue.Value / 1000000f),
                        new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode),
                        new Firebase.Analytics.Parameter("precision", (int)adValue.Precision)
                    };
                    FirebaseAnalytics.Instance.PushEvent("ad_impression", LTVParameters);
                }
            }
        }


        public void ShowAppOpenAd()
        {
            StartCoroutine(DelayFrameShowAds());
        }
        IEnumerator DelayFrameShowAds()
        {
            yield return new WaitForEndOfFrame();
            AdsController.Instance.IsShowingAd = true;
            appOpenAd.Show();
        }

        #endregion
    }
}