using UnityEngine;
using Common;

namespace Monetization.Ads
{
    public class AdsController : SingletonPersistent<AdsController>
    {
        private AdsUIController _adsUIController;

        protected override void Awake()
        {
            base.Awake();
            _adsUIController = FindObjectOfType<AdsUIController>();
        }

        public void ShowOpenAds()
        {

        }
    }
}
