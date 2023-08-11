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

    public class AdsController : SingletonPersistent<AdsController>
    {
        public enum AdType
        {
            BANNER, INTER, REWARD, OPEN
        }

        public enum RewardType
        {

        }

        private IronSourceAds _ironsource;
        private AdmobAds _admob;

        public RewardType rewardType;
        public Action onInterClosed;
        public Action<bool> onRewardClosed;
        public Action onOpenAdClosed;
        public Action OnRemoveAdsPurchased;

        public AdsUIController adsUIController;

        private List<NativeAdPanel> _nativeAdPanels = new List<NativeAdPanel>();

        public bool HasBanner { get; set; }
        public bool IsShowingAd { get; set; }
        public bool HasInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public bool RemoveAds { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _ironsource = GetComponent<IronSourceAds>();
            _admob = GetComponent<AdmobAds>();
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;

            CachedNativeAds = new List<CachedNativeAd>();
            IsShowingAd = false;
            _ironsource.Init();
            _admob.Init();
        }

        public void SetInterval(AdType type)
        {
            switch (type)
            {
                case AdType.REWARD:
                    break;
                case AdType.INTER:
                    break;
                case AdType.OPEN:
                    break;
            }
        }
        public bool CheckInterval(AdType type)
        {
            bool ret = true;
            switch (type)
            {
                case AdType.REWARD:
                    break;
                case AdType.INTER:
                    break;
                case AdType.OPEN:
                    break;
            }
            return ret;
        }

        #region NativeAd

        private const int NATIVE_AD_CACHED_TIMEOUT_MINUTES = 30;
        public const int MAX_NATIVE_AD_CACHE_SIZE = 4;

        public List<CachedNativeAd> CachedNativeAds { get; private set; }
        public void RegisterNativeAdPanel(NativeAdPanel nativeAdPanel)
        {
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
                return;
            _nativeAdPanels.Add(nativeAdPanel);
            CachedNativeAd cachedNativeAd;
            if (CachedNativeAds.GetFirst(out cachedNativeAd))
            {
                if (IsCachedNativeAdTimeout(cachedNativeAd))
                    cachedNativeAd.Disolve();
                else
                {
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
            if (!nativeAdPanel.IsNativeAdShowed)
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
            }
            else
            {
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
                    nativeAdPanel.CachedNativeAd = cachedNativeAd;
                    isAssigned = true;
                }
            }
            return isAssigned;
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
                nativeAdPanel.Show();
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
            if (_admob.IsAppOpenAdAvailable)
            {
                _admob.ShowAppOpenAd();
            }
            // else if (other ad source)
        }
        #endregion
        #region Reward
        public void ShowReward(Action<bool> watched)
        {
            onRewardClosed = watched;
            if (Enviroment.ENV == Enviroment.Env.DEV)
                onRewardClosed.Invoke(true);
            if (_ironsource.IsRewardReady)
            {
                _ironsource.ShowReward();
            }
            //else if(other ad source)
            else if (!HasInternet)
            {
                onRewardClosed.Invoke(false);
                onRewardClosed = null;
            }
            else
            {
                StartCoroutine(WaitForRewardVideo());
            }

            IEnumerator WaitForRewardVideo()
            {
                _ironsource.LoadReward();
                //load ad here 
                adsUIController.ShowWaitingBox();
                yield return new WaitForSeconds(3f);
                adsUIController.CloseWaitingBox();
                if (_ironsource.IsRewardReady)
                {
                    _ironsource.ShowReward();
                }
                //else if(other ad source)
                else
                {
                    adsUIController.ShowRewardUnavailableBox();
                    onRewardClosed.Invoke(false);
                    onRewardClosed = null;
                }
            }
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
            if (RemoveAds || Enviroment.ENV == Enviroment.Env.DEV)
            {
                onInterClosed.Invoke();
                return;
            }
            this.onInterClosed = onInterClosed;
            if (_ironsource.IsInterReady)
            {
                _ironsource.ShowInter();
            }
            // else if (other.isInterReady)
        }
        #endregion
    }
}