using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MatchSetting", menuName = "ScriptableObjects/MatchSetting")]
public class MatchSetting : ScriptableObject
{
    public enum SportName
    {
        TENNIS, VOLLEYBALL
    }
    public readonly int MaxIncremental = 10;
    public readonly SportName[] AvailableSports = { SportName.TENNIS, SportName.VOLLEYBALL };
    public readonly Dictionary<SportName, string> SportNamesMap = new Dictionary<SportName, string>() {
        {SportName.TENNIS,"Tennis" },
        {SportName.VOLLEYBALL, "Volleyball" }
    };

    public int Incremental = 1;
    public SportName Sport;
    private int _currentSportIndex;

    public void ChangeGame()
    {
        if (_currentSportIndex != AvailableSports.Length - 1)
        {
            _currentSportIndex++;
        }
        else
        {
            _currentSportIndex = 0;
        }
        Sport = AvailableSports[_currentSportIndex];
    }
    public void ChangeIncremental()
    {
        if (Incremental != MaxIncremental)
        {
            Incremental += 1;
        }
        else
        {
            Incremental = 1;
        }
    }
    public string GetCurrentSportNameToString()
    {
        return SportNamesMap[Sport];
    }
}

