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
        public RemoteDouble NewMaxIncrement;
        public RemoteBool IsBannerOnMatchScene;

        public MyRemoteVariableCollection()
        {
            IncrementalStep = new RemoteJson("IncrementalSteps", JsonUtility.ToJson(new IncrementalStep()));
            TimescaleStep = new RemoteDouble("TimescaleStep", 1);
            MaxIncrement = new RemoteDouble("MaxIncrement", 10);
            BackgroundColorOrder = new RemoteJson("BackgroundColorOrder", JsonUtility.ToJson(new BackgroundColorOrder()));
            NewMaxIncrement = new RemoteDouble("NewMaxIncrement", 11);
            IsBannerOnMatchScene = new RemoteBool("IsBannerOnMatchScene", false);
        }

        public override void AddToFetchQueue()
        {
            AddVariable(TimescaleStep);
            AddVariable(MaxIncrement);
            AddVariable(IncrementalStep);
            AddVariable(BackgroundColorOrder);
            AddVariable(NewMaxIncrement);
            AddVariable(IsBannerOnMatchScene);
        }
    }
}