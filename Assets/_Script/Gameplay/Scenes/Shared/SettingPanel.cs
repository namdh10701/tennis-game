using Gameplay;
using UI;
using UnityEngine;
using static UI.ToggleButton;
using Monetization.Ads.UI;
using Monetization.Ads;
using Phoenix.Gameplay.Vibration;
using TMPro;
using Audio;
public class SettingPanel : InterPopup
{
    private SettingManager _settingManager;
    [SerializeField] private ToggleButton _musicButton;
    [SerializeField] private ToggleButton _soundButton;
    [SerializeField] private ToggleButton _vibrateButton;
    [SerializeField] private ToggleButton _reversedButton;
    [SerializeField] private NativeAdPanel _nativeAdPanel;
    [SerializeField] private TextMeshProUGUI _versionName;
    public void Init(SettingManager settingManager)
    {
        _settingManager = settingManager;
        _musicButton.Init(_settingManager.GameSettings.IsMusicOn ? State.ON : State.OFF);
        _soundButton.Init(_settingManager.GameSettings.IsSoundOn ? State.ON : State.OFF);
        _vibrateButton.Init(_settingManager.GameSettings.IsVibrationOn ? State.ON : State.OFF);

        Debug.Log(_settingManager.GameSettings.IsReversed);
        _reversedButton.Init(_settingManager.GameSettings.IsReversed ? State.ON : State.OFF);

        _versionName.text = "Version " + Application.version.ToString();
    }

    public void SetMusic(bool isOn)
    {
        _settingManager.GameSettings.IsMusicOn = isOn;
        _settingManager.SaveSettings();
        AudioController.Instance.ToggleMusic(isOn);
    }
    public void SetSound(bool isOn)
    {
        _settingManager.GameSettings.IsSoundOn = isOn;
        _settingManager.SaveSettings();
        AudioController.Instance.ToggleSound(isOn);
    }
    public void SetVibrate(bool isOn)
    {
        _settingManager.GameSettings.IsVibrationOn = isOn;
        Vibration.SetState(isOn);
        _settingManager.SaveSettings();
    }

    public void SetReversed(bool isReversed)
    {
        _settingManager.GameSettings.IsReversed = isReversed;
        _settingManager.SaveSettings();

        Debug.Log(_settingManager.GameSettings.IsReversed);
    }

    private void OnEnable()
    {
        AdsController.Instance.ShowNativeAd(_nativeAdPanel);
    }
}

