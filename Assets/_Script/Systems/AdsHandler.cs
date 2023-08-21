using Common;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Monetization.Ads;
using UnityEngine;

public class AdsHandler : MonoBehaviour
{
    private void Start()
    {
        AdsController.Instance.SetRemoveAds(PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) == 1);
        AdsController.Instance.Init();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        if (PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) != 1)
        {
            InvokeRepeating("TurnBannerOn", 10, 10);
            InvokeRepeating("LoadNativeAd", 60, 60);
        }
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