using Phoenix;
using System;
using UnityEngine;
using static Gameplay.MatchEvent;
using static Gameplay.MatchSetting;
using static Gameplay.Skin;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        [SerializeField] private InputManager _inputManager;
        private Vector3 _originalPos;
        private Vector3 _originalScale;
        private Vector3 _flippedScale;
        [SerializeField] private PlayerCollider _playerCollider;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        public SkinAsset SkinAsset;
        private SkinType _skinType;
        private bool _isReversed;
        //Todo : skin

        public void Init(MatchEvent matchEvent, MatchSetting matchSettings, bool isReversed)
        {
            _isReversed = isReversed;
            if (isReversed)
            {
                Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 180);
                transform.rotation = newRotation;
            }
            _originalPos = transform.position;
            _inputManager.Init(matchEvent, transform, isReversed);
            _matchEvent = matchEvent;
            _originalScale = transform.localScale;
            _flippedScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _playerCollider.Init(matchEvent);
            _matchEvent.BallMove += HandleFlipCharacter;
            ApplySkin(matchSettings);
        }

        private void ApplySkin(MatchSetting matchSetting)
        {
            switch (matchSetting.SportName)
            {
                case Sport.TENNIS:
                    _skinType = Skin.SkinType.RACKET;
                    break;
                case Sport.BASEBALL:
                    _skinType = Skin.SkinType.BASEBAT;
                    break;
                case Sport.FOOTBALL:
                    _skinType = Skin.SkinType.GLOVES;
                    break;
                case Sport.VOLLEYBALL:
                    _skinType = Skin.SkinType.HAND;
                    break;
            }

            string skinID = "";
            foreach (Skin skin in GameDataManager.Instance.GameDatas.Skins)
            {
                if (skin.Unlocked && skin.BeingUsed && skin.Type == _skinType)
                {
                    skinID = skin.ID;
                }
            }
            _spriteRenderer.sprite = SkinAsset.skinSprites[int.Parse(skinID) - 1];
        }

        private void HandleFlipCharacter(Vector3 newBallPos, Side side)
        {
            if (side == Side.Player)
                return;
            if (newBallPos.x < _originalPos.x)
            {
                transform.localScale = _isReversed?_flippedScale: _originalScale;
            }
            else if (newBallPos.x > _originalPos.x)
            {
                transform.localScale = _isReversed ? _originalScale : _flippedScale;
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
            transform.localPosition = Vector3.zero;
        }
    }
}