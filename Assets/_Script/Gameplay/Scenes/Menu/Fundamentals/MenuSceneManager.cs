using UnityEngine;
using UnityEngine.SceneManagement;
using static Gameplay.MatchSetting;

namespace Gameplay
{

    //Handle UI Interact here
    public class MenuSceneManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private SettingPanel settingPopup;

        [SerializeField] private MenuSceneUI _menuSceneUI;
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        private void Start()
        {
            settingPopup.Init(_gameManager.SettingManager);
            _menuSceneUI.Init(this, _gameManager.MatchSetting, _gameManager.GameDataManager);
        }



        public void StartMatch(Sport sportName)
        {
            _gameManager.MatchSetting.SportName = sportName;
            SceneManager.LoadScene("MatchScene");
        }
    }
}