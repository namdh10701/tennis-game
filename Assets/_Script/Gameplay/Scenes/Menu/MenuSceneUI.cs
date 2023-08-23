using Common;
using JetBrains.Annotations;
using Monetization.Ads.UI;
using Phoenix;
using Services.FirebaseService.Analytics;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    //ToDo: Create Menu UI

    public class MenuSceneUI : MonoBehaviour
    {
        private MenuSceneManager _sceneManager;
        private MatchSetting _matchSetting;
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _incrementalText;

        [SerializeField] private Button _tennisBtn;
        [SerializeField] private RewardButton _unlockFootballBtn;
        [SerializeField] private RewardButton _unlockBaseballBtn;
        [SerializeField] private RewardButton _unlockVolleyballBtn;
        [SerializeField] private Image _catImage;
        [SerializeField] private SceneTransition _sceneTransition;


        [SerializeField] private BasePopup _notUnlockedIncrement;

        public CatAsset CatAsset;
        public void Init(MenuSceneManager sceneManager,
            MatchSetting matchSetting)
        {
            _sceneManager = sceneManager;
            _matchSetting = matchSetting;
            InitUIContent();
            void InitUIContent()
            {
                _incrementalText.text = "X" + _matchSetting.Incremental;
                _highScore.text = GameDataManager.Instance.GameDatas.HighScore.ToString();

                foreach (Sport sport in GameDataManager.Instance.GameDatas.UnlockedSports)
                {
                    UnlockSportUI(sport);
                }
                _tennisBtn.onClick.AddListener(
                    () =>
                    {
                        _sceneManager.StartMatch(Sport.TENNIS);
                    }
                    );
            }
        }

        public void OnIncrementalClick()
        {
            _matchSetting.ChangeIncremental();
            _incrementalText.text = "X" + _matchSetting.Incremental;
            _catImage.sprite = CatAsset.CatSprites[_matchSetting.Incremental - 1];
            if (_matchSetting.Incremental > GameDataManager.Instance.GameDatas.UnlockedIncremental)
            {
                _catImage.color = Color.black;
            }
            else
            {
                _catImage.color = Color.white;
            }
        }

        private void UnlockSportUI(Sport sport)
        {
            switch (sport)
            {
                case Sport.FOOTBALL:
                    FirebaseAnalytics.Instance.PushEvent(Constant.REWARD_COMPLETE_FOOTBALL);
                    _unlockFootballBtn.IsButtonActive = false;
                    _unlockFootballBtn.GetComponent<Button>().onClick.AddListener(() => _sceneManager.StartMatch(sport));
                    break;
                case Sport.BASEBALL:
                    FirebaseAnalytics.Instance.PushEvent(Constant.REWARD_COMPLETE_BASEBALL);
                    _unlockBaseballBtn.IsButtonActive = false;
                    _unlockBaseballBtn.GetComponent<Button>().onClick.AddListener(() => _sceneManager.StartMatch(sport));
                    break;
                case Sport.VOLLEYBALL:
                    FirebaseAnalytics.Instance.PushEvent(Constant.REWARD_COMPLETE_VOLLEYBALL);
                    _unlockVolleyballBtn.IsButtonActive = false;
                    _unlockVolleyballBtn.GetComponent<Button>().onClick.AddListener(() => _sceneManager.StartMatch(sport));
                    break;
            }
        }

        public void OnReward(string sportName)
        {
            Sport sport = (Sport)Enum.Parse(typeof(Sport), sportName);
            UnlockSportUI(sport);
            if (!GameDataManager.Instance.GameDatas.UnlockedSports.Contains(sport))
                GameDataManager.Instance.GameDatas.UnlockedSports.Add(sport);
            GameDataManager.Instance.SaveDatas();

        }




        //ToDo: change to leaderboard scene here
        public void OnRankClick()
        {
            Leaderboard.Instance.SignInToLeaderboardManual(
                singed =>
                {
                    if (singed)
                    {
                        Leaderboard.Instance.ShowLeaderboard();
                    }
                });
        }

        public void OnSkinClick()
        {
            _sceneTransition.ChangeScene("SkinScene");
        }

        public void OpenNotUnlockedIncremental()
        {
            _notUnlockedIncrement.Open();
        }
    }
}