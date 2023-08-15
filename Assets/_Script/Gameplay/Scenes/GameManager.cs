using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
using Enviroments;
using Monetization.Ads;
using Phoenix.Gameplay.Vibration;
using com.adjust.sdk;

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
                Adjust.setEnabled(false);
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
            AdsController.Instance.Init();
            InvokeRepeating("TurnBannerOn", 10, 10);
            _firebaseManager.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
        }
        private void TurnBannerOn()
        {
            AdsController.Instance.ShowBanner();
        }

        private void SaveRemoteVariable()
        {
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
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                AdsController.Instance.ShowAppOpenAd();
            }
        }
    }
}