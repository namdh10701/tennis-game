using UnityEngine;
using System;
using System.Collections;
using Common;
using Monetization.Ads.UI;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using ListExtensions;
using Enviroments;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;

namespace Monetization.Ads
{

    public class AdsController : SingletonPersistent<AdsController>
    {
        public enum AdType
        {
            BANNER, INTER, REWARD, OPEN, NATIVE
        }
        private IronSourceAds _ironsource;
        private AdmobAds _admob;

        private Action _onInterClosed;
        private Action<bool> _onRewardClosed;
        [HideInInspector] public Action OnRemoveAdsPurchased;

        private AdsUIController _adsUIController;


        [HideInInspector]public bool InterCanceled = false;

        [HideInInspector] public bool HasBanner;
        [HideInInspector] public bool IsShowingOpenAd;
        [HideInInspector] public bool IsShowingInterAd;
        [HideInInspector] public bool IsShowingReward;
        [HideInInspector] public bool RewardedAdJustClose;

        [SerializeField] private bool _isDebugNative;
        [SerializeField] private bool _isDebugBanner;
        [SerializeField] private bool _isDebugOpen;
        [SerializeField] private bool _isDebugRewarded;
        [SerializeField] private bool _isDebugInter;

        [SerializeField] private int _nativeAdPanelNumber;


        public bool HasInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public bool RemoveAds { get; set; }

        protected override void Awake()
        {
            base.Awake();
            AdsLogger.IsDebugBanner = _isDebugBanner;
            AdsLogger.IsDebugRewarded = _isDebugRewarded;
            AdsLogger.IsDebugInter = _isDebugInter;
            AdsLogger.IsDebugNative = _isDebugNative;
            AdsLogger.IsDebugOpen = _isDebugOpen;

            CachedNativeAds = new CachedNativeAd[_nativeAdPanelNumber];
            _nativeAdPanels = new NativeAdPanel[_nativeAdPanelNumber];
            _ironsource = GetComponent<IronSourceAds>();
            _admob = GetComponent<AdmobAds>();
            IsShowingOpenAd = false;
            IsShowingInterAd = false;
        }

        public void Init()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            _ironsource.Init();
            _admob.Init();
        }

        #region COMMON
        public void RegisterAdsUI(AdsUIController adsUI)
        {
            _adsUIController = adsUI;
        }
        public void UnregisterAdsUI(AdsUIController adsUI)
        {
            if (_adsUIController == adsUI)
                _adsUIController = null;
        }
        public void OnRemoveAds(bool removeAdsPuchased)
        {
            RemoveAds = removeAdsPuchased;
            _ironsource.ToggleBanner(!removeAdsPuchased);
            if (_nativeAdPanels != null)
            {
                foreach (NativeAdPanel panel in _nativeAdPanels)
                {
                    if (panel != null)
                    {
                        panel.gameObject.SetActive(!removeAdsPuchased);
                    }
                }
            }
        }

        #endregion
        #region NativeAd
        public CachedNativeAd[] CachedNativeAds;
        private NativeAdPanel[] _nativeAdPanels;

