using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class TimeManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private MatchData _matchData;
        private Coroutine timecount;

        public void Init(MatchEvent matchEvent, MatchData matchData)
        {
            _matchEvent = matchEvent;
            _matchData = matchData;
        }
        private void OnEnable()
        {
            if (_matchEvent != null)
            {
                _matchEvent.MatchStart += () => StartTime();
                _matchEvent.MatchEnd += () => StopTime();
            }
        }
        private void OnDisable()
        {
            _matchEvent.MatchStart -= () => StartTime();
            _matchEvent.MatchEnd -= () => StopTime();
        }

        private void StartTime()
        {
            timecount = StartCoroutine(TimeCount());
        }

        private IEnumerator TimeCount()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                _matchData.ElapsedTime++;
                _matchEvent.TimeUpdate.Invoke();
                if (_matchData.ElapsedTime % 7 == 0)
                {
                    _matchEvent.Increment.Invoke();
                }
            }
        }

        private void StopTime()
        {
            if (timecount != null)
                StopCoroutine(timecount);
        }
    }
}