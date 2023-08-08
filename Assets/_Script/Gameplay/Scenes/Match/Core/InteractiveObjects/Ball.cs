using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using static MatchEvent;
using static MatchSetting;

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
        public Transform AimingTarget { get; private set; }
        public State CurrentState { get; private set; }
        public float hangingTimer;
        private Coroutine moveCoroutrine;
        private bool _explosed;
        private bool _isHanging;
        private Side _lastHitBy;

        public void SetCurrentState(State newState)
        {
            CurrentState = newState;
        }

        public void Init(MatchEvent matchEvent, Transform aimingTarget)
        {
            _matchEvent = matchEvent;
            AimingTarget = aimingTarget;
            IsAbleToBeHitByCPU = true;
            IsAbleToBeHitByPlayer = true;
            _explosed = false;
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
                    _matchEvent.BallHitSuccess.Invoke(side);
                }
                if (side == Side.CPU && IsAbleToBeHitByCPU)
                {
                    _lastHitBy = side;
                    IsAbleToBeHitByPlayer = false;
                    MoveToPlayerSide();
                    _matchEvent.BallHitSuccess.Invoke(side);
                }
            }
        }

        public void MoveToServePoint(Transform serveBallPoint)
        {
            transform.position = serveBallPoint.position;
            AimingTarget.position = serveBallPoint.position;
        }

        private void Update()
        {
            if (_matchEvent.CurrentState == MatchState.PLAYING)
            {
                switch (CurrentState)
                {
                    case State.GOING_UP:
                        transform.localScale += new Vector3(1.5f, 1.5f, 1.5f) * Time.deltaTime;
                        transform.Rotate(axis: Vector3.forward, 120 * Time.deltaTime);
                        break;
                    case State.FALLING:
                        if (transform.localScale.x > 3) { transform.localScale -= new Vector3(1.5f, 1.5f, 1.5f) * Time.deltaTime; }
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

            AimingTarget.position = GetRandomCPUSidePos();
            _matchEvent.BallMove.Invoke();
            CurrentState = State.FALLING;
            MoveToAimingTarget();
        }

        private void MoveToPlayerSide()
        {
            AimingTarget.position = GetRandomPlayerSidePos();
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
            while (transform.position != AimingTarget.position)
            {
                float step = 3 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, AimingTarget.position, step);
                yield return null;
            }
            _isHanging = true;
        }

        private void Explose()
        {
            //ToDo:
            Destroy(gameObject);
            Destroy(AimingTarget.gameObject);
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
            _matchEvent.BallHit += side => HitBy(side);
        }

    }
}