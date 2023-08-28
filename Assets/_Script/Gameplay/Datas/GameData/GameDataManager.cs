using System.Collections.Generic;
using System.IO;
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

        private string dataFilePath;

        public GameData GameDatas { get; private set; }

        public GameDataManager()
        {
            dataFilePath = Path.Combine(Application.persistentDataPath, "GameDatas.json");

            if (!File.Exists(dataFilePath))
            {
                GameDatas = CreateDefaultDatas();
                SaveDatas(); // Save default data to create the JSON file
            }
            GameDatas = LoadDatas();
        }

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(GameDatas);
            File.WriteAllText(dataFilePath, json);
        }

        public GameData LoadDatas()
        {
            string json = File.ReadAllText(dataFilePath);
            return JsonUtility.FromJson<GameData>(json);
        }

        private GameData CreateDefaultDatas()
        {
            UnityEngine.TextAsset defaultJsonAsset = Resources.Load<UnityEngine.TextAsset>("DefaultGameDatas");
            return JsonUtility.FromJson<GameData>(defaultJsonAsset.text);
        }

        public void UnlockSkin(Skin skin)
        {
            skin.Unlocked = true;
            SaveDatas();
        }
        public void UseSkin(Skin selectedSkin)
        {
            foreach (Skin skin in GameDatas.Skins)
            {
                if (skin.Type == selectedSkin.Type)
                {
                    skin.BeingUsed = false;
                }
            }
            selectedSkin.BeingUsed = true;
            SaveDatas();
        }
    }
}