using System.Collections;
using UnityEngine;
using static MatchEvent;

namespace Gameplay
{
    public class CPU : MonoBehaviour
    {
        [SerializeField] private Transform _cat;
        [SerializeField] private Transform _serveBallPoint;
        [SerializeField] private Animator _animator;

        private Ball _ball;
        private MatchEvent _matchEvent;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _animator.Play("Idle");
        }

        public void Init(MatchEvent matchEvent, Ball ball)
        {
            _ball = ball;
            _matchEvent = matchEvent;
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
            _matchEvent.BallHit.Invoke(Side.CPU);

            if (_matchEvent.CurrentState == MatchState.PRE_START)
            {
                _matchEvent.BallServed.Invoke();
            }
        }

        private void MoveToBall()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveToPosition(_ball.AimingTarget.position));
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

        private void ServeBall()
        {
            StartCoroutine(ServeBallCoroutine());
        }

        private IEnumerator ServeBallCoroutine()
        {
            _animator.SetTrigger("Hit");
            _ball.MoveToServePoint(_serveBallPoint);
            _ball.SetCurrentState(Ball.State.GOING_UP);
            yield return new WaitForSeconds(0.5f);
            _ball.SetCurrentState(Ball.State.FALLING);
        }
        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.MatchStart += ServeBall;
                _matchEvent.BallMove += MoveToBall;
            }
        }
        private void OnDisable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.MatchStart -= ServeBall;
                _matchEvent.BallMove -= MoveToBall;
            }
        }
    }
}