using UnityEngine;

using Services.FirebaseService;
using Services.FirebaseService.Remote;
using Enviroments;
using Monetization.Ads;
using Audio;
using Common;
using System.Collections;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;
using com.adjust.sdk;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public MatchSetting MatchSetting = new MatchSetting();

        public void ResetMatchSetting()
        {
            MatchSetting = new MatchSetting();
        }
        [SerializeField] private Enviroment.Env env;
        private void Awake()
        {
            SpecifyEnviroment();
            SpecifySystemsSetting();
        }
        private void SpecifyEnviroment()
        {
            Enviroment.ENV = env;
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                //Debug.unityLogger.logEnabled = false;
            }
        }
        private static void SpecifySystemsSetting()
        {
            Vibration.SetState(SettingManager.Instance.GameSettings.IsVibrationOn);
            AudioController.Instance.Init(SettingManager.Instance.GameSettings.IsMusicOn, SettingManager.Instance.GameSettings.IsSoundOn);
            Application.targetFrameRate = 60;
            FirebaseHandler.Init();
            AdjustHandler.Init();
        }
    }
}