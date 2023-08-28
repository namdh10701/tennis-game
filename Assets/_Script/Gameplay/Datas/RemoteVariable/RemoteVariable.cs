using Gameplay;
using Services.FirebaseService.Remote;
using UnityEngine;

public class RemoteVariable
{
    public BackgroundColorOrder BackgroundColorOrder;
    public IncrementalStep IncrementalStep;
    public float TimescaleStep;
    public int MaxIncrement;
    public int NewMaxIncrement;

    public static T Convert<T>(RemoteJson remoteJson)
    {
        return JsonUtility.FromJson<T>((string)remoteJson.Value);
    }
    public static RemoteVariable Convert(MyRemoteVariableCollection myRemoteVariableCollection)
    {
        RemoteVariable remoteVariable = new RemoteVariable();
        remoteVariable.TimescaleStep = (float)myRemoteVariableCollection.TimescaleStep.Value;
        remoteVariable.MaxIncrement = (int)myRemoteVariableCollection.MaxIncrement.Value;
        remoteVariable.BackgroundColorOrder = Convert<BackgroundColorOrder>(myRemoteVariableCollection.BackgroundColorOrder);
        remoteVariable.IncrementalStep = Convert<IncrementalStep>(myRemoteVariableCollection.IncrementalStep);
        remoteVariable.NewMaxIncrement = (int)myRemoteVariableCollection.NewMaxIncrement.Value;
        return remoteVariable;
    }
}