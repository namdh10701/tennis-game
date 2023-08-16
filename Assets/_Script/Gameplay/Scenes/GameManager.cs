using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
using Enviroments;
using Monetization.Ads;
using Phoenix.Gameplay.Vibration;
using Audio;
using Common;
using System.Collections;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public SettingManager SettingManager = new();
        public GameDataManager GameDataManager = new();
        public RemoteVariableManager RemoteVariableManager = new();

        public MatchSetting MatchSetting = new MatchSetting();

        [SerializeField] private FirebaseManager _firebaseManager;
        [SerializeField] private Enviroment.Env env;
        private void Awake()
        {
            Enviroment.ENV = env;
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                Debug.Log(Enviroment.ENV);
                Debug.unityLogger.logEnabled = false;
            }
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                //Adjust.setEnabled(false);
            }
            Application.targetFrameRate = 60;
            SettingManager.LoadSettings();
            Vibration.SetState(SettingManager.GameSettings.IsVibrationOn);

            GameDataManager.LoadDatas();
            RemoteVariableManager.LoadDatas();
            _firebaseManager.RemoteVariableCollection = RemoteVariableManager.MyRemoteVariables;
            _firebaseManager.Init();



        }

        private void Start()
        {
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            bool isRemovedAd = PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) == 1;
            if (isRemovedAd)
            {
                AdsController.Instance.OnRemoveAds();
            }
            AdsController.Instance.Init();
            AudioController.Instance.Init(SettingManager.GameSettings.IsMusicOn, SettingManager.GameSettings.IsSoundOn);

            InvokeRepeating("TurnBannerOn", 10, 10);
            _firebaseManager.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
        }
        private void TurnBannerOn()
        {
            AdsController.Instance.ShowBanner();
        }

        private void SaveRemoteVariable()
        {
            Debug.Log("remotevariable saved");
            RemoteVariableManager.SaveDatas();
        }

        private void OnEnable()
        {
            if (_firebaseManager != null && _firebaseManager.FirebaseRemote != null)
            {
                _firebaseManager.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
            }
        }

        private void OnDisable()
        {
            if (_firebaseManager != null && _firebaseManager.FirebaseRemote != null)
            {
                _firebaseManager.FirebaseRemote.OnFetchedCompleted -= () => SaveRemoteVariable();
            }
        }
        public void ResetMatchSetting()
        {
            MatchSetting = new MatchSetting();

        }
        public void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            Debug.Log("App State is " + state);

            // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (state == AppState.Foreground)
                {
                    AdsController.Instance.ShowAppOpenAd();
                }
            });
        }

    

        private void Update()
        {
            //Debug.Log(RemoteVariableManager.MyRemoteVariables.TimescaleStep.Value);
        }
    }
}