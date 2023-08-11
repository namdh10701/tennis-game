using System.Collections.Generic;
using UnityEngine;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    public class GameDataManager
    {
        public GameData GameDatas { get; private set; }

        public GameDataManager()
        {
            GameDatas = new GameData();
        }

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(GameDatas);
            PlayerPrefs.SetString("GameDatas", json);
            PlayerPrefs.Save();
        }

        public void LoadDatas()
        {
            if (PlayerPrefs.HasKey("GameDatas"))
            {
                string json = PlayerPrefs.GetString("GameDatas");
                GameDatas = JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                CreateDefaultDatas();
            }
        }

        private void CreateDefaultDatas()
        {
            GameDatas.HighScore = 0;
            GameDatas.HighTime = 0;
            GameDatas.LastGamePlayed = null;
            GameDatas.UnlockedSports = new List<Sport>();
        }
    }
}