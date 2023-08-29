using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Audio;
using Phoenix;
using Monetization.Ads;

namespace Gameplay
{
    public class MatchSceneManager : MonoBehaviour
    {
        public AudioAsset AudioAsset;
        private GameManager _gameManager;
        [SerializeField] private MatchSceneUI _matchSceneUI;
        [SerializeField] private MatchManager _matchManager;
        private MatchSetting _matchSetting;
        private MatchEvent _matchEvent;
        private MatchData _matchData;
        private MyRemoteVariableCollection _remoteVariables;
        [SerializeField] private SceneTransition _sceneTransition;

        [SerializeField] private TextMeshProUGUI countdowntext;
        private void Start()
        {
            AudioController.Instance.CrossfadeMusic(AudioAsset.MatchSceneBGM, .3f);
        }
        public void BackToHome()
        {
            //ToDo; Xem lại logic chỗ này
            _gameManager.ResetMatchSetting();
            Time.timeScale = 1.0f;
            _sceneTransition.ChangeScene("MenuScene");
        }

        private void Awake()
        {
            //ToDo; Xem lại logic Init
            InitGameSetting();
            _matchEvent = new MatchEvent();
            _matchData = new MatchData(_matchSetting);
            _matchManager.Init(_matchEvent, _matchData);
            _matchSceneUI.Init(_matchData, _matchManager, this);
            InvokeRepeating("HideBanner", .5f, .1f);
        }
        public void HideBanner()
        {
            if (!RemoteVariableManager.Instance.MyRemoteVariables.IsBannerOnMatchScene.Value)
            {
                AdsController.Instance.ToggleBanner(false);
            }
        }


        private void InitGameSetting()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            _gameManager = gameManager;
            _matchSetting = gameManager.MatchSetting;
        }
    }
}