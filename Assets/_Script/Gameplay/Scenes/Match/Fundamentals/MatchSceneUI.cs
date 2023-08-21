using TMPro;
using UnityEngine;
using Monetization.Ads;
using Monetization.Ads.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Services.FirebaseService.Analytics;
using Common;

namespace Gameplay
{
    public class MatchSceneUI : MonoBehaviour
    {
        private MatchManager _matchManager;
        private MatchData _matchData;
        private MatchSceneManager _sceneManager;
        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private BasePopup _revivePanel;
        public void Init(MatchData matchData, MatchManager matchManager, MatchSceneManager matchSceneManager)
        {
            _sceneManager = matchSceneManager;
            _matchManager = matchManager;
            _matchData = matchData;
        }


        public void OpenGameOverPanel()
        {
            _gameOverPanel.Init(_matchData);
            _gameOverPanel.Open(false);
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

            _gameOverPanel.Close(true).onComplete +=
                () =>
                {
                    _sceneManager.BackToHome();
                };
        }

        public void OnNoThanksClick()
        {
            _revivePanel.Close().onComplete += () =>
            {
                OpenGameOverPanel();
            };
        }

        public void OnReviveClick()
        {
            FirebaseAnalytics.Instance.PushEvent(Constant.REVIVE_CLICK);
        }
    }
}