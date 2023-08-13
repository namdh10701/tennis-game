using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    //ToDo
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI time;
        private MatchData _matchData;
        private Coroutine timecount;

        public void Init(MatchData matchData)
        {
            _matchData = matchData;
        }

        public void StartTime()
        {
            timecount = StartCoroutine(TimeCount());
        }

        private IEnumerator TimeCount()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                _matchData.ElapsedTime++;
                time.text = $"{((int)_matchData.ElapsedTime / 60).ToString("00")}:{(_matchData.ElapsedTime % 60).ToString("00")}";
            }
        }

        public void StopTime()
        {
            if (timecount != null)
                StopCoroutine(timecount);
        }

        public void Prepare()
        {
            time.text = "00:00";
        }
    }
}