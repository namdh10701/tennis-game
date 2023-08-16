using Services.FirebaseService.Remote;
using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public class MyRemoteVariableCollection : RemoteVariableCollection
    {
        public RemoteDouble TimescaleStep;
        public RemoteDouble MaxIncrement;

        public RemoteJson BackgroundColorOrder;

        public RemoteJson IncrementalStep;

        public MyRemoteVariableCollection()
        {
            IncrementalStep = new RemoteJson("IncrementalStep", JsonUtility.ToJson(new IncrementalStep()));
            TimescaleStep = new RemoteDouble("TimescaleStep", 1);
            MaxIncrement = new RemoteDouble("MaxIncrement", 10);
            BackgroundColorOrder = new RemoteJson("BackgroundColorOrder", JsonUtility.ToJson(new BackgroundColorOrder()));
        }

        public override void AddToFetchQueue()
        {
            AddVariable(TimescaleStep);
            AddVariable(MaxIncrement);
            AddVariable(IncrementalStep);
            AddVariable(BackgroundColorOrder);
        }
    }
}