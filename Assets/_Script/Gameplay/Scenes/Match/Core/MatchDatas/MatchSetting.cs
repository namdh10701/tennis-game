using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class MatchSetting
    {
        public enum Sport
        {
            TENNIS, VOLLEYBALL, FOOTBALL, BASEBALL
        }
        public static int MaxIncremental = 10;
        public static readonly Sport[] AvailableSports = { Sport.TENNIS, Sport.VOLLEYBALL };
        public static readonly Dictionary<Sport, string> SportNamesMap = new Dictionary<Sport, string>() {
        {Sport.TENNIS,"Tennis" },
        {Sport.VOLLEYBALL, "Volleyball" },
        {Sport.FOOTBALL, "Football" },
        {Sport.BASEBALL, "Baseball" },
    };

        public MatchSetting()
        {
            Incremental = 1;
            SportName = Sport.TENNIS;
        }

        public int Incremental { get; set; }
        public Sport SportName;

        public void ChangeIncremental()
        {
            if (Incremental >= MaxIncremental)
            {
                return;
            }
            Incremental = Incremental + 1;
        }
        public string GetCurrentSportNameToString()
        {
            return SportNamesMap[SportName];
        }
    }
}