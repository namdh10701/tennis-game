using Gameplay;
using UI;
using UnityEngine;
using static UI.ToggleButton;

public class SettingPopup : MonoBehaviour
{
    private SettingManager _settingManager;
    [SerializeField] private ToggleButton _musicButton;
    [SerializeField] private ToggleButton _soundButton;
    [SerializeField] private ToggleButton _vibrateButton;

    public void Init(SettingManager settingManager)
    {
        _settingManager = settingManager;
        _musicButton.Init(_settingManager.GameSettings.IsMusicOn ? State.ON : State.OFF);
        _soundButton.Init(_settingManager.GameSettings.IsSoundOn ? State.ON : State.OFF);
        _vibrateButton.Init(_settingManager.GameSettings.IsVibrationOn ? State.ON : State.OFF);
    }

    public void SetMusic(bool isOn)
    {
        _settingManager.GameSettings.IsMusicOn = isOn;
        _settingManager.SaveSettings();
    }
    public void SetSound(bool isOn)
    {
        _settingManager.GameSettings.IsSoundOn = isOn;
        _settingManager.SaveSettings();
    }
    public void SetVibrate(bool isOn)
    {
        _settingManager.GameSettings.IsVibrationOn = isOn;
        _settingManager.SaveSettings();
    }
}

