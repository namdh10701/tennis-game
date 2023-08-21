using System;
using System.Collections.Generic;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    [Serializable]
    public class GameData
    {
        public enum TextType
        {
            COOL, EXCELLENT, NICE, PERFECT, WONDERFUL, WOW, YAY
        }

        public float HighTime;
        public int HighScore;
        public MatchData LastGamePlayed;
        public List<Sport> UnlockedSports;
        public List<TextType> UnlockedTexts;
        public int UnlockedIncremental;

        public GameData()
        {
            HighScore = 0;
            HighTime = 0;
            LastGamePlayed = null;
            UnlockedSports = new List<Sport>();
            UnlockedIncremental = 1;
        }
        /* List<Skin> UnlockedSkin;
         List<Skin> LockedSkin;*/
    }
}