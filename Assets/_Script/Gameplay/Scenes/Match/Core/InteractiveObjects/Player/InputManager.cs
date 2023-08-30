using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gameplay
{
    public class InputManager : MonoBehaviour
    {
        private MatchEvent _matchEvent;
        private Transform _player;
        private Vector3 offset;
        private bool _isReversed;
        private bool isDragging = false;
        public SpriteRenderer boundary;
        public SpriteRenderer playerRenderer;
        Vector2 playerSize;
        private void Start()
        {
            boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<SpriteRenderer>();
            playerRenderer = _player.GetComponent<Player>().GetSpriteRenderer();

        }
        public void Init(MatchEvent matchEvent, Transform player, bool isReversed)
        {
            _isReversed = isReversed;
            offset = new Vector3(0, .3f, 0);
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
                        Vector3 clampedPosition = ClampToBoundary(worldMouseDelta);
                        _player.position = _isReversed ? clampedPosition - offset : clampedPosition + offset;
                        break;
                }
            }
        }

        private Vector3 ClampToBoundary(Vector3 position)
        {
            float boundaryMinX = -boundary.bounds.size.x / 2f;
            float boundaryMaxX = boundary.bounds.size.x / 2f;

            float boundaryMinY = -boundary.bounds.size.y / 2f;
            float boundaryMaxY = boundary.bounds.size.y / 2f;

            // Adjust the clamped position to account for the player's sprite size
            float playerWidth = playerRenderer.bounds.size.x;
            float playerHeight = playerRenderer.bounds.size.y;
            //position.x = Mathf.Clamp(position.x, boundaryMinX + playerWidth * .85f / 2, boundaryMaxX - playerWidth * .85f / 2);
            if (_isReversed)
            {
                position.y = Mathf.Clamp(position.y, -10000, boundaryMaxY - playerHeight * .85f / 2 + (_isReversed ? offset.y : -offset.y));
            }
            else
            {
                position.y = Mathf.Clamp(position.y, boundaryMinY + playerHeight * .85f / 2 + (_isReversed ? offset.y : -offset.y), 10000);

            }
            return position;
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
                                Vector3 clampedPosition = ClampToBoundary(worldTouchCurrent);
                                _player.position = _isReversed ? clampedPosition - offset : clampedPosition + offset;

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