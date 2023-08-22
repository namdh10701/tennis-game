using System;
namespace Gameplay
{
    [Serializable]
    public class GameSetting
    {
        public bool IsMusicOn;
        public bool IsSoundOn;
        public bool IsVibrationOn;
        public bool IsReversed;
        public GameSetting()
        {
            IsMusicOn = true;
            IsSoundOn = true;
            IsVibrationOn = true;
            IsReversed = true;
        }
    }
}
