using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Gameplay.MatchSetting;
using Audio;
using Phoenix;
using Monetization.Ads;

namespace Gameplay
{

    //Handle UI Interact here
    public class MenuSceneManager : MonoBehaviour
    {
        public AudioAsset AudioAsset;
        private GameManager _gameManager;
        [SerializeField] private SettingPanel settingPanel;
        [SerializeField] private MenuSceneUI _menuSceneUI;
        [SerializeField] private SceneTransition _sceneTransition;
        private void Awake()
        {
            AdsController.Instance.ToggleBanner(true);
            _gameManager = FindObjectOfType<GameManager>();
        }
        private void Start()
        {
            Time.timeScale = 1;
            AudioController.Instance.CrossfadeMusic(AudioAsset.MenuSceneBGM, .3f);
            settingPanel.Init();
            _menuSceneUI.Init(this, _gameManager.MatchSetting);
        }



        public void StartMatch(Sport sportName)
        {
            _gameManager.MatchSetting.SportName = sportName;
            if (_gameManager.MatchSetting.Incremental > GameDataManager.Instance.GameDatas.UnlockedIncremental)
            {
                _menuSceneUI.OpenNotUnlockedIncremental();
            }
            else
            {
                _sceneTransition.ChangeScene("MatchScene");
            }
        }

        public void OpenSetting()
        {
            settingPanel.Open(true);
        }
    }
}