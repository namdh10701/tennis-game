﻿using UnityEngine;
using static Gameplay.MatchEvent;

namespace Gameplay
{
    //ToDo:Flip character here
    public class Player : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        [SerializeField] private InputManager _inputManager;
        public void Init(MatchEvent matchEvent)
        {
            _inputManager.Init(_matchEvent, transform);
            _matchEvent = matchEvent;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Ball"))
            {
                _matchEvent.BallHit.Invoke(Side.Player);
            }
        }

        private void OnEnable()
        {
            _matchEvent.BallHitSuccess += (side) =>
             {
                 if (side == Side.Player)
                     transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
             };
        }

        private void OnDisable()
        {
            _matchEvent.BallHitSuccess -= (side) =>
            {
                if (side == Side.Player)
                    transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            };
        }
    }
}