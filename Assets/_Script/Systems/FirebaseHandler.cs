using Gameplay;
using Services.FirebaseService;
using UnityEngine;

public class FirebaseHandler : MonoBehaviour
{
    private void Awake()
    {
        FirebaseManager.Instance.RemoteVariableCollection = RemoteVariableManager.Instance.MyRemoteVariables;
        FirebaseManager.Instance.Init();
        FirebaseManager.Instance.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
    }
    private void SaveRemoteVariable()
    {
        RemoteVariableManager.Instance.SaveDatas();
    }

    private void OnEnable()
    {
        if (FirebaseManager.Instance.FirebaseRemote != null)
        {
            FirebaseManager.Instance.FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
        }
    }

    private void OnDisable()
    {
        if (FirebaseManager.Instance.FirebaseRemote != null)
        {
            FirebaseManager.Instance.FirebaseRemote.OnFetchedCompleted -= () => SaveRemoteVariable();
        }
    }
}