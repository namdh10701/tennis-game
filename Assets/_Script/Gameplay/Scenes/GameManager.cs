using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public SettingManager SettingManager = new SettingManager();
        public GameDataManager GameDataManager = new GameDataManager();

        public MatchSetting MatchSetting = new MatchSetting();
        private void Awake()
        {
            if (FindObjectOfType<GameManager>() != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            SettingManager.LoadSettings();
            GameDataManager.LoadSettings();
        }
    }
}