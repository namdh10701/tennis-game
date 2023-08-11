using Services.FirebaseService.Remote;

namespace Gameplay
{
    [System.Serializable]
    public class MyRemoteVariableCollection : RemoteVariableCollection
    {
        public RemoteFloat IncrementTimescale { get; set; }
        public RemoteInt MaxIncrement { get; set; }
        public RemoteInt ScoreToIncrement { get; set; }

        public MyRemoteVariableCollection()
        {
            IncrementTimescale = new RemoteFloat("IncrementTimescale", 1);
            MaxIncrement = new RemoteInt("MaxIncrement", 10);
            ScoreToIncrement = new RemoteInt("ScoreToIncrement", 10);
        }

        public override void AddToFetchQueue()
        {
            AddVariable(IncrementTimescale);
            AddVariable(MaxIncrement);
            AddVariable(ScoreToIncrement);
        }
    }
}