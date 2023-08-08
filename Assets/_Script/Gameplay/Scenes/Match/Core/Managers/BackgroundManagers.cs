using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    //ToDo: 
    public class BackgroundManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        [SerializeField] SpriteRenderer _background;
        List<Sprite> availableBackgrounds;

        public void Init(MatchEvent matchEvent)
        {
            _matchEvent = matchEvent;
            availableBackgrounds = new List<Sprite>();
        }

        private void ChangeBackground()
        {

        }
    }
}