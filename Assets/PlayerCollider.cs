using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gameplay.MatchEvent;

namespace Phoenix
{
    public class PlayerCollider : MonoBehaviour
    {
        private MatchEvent _matchEvent;

        public void Init(MatchEvent matchEvent)
        {
            _matchEvent = matchEvent;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            switch (_matchEvent.CurrentState)
            {
                case MatchState.PLAYING:
                    if (collision.CompareTag("Ball"))
                    {
                        _matchEvent.BallHit.Invoke(Side.Player);
                    }
                    break;

            }
        }
    }
}
