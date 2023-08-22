using Audio;
using Common;
using Phoenix.Gameplay.Vibration;
using Services.FirebaseService.Analytics;
using System.Collections;
using TMPro;
using UnityEngine;
using static Gameplay.MatchEvent;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    public class MatchManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private MatchData _matchData;
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

        [SerializeField] private Transform _playerSpawnPoint;


        [SerializeField] private GameObject _playerTennis;
        [SerializeField] private GameObject _playerBaseball;
        [SerializeField] private GameObject _playerFootball;
        [SerializeField] private GameObject _playerVolleyball;
        private int _retryCount;
        private int _originalIncremental;
        private float _remainingTimeToStart;

        public void Init(MatchEvent matchEvent, MatchData matchData)
        {
            _matchEvent = matchEvent;
            _matchData = matchData;
            _retryCount = 0;
            _originalIncremental = _matchData.MatchSettings.Incremental;
            RemoteVariable remoteVariables = RemoteVariable.Convert(RemoteVariableManager.Instance.MyRemoteVariables);

            _timeManager.Init(_matchData);
            _scoreManager.Init(_matchData);
            _backgroundManager.Init(remoteVariables.BackgroundColorOrder);
            _difficultyManager.Init(_matchData, remoteVariables.IncrementalStep, remoteVariables.TimescaleStep, this, remoteVariables.MaxIncrement);
            _textManager.Init(_matchData);

            bool isMatchReversed = SettingManager.Instance.GameSettings.IsReversed;
            if (isMatchReversed)
            {
                Quaternion newRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z + 180);
                Camera.main.transform.rotation = newRotation;
            }
            switch (matchData.MatchSettings.SportName)
            {
                case Sport.TENNIS:
                    _player = Instantiate(_playerTennis, _playerSpawnPoint, true).GetComponent<Player>();
                    break;
                case Sport.BASEBALL:
                    _player = Instantiate(_playerBaseball, _playerSpawnPoint, true).GetComponent<Player>();
                    break;
                case Sport.FOOTBALL:
                    _player = Instantiate(_playerFootball, _playerSpawnPoint, true).GetComponent<Player>();
                    break;
                case Sport.VOLLEYBALL:
                    _player = Instantiate(_playerVolleyball, _playerSpawnPoint, true).GetComponent<Player>();
                    break;
            }
            _player.Init(_matchEvent, matchData.MatchSettings, isMatchReversed);
            _cpu.Init(_matchEvent, matchData.MatchSettings, _ball, isMatchReversed, remoteVariables.MaxIncrement);
            _ball.Init(_matchEvent, matchData.MatchSettings, isMatchReversed);


            PrepareMatch();
            StartCoroutine(CountdownCoroutine());
        }

        private void PrepareMatch()
        {
            _matchEvent.CurrentState = MatchState.PRE_START;
            _backgroundManager.Prepare();
            _matchData.ResetMatchData();
            _textManager.Prepare();
            _timeManager.Prepare();
            _scoreManager.Prepare();
            _backgroundManager.Prepare();
            _cpu.Prepare();
            _ball.Prepare();
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
            _timeManager.StartTime();
        }

        private void EndMatch()
        {
            _timeManager.StopTime();
            _matchEvent.CurrentState = MatchState.STOPPED;
            if (_matchData.Score > GameDataManager.Instance.GameDatas.HighScore)
            {
                GameDataManager.Instance.GameDatas.HighScore = _matchData.Score;
                GameDataManager.Instance.SaveDatas();
                Leaderboard.Instance.ReportScore(_matchData.Score);
            }
            AudioController.Instance.PlaySound("game_over");

            if (_retryCount == 0)
            {
                _sceneUI.OpenRevivePanel();
            }
            else
            {
                _sceneUI.OpenGameOverPanel();
            }
        }

        public void RestartMatch()
        {
            _matchData.MatchSettings.Incremental = _originalIncremental;
            Time.timeScale = 1;
            _retryCount = 0;
            PrepareMatch();
            StartCoroutine(CountdownCoroutine());
        }

        public void Revive()
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.REVIVE_COMPLETE);
            _matchEvent.CurrentState = MatchState.PRE_START;
            _cpu.Prepare();
            _ball.Prepare();
            _player.Prepare();
            _retryCount++;
            StartCoroutine(CountdownCoroutine());
        }

        public void OnBallHitSuccess(Side side)
        {
            if (side == Side.Player)
            {
                AudioController.Instance.PlaySound("ball_hit");
                _textManager.DisplayText();
                _scoreManager.Increase();
                _difficultyManager.UpdateDifficulty();
                Vibration.Vibrate(100);
            }
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

        public void OnDifficultyChange()
        {
            _backgroundManager.UpdateBackground();
            _cpu.UpdateCat();
            _textManager.UpdateAvailableText();
        }
    }
}