using UnityEngine;

using Common;
using System;
using Firebase.Analytics;
using Enviroments;
using System.Security.Cryptography;

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
            else
            {
                Debug.Log("Firebase Analytics initilized");
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
                Debug.Log("event: " + valueLog + "\n" + parameters);
            }
        }

        public void PushEvent(string eventName)
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                if (FirebaseManager.Instance.FirebaseInitialized)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
                }
            }
            else
            {
                Debug.Log("event: " + eventName);
            }
        }
    }
}
