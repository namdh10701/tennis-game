using Common;
using JetBrains.Annotations;
using Monetization.Ads;
using Monetization.Ads.UI;
using Phoenix;
using Services.FirebaseService.Analytics;
using System;
using System.Collections.Generic;
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
        [SerializeField] private List<Image> _catShadows;
        [SerializeField] private SceneTransition _sceneTransition;


        [SerializeField] private BasePopup _notUnlockedIncrement;

        public CatAsset CatAsset;
        public CatShadowAsset CatShadowAsset;
        public Animator _catSlideAnimator;
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

                UpdateCatImage();
            }
        }

        public void OnIncrementalClick()
        {
            _matchSetting.ChangeIncremental();
            _incrementalText.text = "X" + _matchSetting.Incremental;
            UpdateCatImage();
        }

        private void UpdateCatImage()
        {
            _catImage.sprite = CatAsset.CatSprites[_matchSetting.Incremental - 1];

            if (_matchSetting.Incremental > GameDataManager.Instance.GameDatas.UnlockedIncremental)
            {
                foreach (Image catShadow in _catShadows)
                {
                    catShadow.gameObject.SetActive(false);
                }
                _catImage.color = Color.black;
                _catSlideAnimator.SetTrigger("LockedCatSlide");
            }
            else
            {
                foreach (Image catShadow in _catShadows)
                {
                    catShadow.gameObject.SetActive(true);
                    catShadow.sprite = CatShadowAsset.CatShadowSprites[_matchSetting.Incremental - 1];
                }
                _catImage.color = Color.white;
                _catSlideAnimator.SetTrigger("SlideIn");
            }
        }

        private void UnlockSportUI(Sport sport)
        {
            switch (sport)
            {
                case Sport.FOOTBALL:
                    _unlockFootballBtn.IsButtonActive = false;
                    _unlockFootballBtn.GetComponent<Button>().onClick.AddListener(() => _sceneManager.StartMatch(sport));
                    break;
                case Sport.BASEBALL:
                    _unlockBaseballBtn.IsButtonActive = false;
                    _unlockBaseballBtn.GetComponent<Button>().onClick.AddListener(() => _sceneManager.StartMatch(sport));
                    break;
                case Sport.VOLLEYBALL:
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
            {
                GameDataManager.Instance.GameDatas.UnlockedSports.Add(sport);
                FirebaseAnalytics.Instance.PushEvent($"REWARD_COMPLETE_{sport.ToString().ToUpper()}");
            }
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
            AdsController.Instance.ShowInter(
                () =>
                {
                    _sceneTransition.ChangeScene("SkinScene");
                }
                );
        }

        public void OpenNotUnlockedIncremental()
        {
            _notUnlockedIncrement.Open();
        }
    }
}