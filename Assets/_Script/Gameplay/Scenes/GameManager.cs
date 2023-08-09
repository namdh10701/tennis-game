using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public SettingManager SettingManager = new SettingManager();
        public GameDataManager GameDataManager = new GameDataManager();
        public RemoteVariableManager RemoteVariableManager = new RemoteVariableManager();

        public MatchSetting MatchSetting = new MatchSetting();

        [SerializeField] private FirebaseManager _firebaseManager;
        private void Awake()
        {
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
            _firebaseManager.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
            _firebaseManager.Init();

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