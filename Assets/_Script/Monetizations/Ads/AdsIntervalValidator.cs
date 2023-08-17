using Common;
using UnityEngine;
using static Monetization.Ads.AdsController;

namespace Monetization.Ads
{
    public static class AdsIntervalValidator
    {
        public static double INTERVAL_INTER_INTER = 20;
        //public static double INTERVAL_OPEN_INTER = 15;
        //public static double INTERVAL_OPEN_OPEN = 15;
        public static double INTERVAL_REWARD_INTER = 20;
        public static void SetInterval(AdType type)
        {
            switch (type)
            {
                case AdType.REWARD:
                    GetSetTimeSave.SetTime(Constant.adInterval_Reward_Inter);
                    break;
                case AdType.INTER:
                    GetSetTimeSave.SetTime(Constant.adInterval_Inter_Inter);
                    GetSetTimeSave.SetTime(Constant.adInterval_OpenAd_Inter);
                    break;
                case AdType.OPEN:
                    break;
            }
        }
        public static bool IsValidInterval(AdType type)
        {
            bool ret = true;
            switch (type)
            {
                case AdType.REWARD:
                    break;
                case AdType.INTER:
                    var timeShowInterAdsBetween_Inter_InterToNow = GetSetTimeSave.GetTimeToNow(Constant.adInterval_Inter_Inter);
                    if (timeShowInterAdsBetween_Inter_InterToNow.TotalSeconds > 0
                        && timeShowInterAdsBetween_Inter_InterToNow.TotalSeconds < INTERVAL_INTER_INTER)
                    {
                        ret = false;
                    }
                    var timeShowInterAdsBetween_Inter_RewardToNow = GetSetTimeSave.GetTimeToNow(Constant.adInterval_Reward_Inter);
                    if (timeShowInterAdsBetween_Inter_RewardToNow.TotalSeconds > 0
                        && timeShowInterAdsBetween_Inter_RewardToNow.TotalSeconds < INTERVAL_REWARD_INTER)
                    {
                        ret = false;
                    }
                    /*var timeShowInterAdsBetween_OpenAds_Inter = GetSetTimeSave.GetTimeToNow(Constant.adInterval_OpenAd_Inter);
                    if (timeShowInterAdsBetween_OpenAds_Inter.TotalSeconds > 0
                        && timeShowInterAdsBetween_OpenAds_Inter.TotalSeconds < INTERVAL_OPEN_INTER)
                    {
                        ret = false;
                    }*/
                    break;
                case AdType.OPEN:
                    /*var timeShowInterAds_OpenAds = GetSetTimeSave.GetTimeToNow(Constant.adInterval_OpenAd_Inter);
                    if (timeShowInterAds_OpenAds.TotalSeconds > 0
                        && timeShowInterAds_OpenAds.TotalSeconds < AdsController.Instance.INTERVAL_OPEN_INTER)
                    {
                        ret = false;
                    }
                    var timeShowOpenAds_OpenAds = GetSetTimeSave.GetTimeToNow(Constant.adInterval_OpenAd_OpenAd);
                    if (timeShowOpenAds_OpenAds.TotalSeconds > 0
                        && timeShowOpenAds_OpenAds.TotalSeconds < AdsController.Instance.INTERVAL_OPEN_OPEN)
                    {
                        ret = false;
                    }*/
                    break;
            }
            if (!ret)
            {
                Debug.LogWarning($"{type} Not valid interval");
            }
            return ret;
        }
    }
}