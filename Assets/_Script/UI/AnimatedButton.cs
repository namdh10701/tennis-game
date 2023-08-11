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
    [SerializeField] protected UnityEvent _onClickEvent;
    public UnityEvent OnClickEvent
    {
        get
        {
            return _onClickEvent;
        }
        set
        {
            _onClickEvent = value;
        }
    }
    [SerializeField] private bool _isSpamable;
    [SerializeField] private float _clickCooldownDuration = .5f; // Adjust the duration as needed

    private bool _isCooldown;
    private bool _clickedDown = false;

    protected override void Awake()
    {
        _button = GetComponent<Button>();
        _transform = GetComponent<Transform>();
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
            //AudioController.Instance.PlaySound("button");
            _onClickEvent.Invoke();
            _transform.localScale /= .9f;
            _clickedDown = false;
            if (!_isSpamable)
            {
                StartCoroutine(StartCooldown());
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
    }

    protected override void OnEnable()
    {
        _button.enabled = true;
        _isCooldown = false;
    }
}