using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Tilemaps;
using Common;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using System.Collections;

namespace Monetization.IAP
{

    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private CodelessIAPButton IAPButtons;

        public void OnPurchaseCompelete(Product product)
        {
            Debug.Log("purchased success");
            PlayerPrefs.SetInt(Constant.ADS_REMOVED_KEY, 1);
            AdsController.Instance.OnRemoveAds();
            FirebaseAnalytics.Instance.PushEvent("REMOVES_ADS_PURCHASED");
            FirebaseAnalytics.Instance.PushEvent("REMOVES_ADS_CLICK");
            StartCoroutine(HideButton());

        }

        private IEnumerator HideButton()
        {
            yield return new WaitForSeconds(.2f);
            IAPButtons.gameObject.SetActive(false);
        }

        public void onPurchaseFailed(Product product, PurchaseFailureDescription description)
        {
            Debug.Log($"Purchase failed: {product}, {description}");
            FirebaseAnalytics.Instance.PushEvent("REMOVES_ADS_CLICK");
        }

        public void OnProductFetched(Product product)
        {
            if (product.receipt != null)
            {
                Debug.Log("Player has purchased removed ads");
                PlayerPrefs.SetInt(Constant.ADS_REMOVED_KEY, 1);
                IAPButtons.gameObject.SetActive(false);
                AdsController.Instance.OnRemoveAds();
            }
            else
            {
                Debug.Log("Player has not purchased removed ads");
                PlayerPrefs.SetInt(Constant.ADS_REMOVED_KEY, 0);
            }

        }
    }
}