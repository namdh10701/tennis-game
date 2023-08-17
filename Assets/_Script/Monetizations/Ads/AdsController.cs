using UnityEngine;
using System;
using System.Collections;
using Common;
using Monetization.Ads.UI;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using ListExtensions;
using Enviroments;
namespace Monetization.Ads
{

    public class AdsController : MonoBehaviour
    {
        public static AdsController Instance;
        public enum AdType
        {
            BANNER, INTER, REWARD, OPEN
        }

        private bool hasBanner;

        private IronSourceAds _ironsource;
        private AdmobAds _admob;

        private Action _onInterClosed;
        private Action<bool> _onRewardClosed;
        public Action OnRemoveAdsPurchased;

        private AdsUIController _adsUIController;

        private List<NativeAdPanel> _nativeAdPanels = new List<NativeAdPanel>();

        private bool _isFreeAdsTimeEnded = false;

        public bool HasBanner { get; set; }
        public bool IsShowingAd { get; set; }
        public bool HasInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public bool RemoveAds { get; set; }

        private void Awake()
        {
            Instance = this;
            _ironsource = GetComponent<IronSourceAds>();
            _admob = GetComponent<AdmobAds>();
            IsShowingAd = false;
        }

        public void Init()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            CachedNativeAds = new List<CachedNativeAd>();
            _ironsource.Init();
            _admob.Init();
            Invoke("EndFreeAdsTime", 30);
        }
        private void EndFreeAdsTime()
        {
            _isFreeAdsTimeEnded = true;
        }



        public void SetBanner(bool hasBanner)
        {
            if (hasBanner)
            {
                InvokeRepeating(nameof(ShowBanner), 0, 60);
            }
        }

        public void RegisterAdsUI(AdsUIController adsUI)
        {
            _adsUIController = adsUI;
        }
        public void UnregisterAdsUI(AdsUIController adsUI)
        {
            if (_adsUIController == adsUI)
                _adsUIController = null;
        }

        #region NativeAd
        public List<CachedNativeAd> CachedNativeAds { get; private set; }
        private const int NATIVE_AD_CACHED_TIMEOUT_MINUTES = 30;
        public const int MAX_NATIVE_AD_CACHE_SIZE = 4;

        public void RegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            _nativeAdPanels.Add(nativeAdPanel);
            CachedNativeAd cachedNativeAd;
            if (CachedNativeAds.GetFirst(out cachedNativeAd))
            {
                Debug.Log("get one native ad");
                if (IsCachedNativeAdTimeout(cachedNativeAd))
                {
                    cachedNativeAd.Disolve();
                    Debug.Log("get one native ad timeout");
                }

                else
                {
                    Debug.Log("assign a");
                    nativeAdPanel.CachedNativeAd = cachedNativeAd;
                }
            }
        }
        public void UnRegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            if (_nativeAdPanels.Contains(nativeAdPanel))
                _nativeAdPanels.Remove(nativeAdPanel);
            if (!nativeAdPanel.IsNativeAdShowed && nativeAdPanel.CachedNativeAd != null)
            {
                if (IsCachedNativeAdTimeout(nativeAdPanel.CachedNativeAd))
                {
                    nativeAdPanel.CachedNativeAd.Disolve();
                }
                else
                {
                    TryCacheNativeAd(nativeAdPanel.CachedNativeAd);
                }
            }
        }

        private void TryCacheNativeAd(CachedNativeAd cachedNativeAd)
        {
            if (CachedNativeAds.Count < MAX_NATIVE_AD_CACHE_SIZE)
            {
                CachedNativeAds.AddFirst(cachedNativeAd);
                Debug.Log($"cached native ad {CachedNativeAds.Count}");
            }
            else
            {
                Debug.Log($"native ad disolve");
                cachedNativeAd.Disolve();
            }
        }

        public bool IsCachedNativeAdTimeout(CachedNativeAd cachedNativeAd)
        {
            return DateTime.Now > cachedNativeAd.CachedTime + TimeSpan.FromMinutes(NATIVE_AD_CACHED_TIMEOUT_MINUTES);
        }

        public void OnNativeAdLoaded(NativeAd nativeAd)
        {
            CachedNativeAd cachedNativeAd = new CachedNativeAd(nativeAd);
            if (!AssignToAvailablePanel(cachedNativeAd))
                CachedNativeAds.AddLast(cachedNativeAd);
        }

        private bool AssignToAvailablePanel(CachedNativeAd cachedNativeAd)
        {
            bool isAssigned = false;
            foreach (NativeAdPanel nativeAdPanel in _nativeAdPanels)
            {
                if (nativeAdPanel.CachedNativeAd == null)
                {
                    Debug.Log("native ad assigned");
                    nativeAdPanel.CachedNativeAd = cachedNativeAd;
                    isAssigned = true;
                }
            }
            return isAssigned;
        }

        public void InvokeOnInterClose()
        {
            _onInterClosed?.Invoke();
            _onInterClosed = null;
        }

        public void OpenNotRewardedPanel()
        {
            _adsUIController.ShowNotRewardedBox();
        }

        public void LoadNativeAds()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            _admob.LoadNativeAds();
        }
        public void ShowNativeAd(NativeAdPanel nativeAdPanel)
        {
            if (RemoveAds)
                return;
            if (_nativeAdPanels.Contains(nativeAdPanel))
            {
                nativeAdPanel.Show();
                Debug.Log("native ad panel show request from ads controller");
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
            if (IsShowingAd)
            {
                return;
            }
            if (!AdsIntervalValidator.IsValidInterval(AdType.OPEN))
            {
                return;
            }
            if (_admob.IsAppOpenAdAvailable)
            {
                _admob.ShowAppOpenAd();
            }
        }
        #endregion
        #region Reward
        public void ShowReward(Action<bool> watched)
        {
            if (IsShowingAd)
            {
                return;
            }
            _onRewardClosed = watched;
            if (Enviroment.ENV == Enviroment.Env.DEV)
            {
                _onRewardClosed.Invoke(true);
                return;
            }
            if (IsShowingAd)
            {
                Debug.LogWarning("Another ad is being displayed");
                return;
            }
            if (_ironsource.IsRewardReady)
            {
                _ironsource.ShowReward();
            }
            //else if(other ad source)
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
                _ironsource.LoadReward();
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
            _onRewardClosed.Invoke(rewarded);
            _onRewardClosed = null;
        }

        #endregion
        #region Banner
        public void ShowBanner()
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            _ironsource.LoadBanner();
        }
        public void ToggleBanner(bool visible)
        {
            _ironsource.ToggleBanner(visible);
        }
        #endregion
        #region Inter
        public void ShowInter(Action onInterClosed)
        {
            _onInterClosed = onInterClosed;
            if (!_isFreeAdsTimeEnded)
            {
                _onInterClosed.Invoke();
                return;
            }
            if (IsShowingAd)
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
            if (IsShowingAd)
            {
                Debug.LogWarning("Another ad is being displayed");
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

        public void OnRemoveAds()
        {
            RemoveAds = true;
            _ironsource.ToggleBanner(false);
            _nativeAdPanels?.Clear();
            CachedNativeAds?.Clear();
        }
    }
}