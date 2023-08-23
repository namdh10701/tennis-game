using ListExtensions;
using Phoenix;
using System;
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

        private List<SkinButton> _skinButtons;
        private List<Skin> _skins;

        public void Init(List<Skin> skins)
        {
            _skins = skins;
            CreateRows();

            _skinButtons = _scrollRect.GetComponentsInChildren<SkinButton>().ToList();
            HandleSkinButtons();

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
