using UnityEngine;

namespace Gameplay
{
    public class RemoteVariableManager
    {
        private static RemoteVariableManager instance;
        public static RemoteVariableManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RemoteVariableManager();
                }
                return instance;
            }
        }

        public RemoteVariableManager()
        {
            MyRemoteVariables = LoadDatas();
        }

        public MyRemoteVariableCollection MyRemoteVariables;

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(MyRemoteVariables);
            PlayerPrefs.SetString("RemoteVariableCollection", json);
            PlayerPrefs.Save();
        }

        public MyRemoteVariableCollection LoadDatas()
        {
            if (PlayerPrefs.HasKey("RemoteVariableCollection"))
            {
                string json = PlayerPrefs.GetString("RemoteVariableCollection");
                return JsonUtility.FromJson<MyRemoteVariableCollection>(json);
            }
            else
            {
                return new MyRemoteVariableCollection();
            }
        }
    }
}