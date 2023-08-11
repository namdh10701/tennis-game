using GoogleMobileAds.Api;
using System;

namespace Monetization.Ads
{
    public class CachedNativeAd
    {
        private NativeAd _nativeAd;
        public DateTime CachedTime { get; private set; }
        public NativeAd NativeAd
        {
            get
            {
                return _nativeAd;
            }
            set
            {
               _nativeAd = value;
            }
        }
        public CachedNativeAd(NativeAd nativeAd)
        {
            _nativeAd = nativeAd;
            CachedTime = DateTime.Now;
        }
        public void Disolve()
        {
            _nativeAd.Destroy();
        }
    }
}