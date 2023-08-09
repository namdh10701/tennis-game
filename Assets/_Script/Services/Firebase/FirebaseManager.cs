
using UnityEngine;
using Common;
using Services.FirebaseService.Remote;
using Services.FirebaseService.Analytics;
using Services.FirebaseService.Crashlytics;
using Firebase;
using Firebase.Extensions;

namespace Services.FirebaseService
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        public FirebaseRemote FirebaseRemote { get; private set; }
        public FirebaseCrashlytics FirebaseCrashlytics { get; private set; }
        public FirebaseAnalytics FirebaseAnalytics { get; private set; }
        public bool FirebaseInitialized { get; private set; }


        public RemoteVariableCollection RemoteVariableCollection = null;
        protected override void Awake()
        {
            base.Awake();
            FirebaseRemote = GetComponent<FirebaseRemote>();
            FirebaseCrashlytics = GetComponent<FirebaseCrashlytics>();
            FirebaseAnalytics = GetComponent<FirebaseAnalytics>();
        }

        public void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    var app = FirebaseApp.DefaultInstance;
                    FirebaseCrashlytics?.Init();
                    FirebaseAnalytics?.Init();
                    FirebaseRemote?.Init(RemoteVariableCollection);
                    FirebaseInitialized = true;
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + task.Result);
                }
            });
        }
    }
}
