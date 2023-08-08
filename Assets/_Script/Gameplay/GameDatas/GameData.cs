using System;
using static MatchSetting;

namespace Gameplay
{
    [Serializable]
    public class GameData
    {
        public float HighTime;
        public int HighScore;
        public MatchData LastGamePlayed;
        /* List<Skin> UnlockedSkin;
         List<Skin> LockedSkin;*/
    }
}