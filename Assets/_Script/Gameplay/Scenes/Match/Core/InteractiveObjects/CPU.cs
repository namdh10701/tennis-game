﻿using System.Collections;
using UnityEngine;
using static Gameplay.MatchEvent;

namespace Gameplay
{
    public class CPU : MonoBehaviour
    {
        [SerializeField] private Transform _cat;
        [SerializeField] private Animator _animator;

        private Ball _ball;
        private MatchEvent _matchEvent;
        private Coroutine _moveCoroutine;
        private Vector3 _originalPos;
        private void Start()
        {
            _animator.Play("Idle");
        }

        public void Init(MatchEvent matchEvent, MatchSetting matchSettings, Ball ball)
        {
            _originalPos = _cat.position;
            _ball = ball;
            _matchEvent = matchEvent;
        }

        public void Prepare()
        {
            _cat.position = _originalPos;
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
            _animator.SetTrigger("Hit");
        }

        public void ProcessHit()
        {
            _matchEvent.BallHit.Invoke(Side.CPU);
        }

        private void MoveToBall(Vector3 ballPos)
        {
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
    }
}