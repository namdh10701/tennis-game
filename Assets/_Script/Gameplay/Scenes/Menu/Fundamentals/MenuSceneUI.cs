using TMPro;
using UnityEngine;

namespace Gameplay
{
    //ToDo: Create Menu UI

    public class MenuSceneUI : MonoBehaviour
    {
        private MenuSceneManager _sceneManager;

        public TextMeshProUGUI incrementalText;
        public TextMeshProUGUI sportText;

        public void Init(MenuSceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        private void Awake()
        {
            incrementalText.text = "Speed: x" + _sceneManager.GameManager.MatchSetting.Incremental;
            sportText.text = _sceneManager.GameManager.MatchSetting.GetCurrentSportNameToString();
        }

        public void OnIncrementalClick()
        {
            _sceneManager.GameManager.MatchSetting.ChangeIncremental();
            incrementalText.text = "Speed: x" + _sceneManager.GameManager.MatchSetting.Incremental;
        }

        public void OnPlayCick()
        {
            _sceneManager.StartMatch();
        }
    }
}