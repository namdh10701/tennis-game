using Phoenix;
using System;
using UnityEngine;
using static Gameplay.MatchEvent;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    //ToDo:Flip character here
    public class Player : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        [SerializeField] private InputManager _inputManager;
        private Vector3 _originalPos;
        private Vector3 _originalScale;
        private Vector3 _flippedScale;
        [SerializeField] private PlayerCollider _playerCollider;

        public void Init(MatchEvent matchEvent, MatchSetting matchSettings)
        {
            _originalPos = transform.position;
            _inputManager.Init(matchEvent, transform);
            _matchEvent = matchEvent;
            _originalScale = transform.localScale;
            _flippedScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _playerCollider.Init(matchEvent);
            _matchEvent.BallMove += HandleFlipCharacter;
        }

        private void HandleFlipCharacter(Vector3 newBallPos, Side side)
        {
            if (side == Side.Player)
                return;
            if (newBallPos.x < _originalPos.x)
            {
                transform.localScale = _originalScale;
            }
            else if (newBallPos.x > _originalPos.x)
            {
                transform.localScale = _flippedScale;
            }
        }

        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.BallMove += HandleFlipCharacter;
            }
        }

        private void OnDisable()
        {
            _matchEvent.BallMove -= HandleFlipCharacter;
        }

        public void Prepare()
        {
            transform.position = _originalPos;
        }
    }
}