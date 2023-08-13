using System;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace Gameplay
{
    //ToDo: 
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private Image text;
        private MatchData _matchData;
        public void Init(MatchData matchData)
        {
            _matchData = matchData;
        }
        public void DisplayText()
        {
            //ToDo: wirte list extension get random
            text.DOFade(1, .3f).onComplete += () =>
            {
                text.DOFade(0, .3f);
            };
        }

        public void Prepare()
        {
            
        }
    }
}