using UnityEngine;

using Common;
using System;
using Firebase.Analytics;
using Enviroments;
using System.Security.Cryptography;
using GoogleMobileAds.Api;

namespace Services.FirebaseService.Analytics
{
    public class FirebaseAnalytics : Singleton<FirebaseAnalytics>
    {
        public void Init()
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Firebase.Analytics.FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
            }
        }

        public void PushEvent(string valueLog, params Parameter[] parameters)
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                if (FirebaseManager.Instance.FirebaseInitialized)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(valueLog, parameters);
                }
            }
            else
            {
                //Debug.Log("event: " + valueLog + "\n" + parameters);
            }
        }

        public void PushEvent(string eventName)
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                if (FirebaseManager.Instance.FirebaseInitialized)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
           
                    Debug.Log("event: " + eventName + " logged");
                }
            }
            else
            {
                Debug.Log("event: " + eventName);
            }
        }

        public static void TrackAdmobRevenue(AdValue adValue)
        {
            Parameter[] LTVParameters = {
                new Parameter("ad_platform", "adMob"),
                new Parameter("ad_source", "adMob"),
                new Parameter("value", adValue.Value / 1000000f),
                new Parameter("currency", adValue.CurrencyCode),
                new Parameter("precision", (int)adValue.Precision)
             };
            Instance.PushEvent("ad_impression", LTVParameters);
        }

        public static void TrackIronSourceRevenue(IronSourceImpressionData impressionData)
        {
            Parameter[] AdParameters = {
               new Parameter("ad_platform", "ironSource"),
                new Parameter("ad_source", impressionData.adNetwork),
                new Parameter("ad_unit_name", impressionData.adUnit),
                new Parameter("ad_format", impressionData.instanceName),
                new Parameter("currency","USD"),
                new Parameter("value", (double)impressionData.revenue)
            };
            Instance.PushEvent("ad_impression", AdParameters);
        }
    }
}
