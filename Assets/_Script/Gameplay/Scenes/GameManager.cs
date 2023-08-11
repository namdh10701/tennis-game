using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
using Enviroments;
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

            Debug.Log("game manager awake");
            GameManager instance = FindObjectOfType<GameManager>();
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            SettingManager.LoadSettings();

            GameDataManager.LoadDatas();
            RemoteVariableManager.LoadDatas();

            _firebaseManager.RemoteVariableCollection = RemoteVariableManager.MyRemoteVariables;
            _firebaseManager.Init();

        }

        private void Start()
        {
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
    }
}