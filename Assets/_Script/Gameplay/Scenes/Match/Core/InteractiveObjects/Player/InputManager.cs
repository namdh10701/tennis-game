using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class InputManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private Transform _player;
        private Vector3 offset;
        private bool _isReversed;
        private bool isDragging = false;
        public void Init(MatchEvent matchEvent, Transform player, bool isReversed)
        {
            _isReversed = isReversed;
            offset = new Vector3(0, .4f, 0);
            _matchEvent = matchEvent;
            _player = player;
        }

        void Update()
        {
#if UNITY_EDITOR
            HandleMouseInput();
#else
            HandleTouchInput();
#endif
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                switch (_matchEvent?.CurrentState)
                {
                    case MatchEvent.MatchState.PLAYING:
                        Vector3 worldMouseDelta = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                        _player.position = _isReversed ? worldMouseDelta - offset : worldMouseDelta + offset;
                        break;
                }
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        break;
                    case TouchPhase.Moved:
                        switch (_matchEvent?.CurrentState)
                        {
                            case MatchEvent.MatchState.PLAYING:

                                Vector3 worldTouchCurrent = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
                                _player.position = _isReversed ? worldTouchCurrent - offset : worldTouchCurrent + offset;

                                break;
                        }

                        break;
                    case TouchPhase.Ended:
                        break;
                }
            }
        }
    }
}