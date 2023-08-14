using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using static Gameplay.MatchEvent;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        public enum State
        {
            NONE, GOING_UP, FALLING
        }
        public bool IsAbleToBeHitByPlayer { get; private set; }
        public bool IsAbleToBeHitByCPU { get; private set; }
        [SerializeField] private Transform _aimingTarget;
        public State CurrentState { get; private set; }
        public float hangingTimer;
        private Coroutine moveCoroutrine;
        private bool _isHanging;
        private Side _lastHitBy;
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private Transform _serveBallPoint;
        [SerializeField] private SportAsset _sportAsset;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private bool served;
        public void SetServeBallPoint(Transform point)
        {
            _serveBallPoint = point;
        }
        public void SetCurrentState(State newState)
        {
            CurrentState = newState;
        }

        public void Init(MatchEvent matchEvent, MatchSetting matchSettings)
        {
            _matchEvent = matchEvent;
            switch (matchSettings.SportName)
            {
                case Sport.TENNIS:
                    _spriteRenderer.sprite = _sportAsset.TennisSprite;
                    break;
                case Sport.BASEBALL:
                    _spriteRenderer.sprite = _sportAsset.BaseballSprite;
                    break;
                case Sport.FOOTBALL:
                    _spriteRenderer.sprite = _sportAsset.FootballSprite;
                    break;
                case Sport.VOLLEYBALL:
                    _spriteRenderer.sprite = _sportAsset.VolleyballSprite;
                    break;

            }
        }

        public void Prepare()
        {
            _lastHitBy = Side.Player;
            IsAbleToBeHitByCPU = true;
            IsAbleToBeHitByPlayer = false;
            served = false;
            hangingTimer = 0;
            _isHanging = false;
            gameObject.SetActive(true);
            _aimingTarget.gameObject.SetActive(true);
            MoveToServePoint();
        }

        public void HitBy(Side side)
        {
            if (_lastHitBy != side)
            {
                if (side == Side.Player && IsAbleToBeHitByPlayer)
                {
                    _lastHitBy = side;
                    Debug.Log(side);
                    MoveToOpositeSite();
                    _matchManager.OnBallHitSuccess(side);
                }
                if (side == Side.CPU && IsAbleToBeHitByCPU)
                {
                    if (!served)
                    {
                        served = true;
                        _matchEvent.BallServed.Invoke();
                    }
                    _lastHitBy = side;
                    IsAbleToBeHitByPlayer = false;
                    MoveToPlayerSide();
                    _matchManager.OnBallHitSuccess(side);
                }
            }
        }

        public void MoveToServePoint()
        {
            transform.position = _serveBallPoint.position;
            _aimingTarget.position = _serveBallPoint.position;
        }

        private void Update()
        {
            if (_matchEvent.CurrentState == MatchState.PLAYING)
            {
                switch (CurrentState)
                {
                    case State.GOING_UP:
                        // transform.localScale += new Vector3(1.5f, 1.5f, 1.5f) * Time.deltaTime;
                        transform.Rotate(axis: Vector3.forward, 120 * Time.deltaTime);
                        break;
                    case State.FALLING:
                        //if (transform.localScale.x > 3) { transform.localScale -= new Vector3(1.5f, 1.5f, 1.5f) * Time.deltaTime; }
                        transform.Rotate(axis: Vector3.forward, -120 * Time.deltaTime);
                        break;
                    case State.NONE:
                        break;
                }
                if (_isHanging)
                {
                    hangingTimer += Time.deltaTime;
                    if (hangingTimer > 1)
                        Explose();
                }
            }
        }

        private void MoveToOpositeSite()
        {
            //ToDo logic hàm này và hàm dưới dựa theo màn hình điện thoại 
            _aimingTarget.position = GetRandomCPUSidePos();
            _matchEvent.BallMove.Invoke(_aimingTarget.position, Side.Player);
            CurrentState = State.FALLING;
            MoveToAimingTarget();
        }

        private void MoveToPlayerSide()
        {
            //ToDo logic hàm này và hàm trên dựa theo màn hình điện thoại 
            _aimingTarget.position = GetRandomPlayerSidePos();
            _matchEvent.BallMove.Invoke(_aimingTarget.position, Side.CPU);
            CurrentState = State.GOING_UP;
            MoveToAimingTarget();
        }

        private void MoveToAimingTarget()
        {
            if (moveCoroutrine != null)
                StopCoroutine(moveCoroutrine);
            moveCoroutrine = StartCoroutine(MoveToAimingTargetCoroutine());
        }

        private IEnumerator MoveToAimingTargetCoroutine()
        {
            _isHanging = false;
            hangingTimer = 0;
            while (transform.position != _aimingTarget.position)
            {
                float step = 3 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _aimingTarget.position, step);
                yield return null;
            }
            _isHanging = true;
        }

        private void Explose()
        {
            //ToDo: Anim nổ?
            gameObject.SetActive(false);
            _aimingTarget.gameObject.SetActive(false);
            _matchEvent.MatchEnd.Invoke();
        }

        private Vector2 GetRandomPlayerSidePos()
        {
            float minX = -2.6f;
            float maxX = 2.6f;
            float maxY = 4.7f;
            float minY = 3;

            return new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        }

        private Vector2 GetRandomCPUSidePos()
        {
            float minX = -2.6f;
            float maxX = 2.6f;

            return new Vector2(UnityEngine.Random.Range(minX, maxX), -2.62f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Net"))
            {
                IsAbleToBeHitByPlayer = true;
            }
        }
        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.BallHit += side => HitBy(side);
            }
        }
        private void OnDisable()
        {
            _matchEvent.BallHit -= side => HitBy(side);
        }

    }
}