using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Gameplay.MatchSetting;

namespace Gameplay
{

    //Handle UI Interact here
    public class MenuSceneManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private SettingPanel settingPanel;

        [SerializeField] private MenuSceneUI _menuSceneUI;
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        private void Start()
        {
            settingPanel.Init(_gameManager.SettingManager);
            _menuSceneUI.Init(this, _gameManager.MatchSetting, _gameManager.GameDataManager);
        }



        public void StartMatch(Sport sportName)
        {
            _gameManager.MatchSetting.SportName = sportName;
            if (_gameManager.MatchSetting.Incremental > _gameManager.GameDataManager.GameDatas.UnlockedIncremental)
            {
                _menuSceneUI.OpenNotUnlockedIncremental();
            }
            else
            {
                SceneManager.LoadScene("MatchScene");
            }
        }

        public void OpenSetting()
        {
            settingPanel.Open(true);
        }
    }
}