using UnityEngine;

namespace Gameplay
{
    public class RemoteVariableManager
    {
        public MyRemoteVariableCollection MyRemoteVariables
        {
            get
            {
                return _myRemoteVariables;
            }
            set
            {
                _myRemoteVariables = value;
            }
        }

        private MyRemoteVariableCollection _myRemoteVariables;

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(MyRemoteVariables);
            PlayerPrefs.SetString("RemoteVariableCollection", json);
            PlayerPrefs.Save();
        }

        public void LoadDatas()
        {
            MyRemoteVariableCollection loadedRemoteVariables;
            if (PlayerPrefs.HasKey("RemoteVariableCollection"))
            {
                string json = PlayerPrefs.GetString("RemoteVariableCollection");
                loadedRemoteVariables = JsonUtility.FromJson<MyRemoteVariableCollection>(json);
            }
            else
            {
                loadedRemoteVariables = new MyRemoteVariableCollection();
            }
            _myRemoteVariables = loadedRemoteVariables;
        }
    }
}