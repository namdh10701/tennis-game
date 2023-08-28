using Gameplay;
using Services.FirebaseService;

public static class FirebaseHandler
{
    public static void Init()
    {
        FirebaseManager.Instance.RemoteVariableCollection = RemoteVariableManager.Instance.MyRemoteVariables;
        FirebaseManager.Instance.Init();
        FirebaseManager.Instance.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
    }
    private static void SaveRemoteVariable()
    {
        RemoteVariableManager.Instance.SaveDatas();
        MatchSetting.MaxIncrementalIngame = (int)RemoteVariableManager.Instance.MyRemoteVariables.NewMaxIncrement.GetValue();
    }
}