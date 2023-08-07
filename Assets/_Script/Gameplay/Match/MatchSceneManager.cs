using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MatchSceneManager : MonoBehaviour
{
    [SerializeField] private MatchSceneUI _matchSceneUI;
    [SerializeField] private MatchManager _matchManager;
    [SerializeField] private MatchSetting _matchSetting;
    [SerializeField] private MatchEvent _matchEvent;
    private MatchData _matchData;
    public float RemainingTimeToStart { get; private set; }
    public bool IsGameStarted { get; private set; }

    [SerializeField] private TextMeshProUGUI countdowntext;
    private void Awake()
    {
        _matchData = MatchData.CreateNewInstance();
        _matchManager.Init(_matchSetting, _matchEvent, _matchData);
        _matchSceneUI.Init(_matchEvent, _matchData);
        _matchEvent.CurrentState = MatchEvent.MatchState.PRE_START;
    }
    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        IsGameStarted = false;
        RemainingTimeToStart = 3;
        _matchEvent.CountdownToStart.Invoke();
        _matchEvent.RemainingTimeToStart.Invoke(RemainingTimeToStart);
        while (RemainingTimeToStart > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            RemainingTimeToStart -= 1;
            _matchEvent.RemainingTimeToStart.Invoke(RemainingTimeToStart);
        }
        IsGameStarted = true;
        _matchEvent.MatchStart.Invoke();
    }
}
