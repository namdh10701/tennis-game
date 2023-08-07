using System.Collections;
using UnityEngine;
using static MatchEvent;
public class CPU : MonoBehaviour
{
    private MatchEvent _matchEvent;
    private Ball _ball;
    [SerializeField] private Transform _cat;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _serveBallPoint;
    [SerializeField] private Animator _animator;

    private Coroutine move;
    private void Start()
    {
        _animator.Play("Idle");
    }
    public void Init(MatchEvent matchEvent, MatchSetting.SportName sport, Ball ball)
    {
        _ball = ball;
        _matchEvent = matchEvent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_matchEvent.CurrentState == MatchState.PLAYING)
        {
            if (collision.CompareTag("Ball"))
            {
                Hit();
            }
        }
    }


    private void MoveToBall()
    {
        if (move != null)
        {
            StopCoroutine(move);
        }
            
        move = StartCoroutine(MoveToBallCoroutine());
    }

    private IEnumerator MoveToBallCoroutine()
    {
        if (_cat.position.x > _ball.AimingTarget.position.x)
        {
            while (_cat.position.x > _ball.AimingTarget.position.x)
            {
                float step = 5 * Time.deltaTime;
                _cat.position -= new Vector3(step, 0, 0);
                yield return null;
            }
        }
        else
        {
            while (_cat.position.x < _ball.AimingTarget.position.x)
            {
                float step = 5 * Time.deltaTime;
                _cat.position += new Vector3(step, 0, 0);
                yield return null;
            }
        }

    }

    private void Hit()
    {
        _animator.SetTrigger("Hit");
    }

    private void ProcessHit()
    {
        _matchEvent.BallHit.Invoke(Side.CPU);
    }

    public void ServeBall()
    {
        StartCoroutine(ServeBallCoroutine());
    }

    private IEnumerator ServeBallCoroutine()
    {
        _animator.SetTrigger("Hit");
        _ball.MoveToServePoint(_serveBallPoint);
        _ball.SetCurrentState(Ball.State.GOING_UP);
        yield return new WaitForSeconds(.5f);
        _ball.SetCurrentState(Ball.State.FALLING);

    }


    private void OnEnable()
    {
        if (_matchEvent != null)
        {
            _matchEvent.BallMove.AddListener(() => MoveToBall());
        }
    }

    private void OnDisable()
    {
        _matchEvent.BallMove.RemoveListener(() => MoveToBall());
    }
}