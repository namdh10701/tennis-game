/*using UnityEngine;
using System;
using System.Collections;
using Common;

namespace Monetization.Ads
{

    public class AdsController : Singleton<AdsController>
    {
        public enum AdType
        {
            BANNER, INTER, REWARD, OPEN
        }

        public enum RewardType
        {

        }

        [SerializeField] private IronSourceAds ironsource;

        public RewardType rewardType;
        public Action onInterClosed;
        public Action<bool> onRewardClosed;
        public Action onOpenAdClosed;

        public AdsUIController adsUIController;
        public bool HasBanner { get; set; }
        public bool IsShowingAd { get; set; }
        public bool HasInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }

        protected override void Awake()
        {
            base.Awake();
            IsShowingAd = false;
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
        #region OpenAd
        public void ShowOpenAd()
        {
            *//*if (admob.OpenAdReady)
            {
                admob.ShowOpen();
            }
            // else if (other ad source)*//*
        }
        #endregion
        #region Reward
        public void ShowReward(Action<bool> watched)
        {
            onRewardClosed = watched;

            if (ironsource.IsRewardReady)
            {
                ironsource.ShowReward();
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
                ironsource.LoadReward();
                //load ad here 
                adsUIController.ShowWaitingBox();
                yield return new WaitForSeconds(3f);
                adsUIController.CloseWaitingBox();
                if (ironsource.IsRewardReady)
                {
                    ironsource.ShowReward();
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
            ironsource.ShowBanner();
        }
        public void ToggleBanner(bool visible)
        {
            ironsource.ToggleBanner(visible);
        }
        #endregion
        #region Inter
        public void ShowInter(Action onInterClosed)
        {
            this.onInterClosed = onInterClosed;
            if (ironsource.IsInterReady)
            {
                ironsource.ShowInter();
            }
            // else if (other.isInterReady)
        }
        #endregion
    }
}*/