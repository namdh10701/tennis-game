using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class MatchEvent
    {
        public enum Side
        {
            Player, CPU
        }
        public enum MatchState
        {
            PRE_START, PLAYING, PAUSING, STOPPED
        }

        public MatchState CurrentState = MatchState.PRE_START;
        public Action CountdownToStart;
        public Action<float> RemainingTimeToStart;
        public Action MatchStart;
        public Action BallServed;
        public Action MatchEnd;

        public Action TimeUpdate;
        public Action ScoreUpdate;
        public Action Increment;
        public Action DifficultyIncreased;

        public Action<Side> BallHit;
        public Action<Side> BallHitSuccess;
        public Action BallMove;
    }
}