using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class MatchSceneManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private MatchSceneUI _matchSceneUI;
        [SerializeField] private MatchManager _matchManager;
        private MatchSetting _matchSetting;
        private MatchEvent _matchEvent;
        private MatchData _matchData;
        private GameDataManager _gameDataManager;
        private MyRemoteVariableCollection _remoteVariables;

        [SerializeField] private TextMeshProUGUI countdowntext;

        public void BackToHome()
        {
            //ToDo; Xem lại logic chỗ này
            _gameManager.ResetMatchSetting();
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MenuScene");
        }

        private void Awake()
        {

            //ToDo; Xem lại logic Init
            InitGameSetting();
            _matchEvent = new MatchEvent();
            _matchData = new MatchData(_matchSetting);
            _matchManager.Init(_matchEvent, _matchData, _gameDataManager, _remoteVariables);
            _matchSceneUI.Init(_matchData, _gameDataManager.GameDatas, _matchManager, this);
        }

        private void InitGameSetting()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            _gameManager = gameManager;
            _matchSetting = gameManager.MatchSetting;
            _gameDataManager = gameManager.GameDataManager;
            _remoteVariables = gameManager.RemoteVariableManager.MyRemoteVariables;

        }
    }
}