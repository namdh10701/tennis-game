using System;
using System.Collections.Generic;
using Unity.Mathematics;
using static Gameplay.MatchSetting;

namespace Gameplay
{
    [Serializable]
    public class GameData
    {
        public enum TextType
        {
            COOL = 0, EXCELLENT = 1, NICE = 2, PERFECT = 3, WONDERFUL = 4, WOW = 5, YAY = 6
        }

        public float HighTime;
        public int HighScore;
        public MatchData LastGamePlayed;
        public List<Sport> UnlockedSports;
        public List<TextType> UnlockedTexts;
        public int UnlockedIncremental;
        public List<Skin> Skins;
    }
}