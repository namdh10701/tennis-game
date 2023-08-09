using TMPro;
using UnityEngine;
using UnityEngine.Playables;
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


        [SerializeField] private GameObject _FootballAdLock;
        [SerializeField] private GameObject _BaseballAdLock;
        [SerializeField] private GameObject _VolleyballAdLock;

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
            }
        }

        public void OnIncrementalClick()
        {
            _matchSetting.ChangeIncremental();
            _incrementalText.text = "X" + _matchSetting.Incremental;
        }
        public void OnSportClick(Sport sport)
        {
            if (_gameDataManager.GameDatas.UnlockedSports.Contains(sport))
            {
                //ToDo: start match here
            }
            else
            {
                //ToDo: show reward ad to unlock sport
                //unlock game => destroy adlock
            }
        }

        //ToDo: change to leaderboard scene here
        public void OnRankClick()
        {

        }

    }
}