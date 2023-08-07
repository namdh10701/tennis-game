using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "MatchEvent", menuName = "ScriptableObjects/MatchEvent")]
public class MatchEvent : ScriptableObject
{
    public enum Side
    {
        Player, CPU
    }

    public enum MatchState
    {
        PRE_START, PLAYING, PAUSING, STOPPED
    }

    public UnityEvent CountdownToStart;
    public UnityEvent<float> RemainingTimeToStart;
    public UnityEvent MatchStart;
    public UnityEvent MatchEnd;

    public UnityEvent TimeUpdate;
    public UnityEvent ScoreUpdate;
    public UnityEvent Increment;

    public UnityEvent<Side> BallHit;
    public UnityEvent<Side> BallHitSuccess;
    public UnityEvent BallMove;

    public MatchState CurrentState;
}