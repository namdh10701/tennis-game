using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Monetization.IAP
{

    /*public class IAPManager : MonoBehaviour
    {
        [SerializeField] private List<CodelessIAPButton> IAPButtons;
        private Dictionary<BillData.ID, CodelessIAPButton> IAPButtonsMap;

        private void Awake()
        {
            IAPButtonsMap = new Dictionary<BillData.ID, CodelessIAPButton>
            {
                {BillData.ID.remove_ads,IAPButtons[0]}
            };
        }

        public void OnPurchaseCompelete(Product product)
        {
            switch ((BillData.ID)Enum.Parse(typeof(BillData.ID), product.definition.id))
            {
                case BillData.ID.remove_ads:
                    break;
                    //other product ids
            }
        }

        public void onPurchaseFailed(Product product, PurchaseFailureDescription description)
        {
            Debug.Log($"Purchase failed: {product}, {description}");
        }

        public void onProductFetched(Product product)
        {
            if (product != null && product.hasReceipt)
            {
                ProductMetadata productMetadata = product.metadata;
                IAPButtonsMap[(BillData.ID)Enum.Parse(typeof(BillData.ID), product.definition.id)].buttonType = CodelessButtonType.Restore;
            }
            else
            {
                IAPButtonsMap[(BillData.ID)Enum.Parse(typeof(BillData.ID), product.definition.id)].buttonType = CodelessButtonType.Purchase;
                Debug.Log("Player has not purchased " + product.definition.id);
            }
        }

        public void OnTransactionsRestored(bool success, string error)
        {
            Debug.Log($"TransactionsRestored: {success}, {error}");
        }
    }*/
}