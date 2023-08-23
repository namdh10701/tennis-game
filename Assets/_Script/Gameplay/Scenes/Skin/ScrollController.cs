using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class ScrollController : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private RectTransform _contentRect;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField] private Canvas _canvas;
        private float _totalContentHeight;
        [SerializeField] private RectTransform _labelTransform;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _contentRect = _scrollRect.content;

            CalculateTotalContentHeight();
            UpdateContentHeight();
        }

        private void CalculateTotalContentHeight()
        {
            _totalContentHeight = 0f;
            _totalContentHeight += _contentRect.GetChild(0).GetComponent<RectTransform>().rect.height;
            _totalContentHeight += _contentRect.GetChild(1).GetComponent<RectTransform>().rect.height;
            for (int i = 0; i < _verticalLayoutGroup.transform.childCount; i++)
            {
                _totalContentHeight += _verticalLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
            }
        }

        private void UpdateContentHeight()
        {
            _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x,
                _totalContentHeight - (_canvas.GetComponent<RectTransform>().rect.height - _labelTransform.rect.height));
            _scrollRect.verticalNormalizedPosition = 1; // Scroll to top after resizing
        }

        // You might also want to call this method whenever you add or remove content dynamically
        public void RecalculateContentHeight()
        {
            CalculateTotalContentHeight();
            UpdateContentHeight();
        }
    }
}
