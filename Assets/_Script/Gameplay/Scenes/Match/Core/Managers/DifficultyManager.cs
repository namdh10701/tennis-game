using UnityEngine;

namespace Gameplay
{
    //ToDo: 
    public class DifficultyManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private MatchData _matchData;
        public void Init(MatchEvent matchEvent, MatchData matchData)
        {
            _matchEvent = matchEvent;
            _matchData = matchData;

            ApplyDifficulty();
        }

        private void ApplyDifficulty()
        {
            Time.timeScale = _matchData.MatchSettings.Incremental;
        }
        private void IncreaseDifficulty()
        {
            _matchData.MatchSettings.ChangeIncremental();
            ApplyDifficulty();
        }
    }
}