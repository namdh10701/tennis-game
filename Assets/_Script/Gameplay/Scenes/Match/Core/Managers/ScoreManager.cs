using System;
using TMPro;
using UnityEngine;
using static Gameplay.MatchEvent;

namespace Gameplay
{
    //ToDo: Handle AddScore Mechanism

    public class ScoreManager : MonoBehaviour
    {
        private MatchData _matchData;
        [SerializeField] private TextMeshProUGUI scoreText;


        public void Init(MatchData matchData)
        {
            _matchData = matchData;
        }

        public void Increase()
        {
            _matchData.Score++;
            scoreText.text = _matchData.Score.ToString();
        }

        public void Prepare()
        {
            scoreText.text = "0";
        }
    }
}