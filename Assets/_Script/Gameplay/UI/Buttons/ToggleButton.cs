using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using JetBrains.Annotations;

namespace UI
{
    public class ToggleButton : MonoBehaviour
    {
        public enum ToggleState
        {
            ON, OFF
        }
        private ToggleState _currentState;
        [SerializeField] private float _toggleDuration;
        [SerializeField] private Image _onImage;
        [SerializeField] private Image _offImage;
        [SerializeField] private Image _background;

        [SerializeField] private TextMeshProUGUI _onText;
        [SerializeField] private TextMeshProUGUI _offText;
        private float _onImageOriginalPosX;
        private float _offImageOriginalPosX;

        private bool _isTransitioning = false;

        public UnityEvent _onAction;
        public UnityEvent _offAction;

        [SerializeField] private Button _button;
        private void Awake()
        {
            _onImageOriginalPosX = _onImage.transform.localPosition.x;
            _offImageOriginalPosX = _offImage.transform.localPosition.x;
            _button.onClick.AddListener(
                () => Toggle()
            );
        }

        public void Init(ToggleState state)
        {
            _currentState = state;
            SetDefaultPos(state);
        }

        private void SetDefaultPos(ToggleState state)
        {
            if (state == ToggleState.ON)
            {
                _onImage.gameObject.SetActive(true);
                _offImage.gameObject.SetActive(false);
            }
            else
            {
                _offImage.gameObject.SetActive(true);
                _onImage.gameObject.SetActive(false);
            }
        }

        public void Init(UnityEvent onAction, UnityEvent offAction)
        {
            _onAction = onAction;
            _offAction = offAction;
        }

        public void Toggle(ToggleState state)
        {
            if (_isTransitioning)
            {
                return;
            }
            _isTransitioning = true;
            _currentState = state;

            if (state == ToggleState.ON)
            {
                _onAction?.Invoke();
                HandleToggleOnAnim();
            }
            else
            {
                _offAction?.Invoke();
                HandleToggleOffAnim();
            }

            void HandleToggleOffAnim()
            {
                _onImage.DOFade(1, 0);
                _onImage.DOFade(1, 0);
                _onImage.transform.DOLocalMoveX(_onImageOriginalPosX, 0);
                _onImage.transform.DOLocalMoveX(0, _toggleDuration / 2).OnPlay(
                    () =>
                    {
                        _onImage.DOFade(0, _toggleDuration / 3);
                        _onText.DOFade(0, _toggleDuration / 3);
                    }
                    ).OnComplete(
                    () =>
                    {
                        _onImage.gameObject.SetActive(false);
                        _isTransitioning = false;
                    }
                    );

                _offImage.gameObject.SetActive(true);
                _offImage.DOFade(0, 0);
                _offImage.transform.localPosition = Vector3.zero;
                _offImage.transform.DOLocalMoveX(_offImageOriginalPosX, _toggleDuration / 2).OnPlay(
                    () =>
                    {
                        _offImage.DOFade(1, _toggleDuration / 3);
                        _offText.DOFade(1, _toggleDuration / 3);
                    }
                    );
            }

            void HandleToggleOnAnim()
            {
                _offText.DOFade(1, 0);
                _offImage.DOFade(1, 0);
                _offImage.transform.DOLocalMoveX(_offImageOriginalPosX, 0);
                _offImage.transform.DOLocalMoveX(0, _toggleDuration / 2).OnPlay(
                    () =>
                    {
                        _offImage.DOFade(0, _toggleDuration / 3);
                        _offText.DOFade(0, _toggleDuration / 3);
                    }
                    ).OnComplete(
                    () =>
                    {
                        _offImage.gameObject.SetActive(false);
                        _isTransitioning = false;
                    }

                    );

                _onImage.gameObject.SetActive(true);
                _onImage.DOFade(0, 0);
                _onImage.transform.localPosition = Vector3.zero;
                _onImage.transform.DOLocalMoveX(_onImageOriginalPosX, _toggleDuration / 2).OnPlay(
                    () =>
                    {
                        _onImage.DOFade(1, _toggleDuration / 3);
                        _onText.DOFade(1, _toggleDuration / 3);
                    }
                    );
            }
        }
        public void Toggle()
        {
            Toggle(_currentState == ToggleState.ON ? ToggleState.OFF : ToggleState.ON);
        }
    }
}
