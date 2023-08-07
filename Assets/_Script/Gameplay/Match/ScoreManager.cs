using UnityEngine;
using static MatchEvent;

public class ScoreManager : MonoBehaviour
{
    private MatchEvent _matchEvent;
    private MatchData _matchData;

    public void Init(MatchEvent matchEvent, MatchData matchData)
    {
        _matchEvent = matchEvent;
        _matchData = matchData;
    }

    public void Increase(Side side)
    {
        if (side == Side.Player)
        {
            _matchData.Score++;
            _matchEvent.ScoreUpdate.Invoke();
        }
    }

    private void OnEnable()
    {
        if (_matchEvent != null)
        {
            _matchEvent.BallHitSuccess.AddListener((side) => Increase(side));
        }
    }

    private void OnDisable()
    {
        _matchEvent.BallHitSuccess.RemoveListener((side) => Increase(side));
    }
}