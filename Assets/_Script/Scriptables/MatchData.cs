using UnityEngine;

public class MatchData : ScriptableObject
{
    public float ElapsedTime;
    public int Score;

    public static MatchData CreateNewInstance()
    {
        MatchData matchData = ScriptableObject.CreateInstance<MatchData>();
        matchData.ElapsedTime = 0;
        matchData.Score = 0;
        return matchData;
    }
}