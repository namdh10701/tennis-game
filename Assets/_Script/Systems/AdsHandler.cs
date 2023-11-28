using com.adjust.sdk;
using Common;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Monetization.Ads;
using System.Collections;
using UnityEngine;

public class AdsHandler : MonoBehaviour
{
    public static bool AdRemoved()
    {
        return PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) == 1;
    }
    private void Start()
    {
        AdsController.Instance.OnRemoveAds(AdRemoved());
        AdsController.Instance.Init();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        if (PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) != 1)
        {
            InvokeRepeating("TurnBannerOn", 6, 2);
            InvokeRepeating("LoadNativeAd", 30, 30);
        }

        //30s đầu trong game k có Inter ads
        StartCoroutine(NonInterTime());
    }
    private IEnumerator NonInterTime()
    {
        AdsController.Instance.InterCanceled = true;
        yield return new WaitForSecondsRealtime(30);
        AdsController.Instance.InterCanceled = false;
    }

    private void TurnBannerOn()
    {
        AdsController.Instance.ShowBanner();
    }
    private void LoadNativeAd()
    {
        AdsController.Instance.LoadNativeAds();
    }
    public void OnAppStateChanged(AppState state)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                AdsController.Instance.ShowAppOpenAd();
            }
        });
    }
}