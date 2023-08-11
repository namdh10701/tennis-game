using TMPro;
using UnityEngine;
using Monetization.Ads.UI;

namespace Gameplay
{
    public class MatchSceneUI : MonoBehaviour
    {
        private MatchData _matchData;
        private GameData _gameData;

        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private BasePopup _revivePanel;
        public void Init(MatchData matchData, GameData gameData)
        {
            _gameData = gameData;
            _matchData = matchData;
        }


        public void OpenGameOverPanel()
        {
            _gameOverPanel.Init(_matchData, _gameData);
            _gameOverPanel.Open();
        }

        public void OpenRevivePanel()
        {
            _revivePanel.Open();
        }
    }
}