        public void RegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            AdsLogger.Log($"Panel registered with ID: {nativeAdPanel.ID}", AdType.NATIVE);
            _nativeAdPanels[0] = nativeAdPanel;
            if (CachedNativeAds[nativeAdPanel.ID] != null)
            {
                nativeAdPanel.CachedNativeAd = CachedNativeAds[nativeAdPanel.ID];
                return;
            }
            else
            {
                int newIndex = -1;
                for (int i = nativeAdPanel.ID + 1; i < CachedNativeAds.Length; i++)
                {
                    if (CachedNativeAds[i] != null)
                    {
                        newIndex = i;
                        nativeAdPanel.CachedNativeAd = CachedNativeAds[i];
                        break;
                    }
                }
                if (newIndex == -1)
                {
                    for (int i = nativeAdPanel.ID - 1; i >= 0; i--)
                    {
                        if (CachedNativeAds[i] != null)
                        {
                            newIndex = i;
                            break;
                        }
                    }
                    if (newIndex != -1)
                    {
                        nativeAdPanel.CachedNativeAd = CachedNativeAds[newIndex];
                        AdsLogger.Log($"Registered panel with ID: {nativeAdPanel.ID} has assigned cached native ad at pos: {nativeAdPanel.ID} ", AdType.NATIVE);
                    }
                    else
                    {
                        AdsLogger.Log($"Registered panel with ID: {nativeAdPanel.ID} has no available cached native ad", AdType.NATIVE);
                    }
                }
            }

        }
        public void UnRegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            if (_nativeAdPanels.Contains(nativeAdPanel))
            {
                _nativeAdPanels[nativeAdPanel.ID] = null;
            }
            AdsLogger.Log($"Panel with ID: {nativeAdPanel.ID} unregisterd", AdType.NATIVE);
        }

        public void OnNativeAdLoaded(NativeAd nativeAd)
        {
            AdsLogger.Log("Native ad unit loaded", AdType.NATIVE);
            CachedNativeAd cachedNativeAd = new CachedNativeAd(nativeAd);
            int lastPos = 0;
            int emptySlotIndex = -1;
            for (int i = 0; i < CachedNativeAds.Length; i++)
            {
                if (CachedNativeAds[i] == null)
                {
                    AdsLogger.Log("native ad loaded", AdType.NATIVE);
                    emptySlotIndex = i;
                    break;
                }
            }
            if (emptySlotIndex != -1)
            {
                AdsLogger.Log("native ad loaded", AdType.NATIVE);
                Debug.Log(emptySlotIndex);
                CachedNativeAds[emptySlotIndex] = cachedNativeAd;
                AdsLogger.Log($"Native ad unit cached into pos: {emptySlotIndex}", AdType.NATIVE);
                lastPos = emptySlotIndex;
            }
            else
            {
                int index = 0;
                int maxTimeShown = CachedNativeAds[0].TimeShown;
                for (int i = 1; i < CachedNativeAds.Length; i++)
                {
                    if (CachedNativeAds[i].TimeShown > maxTimeShown)
                    {
                        maxTimeShown = CachedNativeAds[i].TimeShown;
                        index = i;
                    }
                }
                CachedNativeAds[index] = cachedNativeAd;
                AdsLogger.Log($"Native ad unit cached replace pos: {index}", AdType.NATIVE);
                lastPos = index;
            }
            OnPanelAssignUpdate(lastPos);

        }

        private void OnPanelAssignUpdate(int panelID)
        {
            if (_nativeAdPanels[panelID] != null)
            {
                _nativeAdPanels[panelID].CachedNativeAd = CachedNativeAds[panelID];
                AdsLogger.Log($"Panel with ID: {panelID} Updated native ad unit at pos : {panelID}", AdType.NATIVE);
            }
            else
            {
                for (int i = 0; i < _nativeAdPanels.Length; i++)
                {
                    if (_nativeAdPanels[i] != null && _nativeAdPanels[i].CachedNativeAd != null)
                    {
                        _nativeAdPanels[i].CachedNativeAd = CachedNativeAds[panelID];
                        AdsLogger.Log($"Panel with ID: {_nativeAdPanels[i].ID} Updated native ad unit at pos : {panelID}", AdType.NATIVE);
                    }
                }
            }
        }

        public void LoadNativeAds()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            if (!HasInternet)
            {
                return;
            }
            AdsLogger.Log($"Load native ads", AdType.NATIVE);
            _admob.LoadNativeAds();
        }
        public void ShowNativeAd(NativeAdPanel nativeAdPanel)
        {
            if (RemoveAds)
            {
                return;
            }
            if (_nativeAdPanels.Contains(nativeAdPanel))
            {
                int count = 0;
                for (int i = 0; i < CachedNativeAds.Length; i++)
                {
                    if (CachedNativeAds[i] != null)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    AdsLogger.Log($"Empty cached native ads", AdType.NATIVE);
                    _admob.LoadNativeAds();
                }

                nativeAdPanel.Show();
                AdsLogger.Log($"Panel with ID: {nativeAdPanel.ID} show", AdType.NATIVE);
            }
        }
        public void HideNativeAd(NativeAdPanel nativeAdPanel)
        {
            if (_nativeAdPanels.Contains(nativeAdPanel))
                nativeAdPanel.Hide();
        }
        #endregion
        #region OpenAd
        public void ShowAppOpenAd()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            if (RewardedAdJustClose)
            {
                RewardedAdJustClose = false;
                return;
            }
            if (!AdsIntervalValidator.IsValidInterval(AdType.OPEN))
            {
                return;
            }
            if (_admob.IsAppOpenAdAvailable)
            {
                AdsLogger.Log("Show", AdType.OPEN);
                _admob.ShowAppOpenAd();
            }
        }
        #endregion
        #region Reward
        public void OpenNotRewardedPanel()
        {
            _adsUIController.ShowNotRewardedBox();
        }
        public void OpenNotAvailableRewardedPanel()
        {
            _adsUIController.ShowRewardUnavailableBox();
        }
        public void ShowReward(Action<bool> watched)
        {
            _onRewardClosed = watched;
            if (Enviroment.ENV == Enviroment.Env.DEV)
            {
                _onRewardClosed.Invoke(true);
                return;
            }
            if (IsShowingInterAd || IsShowingOpenAd)
            {
                return;
            }
            if (_ironsource.IsRewardReady)
            {
                _ironsource.ShowReward();
            }

            else if (!HasInternet)
            {
                _adsUIController.ShowRewardUnavailableBox();
            }
            else
            {
                StartCoroutine(WaitForRewardVideo());
            }

            IEnumerator WaitForRewardVideo()
            {
                //_ironsource.LoadReward();
                _adsUIController.ShowWaitingBox();
                yield return new WaitForSecondsRealtime(3f);
                _adsUIController.CloseWaitingBox().onComplete +=
                    () =>
                    {
                        if (_ironsource.IsRewardReady)
                        {
                            _ironsource.ShowReward();
                        }
                        else
                        {
                            _adsUIController.ShowRewardUnavailableBox();
                        }
                    };
            }
        }
        public void InvokeOnRewarded(bool rewarded)
        {
            _onRewardClosed?.Invoke(rewarded);
            _onRewardClosed = null;
        }

        #endregion
        #region Banner
        public void ShowBanner()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            if (HasBanner || !HasInternet)
                return;
            _ironsource.LoadBanner();
        }
        public void ToggleBanner(bool visible)
        {
            _ironsource.ToggleBanner(visible);
        }
        #endregion
        #region Inter
        public void InvokeOnInterClose()
        {
            _onInterClosed?.Invoke();
            _onInterClosed = null;
        }
        public void ShowInter(Action onInterClosed)
        {
            _onInterClosed = onInterClosed;
            if (InterCanceled)
            {
                _onInterClosed.Invoke();
                return;
            }

            if (IsShowingInterAd || IsShowingOpenAd)
            {
                _onInterClosed.Invoke();
                return;
            }
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
            {
                _onInterClosed.Invoke();
                return;
            }
            if (!AdsIntervalValidator.IsValidInterval(AdType.INTER))
            {
                _onInterClosed.Invoke();
                return;
            }
            if (_ironsource.IsInterReady)
            {
                _ironsource.ShowInter();
            }
            else
            {
                _onInterClosed.Invoke();
            }
        }
        #endregion


    }
}