using UnityEngine;

namespace Gameplay
{
    public class SettingManager
    {
        public GameSetting GameSettings { get; private set; }

        public SettingManager()
        {
            GameSettings = new GameSetting();
        }

        public void SaveSettings()
        {
            string json = JsonUtility.ToJson(GameSettings);
            PlayerPrefs.SetString("GameSettings", json);
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey("GameSettings"))
            {
                string json = PlayerPrefs.GetString("GameSettings");
                GameSettings = JsonUtility.FromJson<GameSetting>(json);
            }
            else
            {
                CreateDefaultSettings();
            }
        }

        private void CreateDefaultSettings()
        {
            GameSettings.IsMusicOn = true;
            GameSettings.IsSoundOn = true;
            GameSettings.IsVibrationOn = true;
            GameSettings.IsReversed = false;
        }
    }
}