﻿using System;

namespace Gameplay
{
    public class MatchData
    {
        public MatchSetting MatchSettings;
        public float ElapsedTime;
        public int Score;

        public MatchData(MatchSetting matchSettings)
        {
            MatchSettings = matchSettings;
            ElapsedTime = 0;
            Score = 0;
        }

        public void ResetMatchData()
        {
            ElapsedTime = 0;
            Score = 0;
        }
    }
}