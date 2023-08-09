using System;
using System.Collections.Generic;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    [Serializable]
    public class GameData
    {
        public float HighTime;
        public int HighScore;
        public MatchData LastGamePlayed;
        public List<Sport> UnlockedSports;
        /* List<Skin> UnlockedSkin;
         List<Skin> LockedSkin;*/
    }
}