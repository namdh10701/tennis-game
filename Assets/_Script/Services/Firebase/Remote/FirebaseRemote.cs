
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Enviroments;
using Firebase.RemoteConfig;
using Firebase.Extensions;

namespace Services.FirebaseService.Remote
{

    public class FirebaseRemote : MonoBehaviour
    {
        private RemoteVariableCollection _remoteVariableCollection;

        public Action OnFetchedCompleted;
        public Action OnFetchedFailed;

        public void Init(RemoteVariableCollection remoteVariableCollection)
        {
          /*  if (Enviroment.ENV == Enviroment.Env.PROD)
            {*/
                if (remoteVariableCollection == null)
                {
                    Debug.Log("You havent provide your custom remove variable collection");
                    return;
                }
            _remoteVariableCollection = remoteVariableCollection;
            _remoteVariableCollection.AddToFetchQueue();
            FetchDataAsync();
  /*          }
            else
            {
                Debug.Log("Firebase remote initilized");
            }*/
        }

        public Task FetchDataAsync()
        {
            Task fetchTask =
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        void FetchComplete(Task fetchTask)
        {
            FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        foreach (var kvp in _remoteVariableCollection.Variables)
                        {
                            string variableName = kvp.Key;
                            object variableValue = kvp.Value.GetValue();

                            if (variableValue is double)
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).DoubleValue);
                            }
                            else if (variableValue is string)
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).StringValue);
                            }
                            else if (variableValue is bool)
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).BooleanValue);
                            }
                            Debug.Log($"Feteched {variableName}: {kvp.Value.GetValue()}");
                        }
                        OnFetchedCompleted?.Invoke();
                    });
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }
    }
}