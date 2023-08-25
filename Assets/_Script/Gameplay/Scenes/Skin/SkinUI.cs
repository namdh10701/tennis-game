using Common;
using ListExtensions;
using Monetization.Ads;
using Monetization.Ads.UI;
using Phoenix;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
namespace Gameplay
{
    public class SkinUI : MonoBehaviour
    {
        [SerializeField] GameObject SkinRowPrefab;

        public SkinAsset SkinAsset;
        private const int EXIST_SKIN_ROW = 5;
        private const int SKIN_PER_ROW = 4;
        [SerializeField] private Transform skinRowsHolder;
        [SerializeField] private SceneTransition _sceneTransition;
        [SerializeField] private Transform _scrollRect;
        [SerializeField] private List<SkinButton> _skinButtons;

        [SerializeField] private GameObject _nativeAdPanelHolder;
        [SerializeField] private NativeAdPanel _nativeAdPanel;
        private List<Skin> _skins;

        public void Init(List<Skin> skins)
        {
            _skins = skins;
            CreateRows();
            _nativeAdPanelHolder.SetActive(!AdsHandler.AdRemoved());
            _skinButtons = _scrollRect.GetComponentsInChildren<SkinButton>().ToList();
            HandleSkinButtons();
            StartCoroutine(WaitAndShowNativeAds());
        }
        private IEnumerator WaitAndShowNativeAds()
        {
            while (!_nativeAdPanel.IsRegistered)
            {
                yield return null;
            }
            if (AdsHandler.AdRemoved())
            {
                AdsController.Instance.HideNativeAd(_nativeAdPanel);
                yield break;
            }
            else
            {
                Debug.Log("show native ads from setting");
                AdsController.Instance.ShowNativeAd(_nativeAdPanel);
            }
        }

        private void HandleSkinButtons()
        {
            for (int i = 0; i < _skinButtons.Count - _skins.Count; i++)
            {
                _skinButtons[_skinButtons.Count - 1 - i].Disalbe();
            }

            for (int i = 0; i < _skins.Count; i++)
            {
                if (!_skinButtons[i].Disabled)
                    _skinButtons[i].Init(this, _skins[i], SkinAsset.skinSprites[int.Parse(_skins[i].ID) - 1]);
            }
        }

        private void CreateRows()
        {
            int numberOfRow = _skins.Count / SKIN_PER_ROW + 1;
            if (numberOfRow > EXIST_SKIN_ROW)
            {
                for (int i = 0; i < numberOfRow - EXIST_SKIN_ROW; i++)
                {
                    Instantiate(SkinRowPrefab, skinRowsHolder);
                }
            }
        }

        public void OnBackButtonClick()
        {
            _sceneTransition.ChangeScene("MenuScene");
        }

        public void OnSkinSelect(Skin skin)
        {
            GameDataManager.Instance.UseSkin(skin);
            foreach (SkinButton skinBtn in _skinButtons)
            {
                if (!skinBtn.Disabled && skinBtn.Skin.Unlocked)
                {
                    skinBtn.UpdateSkinButtonVisualAndFunction(this);
                }
            }
        }
    }
}
