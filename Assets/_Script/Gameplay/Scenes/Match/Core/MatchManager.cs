using System.Collections;
using UnityEngine;

namespace Gameplay
{
    //ToDo: HandleRevive here

    public class MatchManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private MatchData _matchData;

        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private TextManager _textManager;
        [SerializeField] private DifficultyManager _difficultyManager;
        [SerializeField] private BackgroundManager _backgroundManager;

        [SerializeField] private Player _player;
        [SerializeField] private CPU _cpu;
        [SerializeField] private Ball _ball;
        [SerializeField] private Transform _ballAimingTarget;

        private float _remainingTimeToStart;
        private bool _isGameStarted;
        public void Init(MatchEvent matchEvent, MatchData matchData)
        {
            _matchEvent = matchEvent;
            _matchData = matchData;

            InitManagers();
            InitInteractiveObjects();

            void InitManagers()
            {
                _scoreManager.Init(_matchEvent, _matchData);
                _timeManager.Init(_matchEvent, _matchData);
                _backgroundManager.Init(_matchEvent);
                _difficultyManager.Init(_matchEvent, _matchData);
                _textManager.Init(_matchEvent);
            }
            void InitInteractiveObjects()
            {
                _player.Init(_matchEvent);
                _cpu.Init(_matchEvent, _ball);
                _ball.Init(_matchEvent, _ballAimingTarget);
            }
        }

        private void StartMatch()
        {
            _matchEvent.CurrentState = MatchEvent.MatchState.PLAYING;
        }

        private void EndMatch()
        {
            _matchEvent.CurrentState = MatchEvent.MatchState.STOPPED;
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

        public void StartCountdown()
        {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            _isGameStarted = false;
            _remainingTimeToStart = 3;
            _matchEvent.CountdownToStart.Invoke();
            _matchEvent.RemainingTimeToStart.Invoke(_remainingTimeToStart);
            while (_remainingTimeToStart > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                _remainingTimeToStart -= 1;
                _matchEvent.RemainingTimeToStart.Invoke(_remainingTimeToStart);
            }
            _isGameStarted = true;
            _matchEvent.MatchStart.Invoke();
        }
    }
}