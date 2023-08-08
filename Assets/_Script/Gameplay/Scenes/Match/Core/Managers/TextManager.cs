using UnityEngine;

namespace Gameplay
{
    //ToDo: 
    public class TextManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        public void Init(MatchEvent matchEvent)
        {
            _matchEvent = matchEvent;
        }

        public void SpawnText(Vector2 position)
        {

        }
    }
}