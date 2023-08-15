using JetBrains.Annotations;
using Monetization.Ads.UI;
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

        //ToDo optional
        private GameDataManager _gameDataManager;

        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _incrementalText;

        [SerializeField] private Button _tennisBtn;
        [SerializeField] private RewardButton _unlockFootballBtn;
        [SerializeField] private RewardButton _unlockBaseballBtn;
        [SerializeField] private RewardButton _unlockVolleyballBtn;
        [SerializeField] private Image _catImage;


        [SerializeField] private BasePopup _notUnlockedIncrement;

        public CatAsset CatAsset;
        public void Init(MenuSceneManager sceneManager,
            MatchSetting matchSetting,
            GameDataManager gameDataManager)
        {
            _sceneManager = sceneManager;
            _matchSetting = matchSetting;
            _gameDataManager = gameDataManager;
            InitUIContent();
            void InitUIContent()
            {
                _incrementalText.text = "X" + _matchSetting.Incremental;
                _highScore.text = _gameDataManager.GameDatas.HighScore.ToString();

                foreach (Sport sport in _gameDataManager.GameDatas.UnlockedSports)
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
            if (_matchSetting.Incremental > _gameDataManager.GameDatas.UnlockedIncremental)
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
            Sport sport = (Sport)System.Enum.Parse(typeof(Sport), sportName);
            UnlockSportUI(sport);
            if (!_gameDataManager.GameDatas.UnlockedSports.Contains(sport))
                _gameDataManager.GameDatas.UnlockedSports.Add(sport);
            _gameDataManager.SaveDatas();

        }




        //ToDo: change to leaderboard scene here
        public void OnRankClick()
        {

        }

        public void OpenNotUnlockedIncremental()
        {
            _notUnlockedIncrement.Open();
        }
    }
}