using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class MatchSceneManager : MonoBehaviour
    {
        [SerializeField] private MatchSceneUI _matchSceneUI;
        [SerializeField] private MatchManager _matchManager;
        private MatchSetting _matchSetting;
        private MatchEvent _matchEvent;
        private MatchData _matchData;

        [SerializeField] private TextMeshProUGUI countdowntext;
        private void Awake()
        {
            InitGameSetting();

            _matchEvent = new MatchEvent();
            _matchData = new MatchData(_matchSetting);
            _matchManager.Init(_matchEvent, _matchData);
            _matchSceneUI.Init(_matchEvent, _matchData);
        }

        private void InitGameSetting()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            _matchSetting = gameManager.MatchSetting;
        }
        private void Start()
        {
            _matchManager.StartCountdown();
        }


    }
}