using UnityEngine;

namespace Gameplay
{
    public class GameDataManager
    {
        public GameData GameDatas { get; private set; }

        public GameDataManager()
        {
            GameDatas = new GameData();
        }

        public void SaveSettings()
        {
            string json = JsonUtility.ToJson(GameDatas);
            PlayerPrefs.SetString("GameDatas", json);
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey("GameDatas"))
            {
                string json = PlayerPrefs.GetString("GameDatas");
                GameDatas = JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                CreateDefaultSettings();
            }
        }

        private void CreateDefaultSettings()
        {
            GameDatas.HighScore = 0;
            GameDatas.HighTime = 0;
        }
    }
}