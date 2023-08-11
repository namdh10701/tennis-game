using UnityEngine;

namespace Gameplay
{
    //ToDo: 
    public class DifficultyManager : MonoBehaviour
    {
        private MatchData _matchData;
        public void Init(MatchData matchData)
        {
            _matchData = matchData;
        }

        public void ApplyDifficulty()
        {
            Time.timeScale = _matchData.MatchSettings.Incremental;
        }
        public void IncreaseDifficulty()
        {
            _matchData.MatchSettings.ChangeIncremental();
            ApplyDifficulty();
        }
    }
}