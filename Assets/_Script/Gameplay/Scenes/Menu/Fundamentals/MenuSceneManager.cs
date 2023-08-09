using UnityEngine;
using UnityEngine.SceneManagement;
using static Gameplay.MatchSetting;

namespace Gameplay
{

    //Handle UI Interact here
    public class MenuSceneManager : MonoBehaviour
    {
        public GameManager GameManager;

        [SerializeField] private MenuSceneUI _menuSceneUI;
        private void Awake()
        {
            _menuSceneUI.Init(this, GameManager.MatchSetting, GameManager.GameDataManager);
        }

        public void StartMatch(Sport sportName)
        {
            GameManager.MatchSetting.SportName = sportName;
            SceneManager.LoadScene("MatchScene");
        }
    }
}