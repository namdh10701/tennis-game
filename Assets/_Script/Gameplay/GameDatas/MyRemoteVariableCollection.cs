using Services.FirebaseService.Remote;

namespace Gameplay
{
    [System.Serializable]
    public class MyRemoteVariableCollection : RemoteVariableCollection
    {
        public RemoteInt PlayerHealth { get; set; }
        public RemoteFloat PlayerSpeed { get; set; }
        public RemoteBool IsDebugEnabled { get; set; }
        public MyRemoteVariableCollection()
        {
            PlayerHealth = new RemoteInt("PlayerHealth", 0);
            PlayerSpeed = new RemoteFloat("PlayerSpeed", 0);
            IsDebugEnabled = new RemoteBool("IsDebugEnabled", false);
        }

        public override void AddToFetchQueue()
        {
            AddVariable(PlayerHealth);
            AddVariable(PlayerSpeed);
            AddVariable(IsDebugEnabled);
        }
    }
}