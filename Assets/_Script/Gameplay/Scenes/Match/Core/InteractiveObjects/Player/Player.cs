using System;
using UnityEngine;
using static Gameplay.MatchEvent;

namespace Gameplay
{
    //ToDo:Flip character here
    public class Player : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        [SerializeField] private InputManager _inputManager;
        public void Init(MatchEvent matchEvent, MatchSetting matchSettings)
        {
            _inputManager.Init(matchEvent, transform);
            _matchEvent = matchEvent;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Ball"))
            {
                _matchEvent.BallHit.Invoke(Side.Player);
            }
        }
        private void HandleFlipCharacter(Vector3 newBallPos)
        {
            //throw new NotImplementedException();
        }

        private void OnEnable()
        {
            _matchEvent.BallMove += HandleFlipCharacter;
        }

        private void OnDisable()
        {
            _matchEvent.BallMove -= HandleFlipCharacter;
        }
    }
}