
using Enviroments;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Services.Adjust
{
    public class Adjust : MonoBehaviour
    {
        public static void TrackIronsourceRevenue(IronSourceImpressionData impressionData)
        {
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                Debug.Log(impressionData.ToString());
                return;
            }

          /*  AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
            adRevenue.setRevenue((double)impressionData.revenue, "USD");
            adRevenue.setAdRevenueNetwork(impressionData.adNetwork);
            adRevenue.setAdRevenueUnit(impressionData.adUnit);
            adRevenue.setAdRevenuePlacement(impressionData.placement);
            com.adjust.sdk.Adjust.trackAdRevenue(adRevenue);*/
        }

        public static void TrackAdmobRevenue(AdValue adValue)
        {
            if (Enviroment.ENV != Enviroment.Env.PROD)
            {
                Debug.Log(adValue.ToString());
                return;
            }
           /* AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            adRevenue.setRevenue(adValue.Value / 1000000f, adValue.CurrencyCode);
            Debug.Log("=> admob Adjust Value : " + (adValue.Value));
            Debug.Log("=> admob Adjust 1000000 : " + (adValue.Value / 1000000f));
            com.adjust.sdk.Adjust.trackAdRevenue(adRevenue);*/
        }
    }
}