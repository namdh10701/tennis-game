using UnityEngine;

namespace Gameplay
{
    public class SettingManager
    {
        private static SettingManager instance;
        public static SettingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingManager();
                }
                return instance;
            }
        }
        public GameSetting GameSettings { get; private set; }

        public SettingManager()
        {
            GameSettings = LoadSettings();
        }

        public void SaveSettings()
        {
            string json = JsonUtility.ToJson(GameSettings);
            PlayerPrefs.SetString("GameSettings", json);
            PlayerPrefs.Save();
        }

        public GameSetting LoadSettings()
        {
            if (PlayerPrefs.HasKey("GameSettings"))
            {
                string json = PlayerPrefs.GetString("GameSettings");
                return JsonUtility.FromJson<GameSetting>(json);
            }
            else
            {
                return CreateDefaultSettings();
            }
        }

        private GameSetting CreateDefaultSettings()
        {
            GameSetting defaultGameSettings = new GameSetting();
            return defaultGameSettings;
        }
    }
}