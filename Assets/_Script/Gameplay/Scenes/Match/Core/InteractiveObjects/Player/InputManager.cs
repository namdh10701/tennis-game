using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class InputManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private Vector2 touchStart;
        private Vector3 touchCurrent;
        private float threshold;
        private Transform _player;
        public void Init(MatchEvent matchEvent, Transform player)
        {
            _matchEvent = matchEvent;
            _player = player;
        }

        void Update()
        {
            //ToDo: switch state here 
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStart = touch.position;
                        break;
                    case TouchPhase.Moved:
                        float distance = (touch.position - touchStart).magnitude;
                        if (distance > threshold)
                        {
                            touchCurrent = touch.position;
                            Vector3 worldTouchCurrent = Camera.main.ScreenToWorldPoint(new Vector3(touchCurrent.x, touchCurrent.y, Camera.main.nearClipPlane));
                            _player.position = worldTouchCurrent;
                        }
                        break;
                    case TouchPhase.Ended:
                        break;
                }
            }
        }
    }
}