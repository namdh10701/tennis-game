using System.Collections.Generic;
using UnityEngine;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    public class GameDataManager
    {
        private static GameDataManager instance;
        public static GameDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameDataManager();
                }
                return instance;
            }
        }
        public GameData GameDatas { get; private set; }

        public GameDataManager()
        {
            GameDatas = LoadDatas();
        }

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(GameDatas);
            PlayerPrefs.SetString("GameDatas", json);
            PlayerPrefs.Save();
        }

        public GameData LoadDatas()
        {
            if (PlayerPrefs.HasKey("GameDatas"))
            {
                string json = PlayerPrefs.GetString("GameDatas");
                return JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                return CreateDefaultDatas();
            }
        }

        private GameData CreateDefaultDatas()
        {
            GameData defaultGameData = new GameData();
            return defaultGameData;
        }

        public void UnlockSkin(Skin skin)
        {
            if (!GameDatas.UnlockedSkin.Contains(skin))
            {
                GameDatas.UnlockedSkin.Add(skin);
            }
            if (GameDatas.LockedSkin.Contains(skin))
            {
                GameDatas.LockedSkin.Remove(skin);
            }
            SaveDatas();
        }
    }
}