using GoogleMobileAds.Api;
using UnityEngine;
using static Monetization.Ads.AdsController;

public static class AdsLogger
{
    public static bool IsDebugBanner;
    public static bool IsDebugInter;
    public static bool IsDebugRewarded;
    public static bool IsDebugOpen;
    public static bool IsDebugNative;

    public static void Log(string logDetails, AdType adType)
    {
        switch (adType)
        {
            case AdType.INTER:
                if (IsDebugBanner)
                {
                    Debug.Log($"<color=green>INTER: {logDetails}</color>");
                }
                break;
            case AdType.BANNER:
                if (IsDebugInter)
                {
                    Debug.Log($"<color=blue>BANNER: {logDetails}</color>");
                }
                break;
            case AdType.REWARD:
                if (IsDebugRewarded)
                {
                    Debug.Log($"<color=yellow>REWARD: {logDetails}</color>");
                }
                break;
            case AdType.OPEN:
                if (IsDebugOpen)
                {
                    Debug.Log($"<color=red>OPEN: {logDetails}</color>");
                }
                break;
            case AdType.NATIVE:
                if (IsDebugNative)
                {
                    Debug.Log($"<color=pink>NATIVE: {logDetails}</color>");
                }
                break;
        }
    }
}