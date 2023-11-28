
using UnityEngine;
using Common;
using Services.FirebaseService.Remote;
using Services.FirebaseService.Analytics;
using Services.FirebaseService.Crashlytics;
using Firebase;
using Firebase.Extensions;

namespace Services.FirebaseService
{
    public class FirebaseManager : SingletonPersistent<FirebaseManager>
    {
        public FirebaseRemote FirebaseRemote { get; private set; }
        public FirebaseCrashlytics FirebaseCrashlytics { get; private set; }
        public FirebaseAnalytics FirebaseAnalytics { get; private set; }
        public bool FirebaseInitialized { get; private set; }

        private DependencyStatus dependencyStatus;

        public RemoteVariableCollection RemoteVariableCollection = null;
        protected override void Awake()
        {
            base.Awake(); dependencyStatus = DependencyStatus.UnavailableUpdating;
            FirebaseRemote = GetComponent<FirebaseRemote>();
            FirebaseCrashlytics = GetComponent<FirebaseCrashlytics>();
            FirebaseAnalytics = GetComponent<FirebaseAnalytics>();
        }

        public void Init()
        {
            dependencyStatus = DependencyStatus.UnavilableMissing;
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                Debug.Log("Finish check here "+ (task.Result == DependencyStatus.Available));

                if (task.Result == DependencyStatus.Available)
                {
                    var app = FirebaseApp.DefaultInstance;
                    FirebaseInitialized = true;
                    Debug.LogWarning("firebase Initialized");
                    FirebaseCrashlytics?.Init();
                    FirebaseAnalytics?.Init();
                    FirebaseRemote?.Init(RemoteVariableCollection);
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
