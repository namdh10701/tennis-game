using ListExtensions;
using System;
using System.Collections;
using UnityEngine;
using static Gameplay.MatchEvent;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    public class CPU : MonoBehaviour
    {
        [SerializeField] private Transform _cat;

        [SerializeField] private SpriteRenderer _catSprite;
        [SerializeField] private SpriteRenderer _toolSprite;
        [SerializeField] private Animator _animator;
        public SportAsset SportAsset;
        public CatAsset CatAsset;
        public SkinAsset SkinAsset;

        private Ball _ball;
        private MatchEvent _matchEvent;
        private Coroutine _moveCoroutine;
        private Vector3 _originalPos;
        private MatchSetting _matchSetting;
        private bool _isReversed;
        private int _maxCat;
        private Skin.SkinType _skinType;
        private void Start()
        {
            _animator.Play("Idle");
        }

        public void Init(MatchEvent matchEvent, MatchSetting matchSettings, Ball ball, bool isReversed, int maxCat)
        {
            _maxCat = maxCat;
            _isReversed = isReversed;
            if (_isReversed)
            {
                _cat.transform.localScale = new Vector3(_cat.transform.localScale.x, -_cat.transform.localScale.y, _cat.transform.localScale.z);
            }
            _originalPos = _cat.position;
            _ball = ball;
            _matchEvent = matchEvent;
            _matchSetting = matchSettings;
            switch (matchSettings.SportName)
            {
                case Sport.TENNIS:
                    _toolSprite.sprite = SportAsset.RacketSprite;
                    _skinType = Skin.SkinType.RACKET;
                    break;
                case Sport.BASEBALL:
                    _toolSprite.sprite = SportAsset.BasebatSprite;
                    _skinType = Skin.SkinType.BASEBAT;
                    break;
                case Sport.FOOTBALL:
                    _toolSprite.sprite = null;
                    _skinType = Skin.SkinType.GLOVES;
                    break;
                case Sport.VOLLEYBALL:
                    _toolSprite.sprite = null;
                    _skinType = Skin.SkinType.HAND;
                    break;
            }
            if (_skinType == Skin.SkinType.GLOVES
                || _skinType == Skin.SkinType.HAND)
            {
                return;
            }
            //ApplySkin();
        }

        private void ApplySkin()
        {
            string skinID = "";
            foreach (Skin skin in GameDataManager.Instance.GameDatas.Skins)
            {
                if (skin.Unlocked && skin.BeingUsed && skin.Type == _skinType)
                {
                    skinID = skin.ID;
                }
            }
            _toolSprite.sprite = SkinAsset.skinSprites[int.Parse(skinID) - 1];
        }

        public void Prepare()
        {
            _cat.position = _originalPos;
            _catSprite.sprite = CatAsset.CatSprites[_matchSetting.Incremental - 1];
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_matchEvent.CurrentState == MatchState.PLAYING && collision.CompareTag("Ball"))
            {
                Hit();
            }
        }

        private void Hit()
        {
            if (_isReversed)
            {
                _animator.SetTrigger("ReversedHit");
            }
            else
            {
                _animator.SetTrigger("Hit");
            }
        }

        public void ProcessHit()
        {
            _matchEvent.BallHit.Invoke(Side.CPU);
        }

        private void MoveToBall(Vector3 ballPos, Side side)
        {
            if (side == Side.CPU)
                return;
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveToPosition(ballPos));
        }

        private IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            while (!Mathf.Approximately(_cat.position.x, targetPosition.x))
            {
                float step = 5 * Time.deltaTime;
                _cat.position = Vector3.MoveTowards(_cat.position, new Vector3(targetPosition.x, _cat.position.y, _cat.position.z), step);
                yield return null;
            }
        }

        public void ServeBall()
        {
            StartCoroutine(ServeBallCoroutine());
        }

        private IEnumerator ServeBallCoroutine()
        {
            Hit();
            _ball.SetCurrentState(Ball.State.GOING_UP);
            yield return new WaitForSeconds(0.5f);
            _ball.SetCurrentState(Ball.State.FALLING);
            yield return new WaitForSeconds(0.5f);
        }
        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.BallMove += MoveToBall;
            }
        }
        private void OnDisable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.BallMove -= MoveToBall;
            }
        }

        public void UpdateCat()
        {
            _catSprite.sprite = CatAsset.CatSprites[(_matchSetting.Incremental - 1) % CatAsset.CatSprites.Count];
        }
    }
}