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
        public Action MatchStart;
        public Action BallServed;
        public Action MatchEnd;


        public Action<Side> BallHit;
        public Action<Vector3, Side> BallMove;
    }
}