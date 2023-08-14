using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BackgroundManager : MonoBehaviour
    {
        public BackgroundAsset BackgroundAsset;
        private MatchEvent _matchEvent;
        [SerializeField] SpriteRenderer _background;
        private BackgroundColorOrder _backgroundColorOrder;
        private Dictionary<string, Sprite> _spritesStringMap;
        private int _currentColorIndex;

        //ToDo: read matchSetting here
        public void Init(BackgroundColorOrder backgroundColorOrder)
        {
            _spritesStringMap = new Dictionary<string, Sprite>()
            {
                { "red" , BackgroundAsset.red},
                { "yellow" , BackgroundAsset.yellow},
                { "green", BackgroundAsset.green},
                { "blue", BackgroundAsset.blue },
                { "purple", BackgroundAsset.purple},
                { "orange", BackgroundAsset.orange},
                { "pink", BackgroundAsset.pink}
            };
            _backgroundColorOrder = backgroundColorOrder;
        }

        public void UpdateBackground()
        {
            _currentColorIndex++;
            _background.sprite = _spritesStringMap[_backgroundColorOrder.Strings[_currentColorIndex % 7]];
        }
        public void Prepare()
        {
            _currentColorIndex = 0;
            _background.sprite = _spritesStringMap[_backgroundColorOrder.Strings[_currentColorIndex]];

        }
    }
}