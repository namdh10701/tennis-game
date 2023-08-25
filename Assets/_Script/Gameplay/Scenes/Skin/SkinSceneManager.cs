using Monetization.Ads;
using Monetization.Ads.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SkinSceneManager : MonoBehaviour
    {
        [SerializeField] private SkinUI _skinUI;
        private List<Skin> skins;
        [SerializeField] private NativeAdPanel _adPanel;
        private void Start()
        {
            _skinUI.Init(GameDataManager.Instance.GameDatas.Skins);
            AdsController.Instance.ShowNativeAd(_adPanel);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
