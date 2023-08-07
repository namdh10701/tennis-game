using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using static Ball;

public class MatchManager : MonoBehaviour
{
    private MatchSetting _matchSetting;
    private MatchEvent _matchEvent;
    private MatchData _matchData;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TimeManager _timeManager;

    [SerializeField] private Player _player;
    [SerializeField] private CPU _cpu;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _ballAimingTarget;
    [SerializeField] private InputManager _inputManager;
    public void Init(MatchSetting matchSetting, MatchEvent matchEvent, MatchData matchData)
    {
        _matchSetting = matchSetting;
        _matchEvent = matchEvent;
        _matchData = matchData;
        _scoreManager.Init(_matchEvent, _matchData);
        _timeManager.Init(_matchEvent, _matchData);
        _player.Init(_matchEvent, _matchSetting.Sport);
        _cpu.Init(_matchEvent, _matchSetting.Sport, _ball);
        _ball.Init(_matchEvent, _matchSetting.Sport, _ballAimingTarget);
        _inputManager.Init(_matchEvent, _player.transform);
    }

    private void ServeBall()
    {
        _matchEvent.CurrentState = MatchEvent.MatchState.PLAYING;
        _cpu.ServeBall();
        if (_matchSetting.Incremental != 1)
        {
            Time.timeScale = _matchSetting.Incremental / 1.5f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void EndMatch()
    {
        _matchEvent.CurrentState = MatchEvent.MatchState.STOPPED;
    }

    private void IncreaseDifficulty()
    {
        _matchSetting.ChangeIncremental();
        if (_matchSetting.Incremental != 1)
        {
            Time.timeScale = _matchSetting.Incremental / 1.5f;
        }

    }

    private void OnEnable()
    {
        if (_matchEvent != null)
        {
            _matchEvent.MatchStart.AddListener(() => ServeBall());
            _matchEvent.MatchEnd.AddListener(() => EndMatch());
            _matchEvent.Increment.AddListener(() => IncreaseDifficulty());
        }
    }
    private void OnDisable()
    {
        _matchEvent.MatchStart.RemoveListener(() => ServeBall());
        _matchEvent.MatchEnd.RemoveListener(() => EndMatch());
        _matchEvent.Increment.RemoveListener(() => IncreaseDifficulty());
    }
}