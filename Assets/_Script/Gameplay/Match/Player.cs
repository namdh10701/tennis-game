using UnityEngine;
using static MatchEvent;

public class Player : MonoBehaviour
{
    private MatchEvent _matchEvent;

    public void Init(MatchEvent matchEvent, MatchSetting.SportName sport)
    {
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
        _matchEvent.BallHitSuccess.AddListener((side) =>
        {
            if (side == Side.Player)
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        });
    }

    private void OnDisable()
    {
        _matchEvent.BallHitSuccess.RemoveListener((side) =>
        {
            if (side == Side.Player)
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        });
    }
}