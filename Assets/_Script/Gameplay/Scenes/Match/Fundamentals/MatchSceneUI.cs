using TMPro;
using UnityEngine;
using Monetization.Ads;
using Monetization.Ads.UI;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class MatchSceneUI : MonoBehaviour
    {
        private MatchManager _matchManager;
        private MatchData _matchData;
        private GameData _gameData;

        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private BasePopup _revivePanel;
        public void Init(MatchData matchData, GameData gameData, MatchManager matchManager)
        {
            _matchManager = matchManager;
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

        public void OnRestartClick()
        {
            _gameOverPanel.Close(true).onComplete +=
                () =>
                {
                    _matchManager.RestartMatch();
                };
        }

        public void OnHomeClick()
        {
            AdsController.Instance.ShowInter(
                () =>
                {
                    SceneManager.LoadScene("MenuScene");
                }
                );

        }
    }
}