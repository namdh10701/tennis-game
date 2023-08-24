using Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedButton : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button _button;
    private Transform _transform;
    [SerializeField] private UnityEvent _onClickEvent;
    [SerializeField] private bool _isSpamable;
    [SerializeField] private float _clickCooldownDuration = .5f; // Adjust the duration as needed

    private bool _isCooldown;
    private bool _clickedDown = false;

    protected override void Awake()
    {
        _clickCooldownDuration = .2f;
        _button = GetComponent<Button>();
        _transform = GetComponent<Transform>();
    }
    public void SetOnClickEnvent(UnityAction onClick)
    {
        _onClickEvent.AddListener(onClick);
    }
    public void RemoveOnClickEvent()
    {
        _onClickEvent.RemoveAllListeners();
    }
    public void SetEnable(bool IsEnable)
    {
        _button.interactable = IsEnable;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.enabled && !_isCooldown)
        {
            _clickedDown = true;
            _transform.localScale *= .9f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.enabled && !_isCooldown && _clickedDown)
        {
            Vector2 localPoint;
            _transform.localScale /= .9f;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _button.GetComponent<RectTransform>(),
                eventData.position, eventData.pressEventCamera, out localPoint))
            {
                if (_button.GetComponent<RectTransform>().rect.Contains(localPoint))
                {
                    AudioController.Instance.PlaySound("button");
                    _onClickEvent?.Invoke();
                }
            }
            _clickedDown = false;

            if (!_isSpamable)
            {
                StartCoroutine(StartCooldown());
            }
        }
    }
    IEnumerator StartCooldown()
    {
        _isCooldown = true;
        _button.enabled = false;
        yield return new WaitForSeconds(_clickCooldownDuration);
        _button.enabled = true;
        _isCooldown = false;
    }
    protected override void OnEnable()
    {
        _button.enabled = true;
        _isCooldown = false;
    }
}