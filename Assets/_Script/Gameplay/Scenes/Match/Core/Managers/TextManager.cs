using System;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using ListExtensions;
using System.Drawing.Drawing2D;

namespace Gameplay
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private Image text;
        public TextAsset TextAsset;
        private MatchData _matchData;
        private List<Sprite> availableTexts;
        public void Init(MatchData matchData)
        {
            //ToDo: Check lại logic chỗ này(trên doc)
            availableTexts = new List<Sprite>();
            _matchData = matchData;
            for (int i = 0; i < matchData.MatchSettings.Incremental; i++)
            {
                if (i < TextAsset.TextSprites.Count)
                {
                    availableTexts.Add(TextAsset.TextSprites[i]);
                }
            }

        }
        public void DisplayText()
        {
            text.sprite = availableTexts.GetRandom();
            text.DOFade(1, 1f).onComplete += () =>
            {
                text.DOFade(0, 1f);
            };
        }

        public void UpdateAvailableText()
        {
            for (int i = 0; i < _matchData.MatchSettings.Incremental; i++)
            {
                if (i < TextAsset.TextSprites.Count)
                {
                    if (!availableTexts.Contains(TextAsset.TextSprites[i]))
                    {
                        availableTexts.Add(TextAsset.TextSprites[i]);
                    }
                }
            }
        }

        public void Prepare()
        {

        }
    }
}