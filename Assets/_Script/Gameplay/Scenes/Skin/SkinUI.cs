using ListExtensions;
using Phoenix;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
namespace Gameplay
{
    public class SkinUI : MonoBehaviour
    {
        [SerializeField] GameObject SkinRowPrefab;

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
            int numberOfRow = skins.Count / SKIN_PER_ROW + 1;
            if (numberOfRow > EXIST_SKIN_ROW)
            {
                for (int i = 0; i < numberOfRow - EXIST_SKIN_ROW; i++)
                {
                    Instantiate(SkinRowPrefab, skinRowsHolder);
                }
            }

            _skinButtons = _scrollRect.GetComponentsInChildren<SkinButton>().ToList();
            for (int i = 0; i < _skinButtons.Count - skins.Count; i++)
            {
                _skinButtons[_skinButtons.Count - 1 - i].Disalbe();
            }

            for (int i = 0; i < skins.Count; i++)
            {
                _skinButtons[i].Init(_skins[i]);
            }


        }

        public void OnBackButtonClick()
        {
            _sceneTransition.ChangeScene("MenuScene");
        }

        public void OnSkinClick()
        {

        }
    }
}
