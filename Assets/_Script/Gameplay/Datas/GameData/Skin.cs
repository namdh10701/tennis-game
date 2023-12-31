using Monetization.Ads;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    [System.Serializable]
    public class Skin
    {
        public enum SkinType
        {
            RACKET = 0, HAND = 1, GLOVES = 2, BASEBAT = 3
        }

        public string ID;
        public SkinType Type; 
        public bool Unlocked;
        public bool BeingUsed;
    }
}