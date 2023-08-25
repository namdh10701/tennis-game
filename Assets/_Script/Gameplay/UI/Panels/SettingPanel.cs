using Gameplay;
using UI;
using UnityEngine;
using static UI.ToggleButton;
using Monetization.Ads.UI;
using Monetization.Ads;
using TMPro;
using Audio;
using System.Collections;

public class SettingPanel : InterPopup
{
    [SerializeField] private ToggleButton _musicButton;
    [SerializeField] private ToggleButton _soundButton;
    [SerializeField] private ToggleButton _vibrateButton;
    [SerializeField] private ToggleButton _reversedButton;
    [SerializeField] private NativeAdPanel _nativeAdPanel;
    [SerializeField] private TextMeshProUGUI _versionName;
    private void OnEnable()
    {
        StartCoroutine(WaitAndShowNativeAds());
    }
    private void OnDisable()
    {
        AdsController.Instance.HideNativeAd(_nativeAdPanel);
    }
    public void Init()
    {
        _musicButton.Init(SettingManager.Instance.GameSettings.IsMusicOn ? State.ON : State.OFF);
        _soundButton.Init(SettingManager.Instance.GameSettings.IsSoundOn ? State.ON : State.OFF);
        _vibrateButton.Init(SettingManager.Instance.GameSettings.IsVibrationOn ? State.ON : State.OFF);
        _reversedButton.Init(SettingManager.Instance.GameSettings.IsReversed ? State.ON : State.OFF);

        _versionName.text = "Version " + Application.version.ToString();
    }

    public void SetMusic(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsMusicOn = isOn;
        SettingManager.Instance.SaveSettings();
        AudioController.Instance.ToggleMusic(isOn);
    }
    public void SetSound(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsSoundOn = isOn;
        SettingManager.Instance.SaveSettings();
        AudioController.Instance.ToggleSound(isOn);
    }
    public void SetVibrate(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsVibrationOn = isOn;
        Vibration.SetState(isOn);
        SettingManager.Instance.SaveSettings();
    }

    public void SetReversed(bool isReversed)
    {
        SettingManager.Instance.GameSettings.IsReversed = isReversed;
        SettingManager.Instance.SaveSettings();

    }

    private IEnumerator WaitAndShowNativeAds()
    {
        while (!_nativeAdPanel.IsRegistered)
        {
            yield return null;
        }
        if (AdsHandler.AdRemoved())
        {
            AdsController.Instance.HideNativeAd(_nativeAdPanel);
            yield break;
        }
        else
        {
            Debug.Log("show native ads from setting");
            AdsController.Instance.ShowNativeAd(_nativeAdPanel);
        }
    }
}

