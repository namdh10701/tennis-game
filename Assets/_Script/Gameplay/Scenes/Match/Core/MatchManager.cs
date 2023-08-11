using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using static Gameplay.MatchEvent;

namespace Gameplay
{
    //ToDo: HandleRevive here

    public class MatchManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private MatchData _matchData;
        private GameDataManager _gameDataManager;
        [SerializeField] private MatchSceneUI _sceneUI;

        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private TextManager _textManager;
        [SerializeField] private DifficultyManager _difficultyManager;
        [SerializeField] private BackgroundManager _backgroundManager;

        [SerializeField] private Player _player;
        [SerializeField] private CPU _cpu;
        [SerializeField] private Ball _ball;
        [SerializeField] private Transform _ballAimingTarget;
        [SerializeField] private TextMeshProUGUI _countdownText;


        private float _remainingTimeToStart;
        public void Init(MatchEvent matchEvent, MatchData matchData, GameDataManager gameDataManager)
        {
            _gameDataManager = gameDataManager;
            _matchEvent = matchEvent;
            _matchData = matchData;

            InitManagers();
            InitInteractiveObjects();

            void InitManagers()
            {

                _timeManager.Init(_matchEvent, _matchData);
                _scoreManager.Init(_matchData);
                _backgroundManager.Init();
                _difficultyManager.Init(_matchData);
                _textManager.Init(_matchData);
            }
            void InitInteractiveObjects()
            {
                _player.Init(_matchEvent, matchData.MatchSettings);
                _cpu.Init(_matchEvent, matchData.MatchSettings, _ball);
                _ball.Init(_matchEvent, _ballAimingTarget, matchData.MatchSettings, this);
            }

            StartCoroutine(CountdownCoroutine());
        }

        public IEnumerator CountdownCoroutine()
        {
            _countdownText.gameObject.SetActive(true);
            _remainingTimeToStart = 3;
            _countdownText.text = _remainingTimeToStart.ToString();
            while (_remainingTimeToStart > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                _remainingTimeToStart -= 1;
                _countdownText.text = _remainingTimeToStart.ToString();
            }
            _countdownText.gameObject.SetActive(false);
            _cpu.ServeBall();
        }

        private void StartMatch()
        {
            _matchEvent.CurrentState = MatchState.PLAYING;
            _difficultyManager.ApplyDifficulty();
        }

        private void EndMatch()
        {
            StartCoroutine(EndMatchCoroutine());
        }

        private IEnumerator EndMatchCoroutine()
        {

            _matchEvent.CurrentState = MatchState.STOPPED;
            if (_matchData.Score > _gameDataManager.GameDatas.HighScore)
            {
                _gameDataManager.GameDatas.HighScore = _matchData.Score;
                _gameDataManager.SaveDatas();
            }
            yield return new WaitForSecondsRealtime(1);
            _sceneUI.OpenGameOverPanel();
        }

        public void OnBallHitSuccess(Side side)
        {
            if (side == Side.Player)
            {
                _textManager.DisplayText();
                _scoreManager.Increase();

                //ToDo: get from remote variable
                if (_matchData.Score % 10 == 0)
                {
                    _difficultyManager.IncreaseDifficulty();
                    _backgroundManager.ChangeBackground();
                }

            }
        }

        public void RestartMatch()
        {
            _matchData.ResetMatchData();
            _matchEvent.CurrentState = MatchState.PRE_START;
            StartCoroutine(CountdownCoroutine());
        }

        public void Home()
        {

        }


        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.BallServed += () => StartMatch();
                _matchEvent.MatchEnd += () => EndMatch();
            }
        }
        private void OnDisable()
        {
            _matchEvent.MatchStart += () => StartMatch();
            _matchEvent.MatchEnd += () => EndMatch();
        }

    }
}