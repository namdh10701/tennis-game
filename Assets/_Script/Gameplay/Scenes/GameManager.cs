using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
using Enviroments;
using Monetization.Ads;

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
            Application.targetFrameRate = 60;
            SettingManager.LoadSettings();
            GameDataManager.LoadDatas();
            RemoteVariableManager.LoadDatas();

            _firebaseManager.RemoteVariableCollection = RemoteVariableManager.MyRemoteVariables;
            _firebaseManager.Init();
        }

        private void Start()
        {
            AdsController.Instance.Init();
            _firebaseManager.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
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
    }
}